# Payment Flow — ClassUP

This document explains how payments are handled in ClassUP, covering the full lifecycle from initiating a purchase to enrolling a user in a course.

---

## Overview

ClassUP uses **[Paymob](https://paymob.com/)** as its payment gateway. The payment flow is split into two parts:

1. **Checkout Initiation** — The server creates an order and returns a Paymob-hosted payment URL.
2. **Webhook Handling** — Paymob notifies the server of the payment result, and the server enrolls the user accordingly.

---

## Flow Diagram

```
User clicks "Enroll"
        |
        v
[POST /payments/create]
        |
        +-- Course not found?          -> 404 Not Found
        +-- Already enrolled?          -> 400 Bad Request
        +-- Price == 0?                -> Enroll directly, return { isFreeCourse: true }
        +-- Pending order exists?      -> 400 Bad Request
        |
        v
Create Order in DB (status: Pending)
        |
        v
Call Paymob API
  1. Get Auth Token
  2. Create Paymob Order
  3. Generate Payment Key
        |
        +-- Paymob call fails?         -> Cancel DB order, re-throw exception
        |
        v
Return { orderId, paymentUrl }
        |
        v
User completes payment on Paymob iframe
        |
        v
Paymob sends webhook -> [POST /payments/webhook]
        |
        +-- Invalid HMAC?              -> 400 Rejected
        +-- Duplicate transaction?     -> 200 Ignored (idempotent)
        +-- Invalid MerchantOrderId?   -> 400 Bad Request
        +-- Order not found?           -> 404 Not Found
        +-- Order already completed?   -> 200 Ignored
        |
        v
Persist Payment record
Update Order status (Completed / Cancelled)
        |
        +-- Payment successful?        -> Create Enrollment record
        |
        v
Done
```

---

## Step-by-Step Breakdown

### 1. Checkout Initiation (`CreatePaymentAsync`)

When a user requests to enroll in a course, the following happens:

**Validation**
- The course must exist.
- The user must not already be enrolled in the course.
- There must be no existing `Pending` order for the same user + course combination.

**Free Course Shortcut**

If `course.Price == 0`, the user is enrolled immediately without going through Paymob, and a response of `{ isFreeCourse: true }` is returned.

**Order Creation**

A new `Order` record is saved to the database with:
- `Status`: `Pending`
- `OrderItems`: a single item referencing the course and its price

**Paymob API Calls**

Three sequential calls are made to Paymob:

| Step | Endpoint | Purpose |
|------|----------|---------|
| 1 | `POST /api/auth/tokens` | Obtain a short-lived auth token using the API key |
| 2 | `POST /api/ecommerce/orders` | Register the order with Paymob; links back to our DB order via `merchant_order_id` |
| 3 | `POST /api/acceptance/payment_keys` | Generate a one-time payment key tied to the order, amount, and user billing data |

**Error Handling**

If any Paymob call fails, the DB order is immediately set to `Cancelled` and the exception is re-thrown, preventing orphaned pending orders.

**Response**

On success, the server returns:
```json
{
  "orderId": 42,
  "paymentUrl": "https://accept.paymob.com/api/acceptance/iframes/{iframeId}?payment_token=..."
}
```

The client redirects the user to this URL to complete payment on Paymob's hosted iframe.

---

### 2. Webhook Handling (`HandleWebhookAsync`)

After the user completes (or fails) payment, Paymob sends a POST webhook to the server.

**HMAC Verification**

Before processing anything, the webhook payload is validated using HMAC-SHA512. The signature is computed over a fixed set of transaction fields (in the exact order required by Paymob's spec) and compared against the `hmac` field in the request using a constant-time comparison to prevent timing attacks.

If the HMAC is invalid, the request is rejected immediately.

**Idempotency Check**

The transaction ID (`obj.Id`) is checked against existing `Payment` records. If a record with the same transaction ID already exists, the webhook is silently ignored. This ensures that duplicate webhook deliveries (which Paymob may send) do not cause duplicate enrollments or double-processing.

**Payment Outcome**

A payment is considered successful when:
```
obj.Success == true  AND  obj.Pending == false
```

**Database Updates**

Regardless of outcome, a `Payment` record is persisted with:
- `TransactionId`
- `OrderId`
- `UserId`
- `Amount` (converted from cents)
- `Status`: `"Success"` or `"Failed"`

The linked `Order` status is updated to `Completed` or `Cancelled`.

**Enrollment**

If the payment succeeded, an `Enrollment` record is created for the user and course (guarded by a second existence check to prevent duplicates).

---

## Security Notes

| Concern | Mitigation |
|---------|-----------|
| Webhook spoofing | HMAC-SHA512 signature verified on every webhook |
| Timing attacks | `CryptographicOperations.FixedTimeEquals` used for HMAC comparison |
| Duplicate processing | Transaction ID idempotency check before any DB writes |
| Orphaned orders | Paymob failures immediately cancel the pending DB order |
| Double enrollment | `ExistsAsync` check before creating any `Enrollment` record |

---

## Configuration

The following values must be present in `appsettings.json` (or environment variables):

```json
{
  "Paymob": {
    "ApiKey": "<your-paymob-api-key>",
    "IntegrationId": 123456,
    "IframeId": 654321,
    "HmacSecret": "<your-hmac-secret>"
  }
}
```

> Never commit real API keys or secrets to source control. Use secrets management (e.g., Azure Key Vault, environment variables, or `dotnet user-secrets` for local development).

---

## Key Classes

| Class / Interface | Responsibility |
|-------------------|---------------|
| `PaymobService` | Orchestrates checkout and webhook handling |
| `IPaymobClient` | Refit HTTP client for Paymob API calls |
| `PaymobHmacService` | Validates webhook HMAC signatures |
| `PaymobSettings` | Strongly-typed configuration binding |