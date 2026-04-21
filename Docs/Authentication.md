# Authentication & Identity Flow — ClassUP

This document explains how user authentication, token management, and email-based flows work in ClassUP.

---

## Overview

ClassUP uses **ASP.NET Core Identity** for user management combined with **JWT** for stateless authentication and **refresh tokens** for session persistence. Email delivery is handled via **MailKit** (SMTP), with confirmation and password-reset emails dispatched through **Hangfire** background jobs where appropriate.

The system covers six main flows:

1. Registration
2. Email Confirmation
3. Login
4. Token Refresh
5. Logout / Token Revocation
6. Forgot Password / Reset Password

---

## Flow Diagram

```
-----------------------------------------------------
REGISTRATION
-----------------------------------------------------
POST /auth/register
        |
        +-- Identity creates user
        +-- Assigns default role (User)
        +-- Generates email confirmation token (Base64Url encoded)
        +-- Dispatches confirmation email via Hangfire background job
                |
                v
        Returns UserDTO (id, name, email, roles)

-----------------------------------------------------
EMAIL CONFIRMATION
-----------------------------------------------------
GET /auth/emailConfirmation?userId=...&code=...
        |
        +-- User not found?             -> Invalid code error
        +-- Email already confirmed?    -> Invalid code error
        +-- Code format invalid?        -> Invalid code error
        +-- Identity confirms email token
                |
                v
        Email marked as confirmed

-----------------------------------------------------
LOGIN
-----------------------------------------------------
POST /auth/login
        |
        +-- User not found?             -> Invalid credentials
        +-- Account disabled?           -> Disabled user error
        +-- Email not confirmed?        -> Email not confirmed error
        +-- Wrong password (lockout)?   -> Account locked out
        +-- Wrong password?             -> Invalid credentials
        |
        v
Generate JWT + Refresh Token
Store refresh token on user record
        |
        v
Returns { token, refreshToken, refreshTokenExpiresAt }

-----------------------------------------------------
TOKEN REFRESH
-----------------------------------------------------
POST /auth/refresh
        |
        +-- Token not found?            -> Invalid refresh token
        +-- Account disabled?           -> Disabled user error
        +-- Account locked out?         -> Locked out error
        +-- Token inactive/expired?     -> Invalid refresh token
        |
        v
Revoke old refresh token (set RevokedOn)
Generate new refresh token + new JWT
        |
        v
Returns { jwtToken, refreshToken, refreshTokenExpiration }

-----------------------------------------------------
LOGOUT / TOKEN REVOCATION
-----------------------------------------------------
POST /auth/revoke          -> Revoke a single token
POST /auth/logout          -> Revoke ALL active tokens for user
        |
        v
Tokens marked as revoked (RevokedOn = UtcNow)

-----------------------------------------------------
FORGOT PASSWORD
-----------------------------------------------------
POST /auth/forgotPassword
        |
        +-- User not found?             -> Silent return (no enumeration)
        +-- Email not confirmed?        -> Email not confirmed error
        |
        v
Generate reset token (Base64Url encoded)
Send reset password email (synchronous SMTP)

-----------------------------------------------------
RESET PASSWORD
-----------------------------------------------------
POST /auth/resetPassword
        |
        +-- User not found?             -> Bad request
        +-- Email not confirmed?        -> Email not confirmed error
        +-- Code format invalid?        -> Bad request
        +-- Identity reset fails?       -> Identity operation error
        |
        v
Password updated successfully
```

---

## Step-by-Step Breakdown

### 1. Registration (`RegisterAsync`)

