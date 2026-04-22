# ClassUP — Course Management System

ClassUP is a scalable course management API built with ASP.NET Core following Clean Architecture. It supports JWT authentication, payments via Paymob, media uploads via Cloudinary, and full course lifecycle management — covering everything from content creation to student enrollment and progress tracking.

---

## Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
- [Docker](#docker)
- [Configuration](#configuration)
- [Authentication](#authentication)
- [API Modules](#api-modules)
- [Documentation](#documentation)

---

## Features

| Module | Highlights |
|--------|-----------|
| Authentication | JWT + refresh token rotation, lockout, email confirmation, password reset |
| Course Management | Full CRUD, thumbnail upload, filtering, pagination, category & instructor views |
| Payments | Paymob iframe flow, HMAC-SHA512 webhook verification, idempotent processing |
| Media Upload | Image validation (magic bytes), Cloudinary upload with transformations |
| Progress Tracking | Mark/unmark lectures, auto progress recalculation, completion detection |
| Enrollment | Free & paid enrollment, unenrollment, status check |
| Reviews | Per-course reviews with rating, owned by student |
| User Management | Admin CRUD, role assignment, account enable/disable |

---

## Architecture

The solution follows **Clean Architecture** with a strict separation of concerns across four projects:

```
ClassUP.API                  <- Controllers, Middleware, DI registration
ClassUP.ApplicationCore      <- Business logic, Services, DTOs, Interfaces
ClassUP.Domain               <- Entities, Enums, Constants (no dependencies)
ClassUP.Infrastructure       <- EF Core, Identity, Cloudinary, Email, Paymob
```

**Dependency rule:** each layer depends only on layers below it. `Domain` has zero external dependencies. `Infrastructure` implements interfaces defined in `ApplicationCore`.

The **Unit of Work** pattern coordinates repository operations and ensures atomic database transactions.

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Framework | ASP.NET Core 8 Web API |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Authentication | ASP.NET Core Identity + JWT Bearer |
| Background Jobs | Hangfire |
| Media Storage | Cloudinary (images & videos) |
| Payment Gateway | Paymob |
| Email | MailKit (SMTP + StartTLS) |
| HTTP Client | Refit |
| Object Mapping | Mapster |
| Containerization | Docker / Docker Compose |
| Architecture | Clean Architecture / Unit of Work / Repository Pattern |

---

## Getting Started

### Prerequisites

- [Docker](https://www.docker.com/) and Docker Compose (recommended)
- Or: .NET 8 SDK + SQL Server for local development

### Run with Docker (Recommended)

**1. Clone the repository**
```bash
git clone https://github.com/Farahhazemm/ClassUP.git
cd ClassUP
```

**2. Create a `.env` file** in the root directory (see [Configuration](#configuration) below).

**3. Start all services**
```bash
docker compose up --build
```

The API will be available at `http://localhost:{port}`. Swagger UI is available at `/swagger`.

### Run Locally (Without Docker)

**1. Clone and configure**
```bash
git clone https://github.com/Farahhazemm/ClassUP.git
cd ClassUP
```

Set the required values in `appsettings.json` (see [Configuration](#configuration) below).

**2. Apply database migrations**
```bash
cd ClassUP.API
dotnet ef database update
```

**3. Run the API**
```bash
dotnet run --project ClassUP.API
```

---

## Docker

The project includes a `docker-compose.yml` that runs the API and SQL Server together.

### Services

| Service | Description |
|---------|-------------|
| `api` | ASP.NET Core Web API |
| `db` | SQL Server 2022 |

### Useful Commands

```bash
# Build and start all containers
docker compose up --build

# Start in detached mode
docker compose up -d

# Stop all containers
docker compose down

# View API logs
docker compose logs -f api

# Rebuild only the API image
docker compose up --build api
```

### Hangfire Dashboard

Once running, the Hangfire background job dashboard is available at:
```
http://localhost:{port}/hangfire
```
Login with the credentials set in `HangfireSettings__username` and `HangfireSettings__password`.

---

## Configuration

The application is configured via environment variables, making it fully compatible with Docker. Create a `.env` file in the project root:

```env
# DATABASE
ConnectionStrings__MyConc=Server=YOUR_SERVER;Database=YOUR_DB;User Id=YOUR_USER;Password=YOUR_PASSWORD
ConnectionStrings__Hangfire=Server=YOUR_SERVER;Database=YOUR_DB;User Id=YOUR_USER;Password=YOUR_PASSWORD

# JWT
JWT__SigningKey=YOUR_SECRET_KEY
JWT__Issuer=ClassUP
JWT__Audience=ClassUPClient
JWT__Lifetime=60

# CLOUDINARY
CloudinarySettings__CloudName=YOUR_CLOUD_NAME
CloudinarySettings__ApiKey=YOUR_API_KEY
CloudinarySettings__ApiSecret=YOUR_API_SECRET

# MAIL
MailSettings__Mail=YOUR_EMAIL
MailSettings__DisplayName=ClassUP
MailSettings__Host=smtp.example.com
MailSettings__Port=587
MailSettings__Password=YOUR_PASSWORD

# ADMIN SEED
Admin__Email=admin@example.com
Admin__Password=YOUR_ADMIN_PASSWORD

# PAYMOB
Paymob__ApiKey=YOUR_API_KEY
Paymob__IntegrationId=YOUR_ID
Paymob__IframeId=YOUR_ID
Paymob__HmacSecret=YOUR_SECRET

# HANGFIRE DASHBOARD
HangfireSettings__username=YOUR_USERNAME
HangfireSettings__password=YOUR_PASSWORD
```

> Never commit a real `.env` file to source control. Add it to `.gitignore`. Use Docker secrets or a secrets manager like Azure Key Vault in production.

| Variable | Description |
|----------|-------------|
| `ConnectionStrings__MyConc` | Main SQL Server connection string |
| `ConnectionStrings__Hangfire` | Hangfire job storage connection string |
| `JWT__Lifetime` | JWT expiry in minutes |
| `Admin__Email` / `Admin__Password` | Seeded admin account credentials |
| `Paymob__IntegrationId` | Paymob card payment integration ID |
| `Paymob__IframeId` | Paymob iframe ID for hosted checkout |
| `HangfireSettings__username` | Hangfire dashboard login username |
| `HangfireSettings__password` | Hangfire dashboard login password |

---

## Authentication

ClassUP uses **JWT Bearer tokens** for API authentication.

**Login flow:**
1. `POST /api/account/login` returns a short-lived JWT and a 10-day refresh token stored in an `HttpOnly` cookie.
2. Include the JWT in the `Authorization` header of all protected requests:
   ```
   Authorization: Bearer <token>
   ```
3. When the JWT expires, call `GET /api/account/refresh-token` to get a new pair.
4. On logout, call `DELETE /api/account/logout` to revoke all active sessions.

**Roles:**

| Role | Access |
|------|--------|
| `User` | Default role — browse courses, enroll, review, track progress |
| `Instructor` | Create and manage their own courses, sections, and lectures |
| `Admin` | Full access — manage users, roles, all courses, and categories |

---

## API Modules

| Controller | Base Route | Auth | Description |
|------------|-----------|------|-------------|
| AccountController | `/api/account` | Public / Authorized | Register, login, logout, refresh token, revoke token, forgot/reset password |
| EmailVerificationController | `/api/emailverification` | Public | Confirm email, resend confirmation email |
| Account_ManagementController | `/Me` | Authorized | Get profile, update profile info, update profile image, change password |
| CoursesController | `/api/courses` | Mixed | Get all courses, get by ID, get by category, get my courses, create, update, delete |
| SectionController | `/api/section` | Mixed | Create, update, delete section; get by ID; get sections by course |
| LecturesController | `/api/lectures` | Mixed | Get all, get by ID, get by section, create, update, delete lecture; upload/delete video |
| EnrollmentController | `/api/enrollment` | Authorized | Get all (Admin), get my enrollments, get by ID, check enrollment, unenroll |
| LectureProgressController | `/api/lectureprogress` | Authorized | Mark complete, unmark complete, check completion, get completed lectures, recalculate progress |
| PaymentController | `/api/payment` | Mixed | Create payment (free or Paymob), webhook verification (GET), webhook handler (POST) |
| ReviewsController | `/api/reviews` | Mixed | Add review, get course reviews, update review, delete review |
| CategoriseController | `/api/categorise` | Mixed | Get all, get by ID (public); create, update, delete (Admin only) |
| User_ManagementController | `/api/user_management` | Admin only | Get all users, get by ID, create user, update user, toggle account status |

---

## Documentation

Detailed flow documentation for the core systems is available in the `/docs` folder:

| Document | Description |
|----------|-------------|
| [AUTH_FLOW.md](./Docs/Authentication.md) | Authentication, token management, email confirmation, password reset |
| [PAYMENT_FLOW.md](./Docs/Payment.md) | Paymob payment integration, webhook handling, enrollment on success |
| [MEDIA_UPLOAD_FLOW.md](./Docs/MediaUpload.md) | Image validation, Cloudinary image and video upload pipelines |