**Steps**
1. An `AppUser` object is constructed from the submitted `RegisterDTO`.
2. ASP.NET Identity creates the user with a hashed password via `CreateAsync`.
3. The user is assigned the default `User` role via `AddToRoleAsync`. If role assignment fails, the user record is deleted to avoid a role-less account.
4. An email confirmation token is generated (see [Email Confirmation](#2-email-confirmation-confirmemailasync) below).
5. A confirmation email is enqueued as a **Hangfire background job** so the HTTP response is not blocked by the SMTP call.
6. A `UserDTO` is returned containing the user's ID, full name, email, profile picture, bio, and assigned roles.

---

### 2. Email Confirmation (`ConfirmEmailAsync`)

**Token Generation (`GenerateVerificationCodeAsync`)**

Identity's `GenerateEmailConfirmationTokenAsync` produces a token which is then Base64Url-encoded before being embedded in the confirmation link. This makes the token URL-safe.

**Confirmation Link Format**
```
{origin}/auth/emailConfirmation?userId={userId}&code={base64UrlToken}
```
The `origin` is read from the incoming request's `Origin` header, making the link environment-agnostic (works across dev, staging, and production).

**Email Dispatch**

The HTML email is built from a template file located at:
```
wwwroot/EmailTemplates/EmailConfirmation.html
```
Placeholders `{{name}}` and `{{action_url}}` are replaced with the user's first name and the confirmation link. The email is dispatched via a **Hangfire background job**.

**Confirmation (`ConfirmEmailAsync`)**

On clicking the link, the server decodes the Base64Url token back to UTF-8 and passes it to Identity's `ConfirmEmailAsync`. The user's `EmailConfirmed` flag is set to `true`.

---

### 3. Login (`LoginAsync`)

**Validation chain (in order)**
1. User lookup by email — fails with a generic `InvalidCredentialsException` (no user enumeration).
2. Disabled account check.
3. Email confirmation check.
4. Password verification via `SignInManager.CheckPasswordSignInAsync` with `lockoutOnFailure: true` — failed attempts increment the lockout counter.
5. Explicit lockout status check.

**Token Issuance**

On successful authentication, two tokens are issued:

| Token | Lifetime | Purpose |
|-------|----------|---------|
| JWT | Configurable (`JwtOptions.Lifetime` minutes) | Stateless API access |
| Refresh Token | 10 days | Obtain new JWTs without re-login |

The refresh token is a cryptographically random 32-byte value (Base64-encoded), stored in the user's `RefreshTokens` collection in the database.

**JWT Claims**

The JWT contains the following claims:

| Claim | Value |
|-------|-------|
| `sub` / `nameidentifier` | User ID |
| `unique_name` / `name` | Username |
| `email` | User email |
| `role` | All assigned roles |
| Any custom claims attached to the user |

The token is signed with **HMAC-SHA256** using the key from `JwtOptions.SigningKey`.

---

### 4. Token Refresh (`RefreshTokenAsync`)

When a JWT expires, the client submits the refresh token to obtain a new pair without requiring the user to log in again.

**Process**
1. The user record is located by matching the submitted token against all stored refresh tokens.
2. Disabled and locked-out account checks are performed.
3. The token's `IsActive` status is verified (not expired and not revoked).
4. The old token is revoked by setting `RevokedOn = UtcNow`.
5. A new refresh token is generated and stored.
6. A new JWT is generated.

This implements **refresh token rotation** — each refresh produces a completely new token pair, limiting the window of exposure if a refresh token is stolen.

---

### 5. Token Revocation

**Single Token (`RevokeTokenAsync`)** — Used to invalidate one specific refresh token (e.g., "log out this device").

**All Tokens (`RevokeAllAsync`)** — Revokes every active refresh token for a user. Used for full logout or security-triggered session termination (e.g., password change).

Revocation sets `RevokedOn = UtcNow` on the token record. The `IsActive` property on `RefreshToken` evaluates to `false` when either `RevokedOn` is set or `ExpiresOn` has passed.

---

### 6. Forgot Password (`SendResetPasswordCode`)

**User enumeration protection**: if the email is not found, the method returns silently without throwing an error. This prevents attackers from probing which emails are registered.

If the user exists and their email is confirmed, a password reset token is generated via Identity's `GeneratePasswordResetTokenAsync`, Base64Url-encoded, and embedded in a reset link:

```
{origin}/auth/forgotpassword?email={email}&code={base64UrlToken}
```

The reset email is built from the template at:
```
wwwroot/ResetPasswordTemplates/ForgotPassword.html
```

Unlike confirmation emails, the reset email is sent **synchronously** (not via a background job).

---

### 7. Reset Password (`ResetPasswordAsync`)

The submitted code is decoded from Base64Url and passed to Identity's `ResetPasswordAsync` along with the new password. Identity validates the token's integrity and expiry before applying the change.

---

## Email Service (`EmailService`)

All emails are sent via **MailKit** over SMTP with `StartTls`. The service accepts a recipient address, subject, and pre-built HTML body, then connects, authenticates, sends, and disconnects within a single call.

**Configuration** (`appsettings.json`):
```json
{
  "MailSettings": {
    "Mail": "noreply@classup.com",
    "DisplayName": "ClassUP",
    "Password": "<smtp-password>",
    "Host": "smtp.example.com",
    "Port": 587
  }
}
```

---

## JWT Configuration

```json
{
  "JwtOptions": {
    "Issuer": "ClassUP",
    "Audience": "ClassUP-Users",
    "SigningKey": "<your-secret-key>",
    "Lifetime": 60
  }
}
```

`Lifetime` is in minutes. Refresh tokens are hard-coded to 10 days.

> Never commit real signing keys, SMTP passwords, or secrets to source control. Use environment variables, `dotnet user-secrets` for local development, or a secrets manager such as Azure Key Vault in production.

---

## Security Notes

| Concern | Mitigation |
|---------|-----------|
| Password brute force | Lockout enabled on failed sign-in (`lockoutOnFailure: true`) |
| User enumeration on login | Generic `InvalidCredentialsException` regardless of whether user exists |
| User enumeration on forgot password | Silent return when email is not found |
| Token theft | Refresh token rotation — old token revoked on every refresh |
| Token replay after logout | All active tokens revoked on logout |
| Email token tampering | Tokens are Identity-generated (cryptographically signed) and Base64Url-encoded |
| SMTP credential exposure | Credentials loaded from configuration, not hardcoded |

---

## Key Classes

| Class / Interface | Responsibility |
|-------------------|---------------|
| `AuthService` | Orchestrates registration, login, confirmation, and password reset flows |
| `UserTokenService` | JWT and refresh token generation, rotation, and revocation |
| `EmailVerificationService` | Generates and dispatches email confirmation tokens |
| `ResetPasswordService` | Generates and dispatches password reset tokens |
| `EmailService` | Low-level SMTP email sending via MailKit |
| `JwtOptions` | Strongly-typed JWT configuration binding |
| `MailSettings` | Strongly-typed SMTP configuration binding |