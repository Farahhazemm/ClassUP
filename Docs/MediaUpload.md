# Media Upload Flow — ClassUP

This document explains how image and video uploads are handled in ClassUP using Cloudinary as the media storage provider.

---

## Overview

ClassUP uses **[Cloudinary](https://cloudinary.com/)** for all media storage and transformation. There are two distinct media pipelines:

1. **Images** — profile pictures and course thumbnails, uploaded with size/dimension constraints and validated before upload.
2. **Videos** — course lecture videos, uploaded with eager transformations for multiple quality variants.

Both pipelines share the same Cloudinary account credentials via `CloudinarySettings`.

---

## Flow Diagram

```
-----------------------------------------------------
IMAGE UPLOAD
-----------------------------------------------------
Client sends IFormFile
        |
        v
ImageValidator.Validate()
        |
        +-- File null or empty?             -> 400 Bad Request
        +-- File size > max?                -> 400 Bad Request
        +-- File name contains / \ .. ?     -> 400 Bad Request
        +-- MIME type not whitelisted?      -> 400 Bad Request
        +-- Binary signature mismatch?      -> 400 Bad Request
        |
        v
CloudinaryService.UploadAsync(file, folder)
        |
        +-- Cloudinary returns non-200/201? -> 400 Bad Request
        |
        v
Returns { Url, PublicId }

-----------------------------------------------------
IMAGE DELETE
-----------------------------------------------------
CloudinaryService.DeleteAsync(publicId)
        |
        +-- PublicId null or empty?         -> Silent return
        |
        v
Cloudinary DestroyAsync (ResourceType: Image)

-----------------------------------------------------
VIDEO UPLOAD
-----------------------------------------------------
Client sends IFormFile
        |
        +-- File null or empty?             -> ArgumentException
        |
        v
VideoService.UploadAsync(file)
        |
        +-- Cloudinary returns non-200?     -> Exception
        |
        v
Cloudinary processes eager transformations:
  - 720x480 (fit)       -> playback quality
  - 160x90  (fill)      -> thumbnail preview, no audio
        |
        v
Returns { VideoUrl, PublicId }

-----------------------------------------------------
VIDEO DELETE
-----------------------------------------------------
VideoService.DeleteAsync(publicId)
        |
        +-- PublicId null or empty?         -> ArgumentException
        +-- Cloudinary returns non-200?     -> Exception
        |
        v
Cloudinary DestroyAsync (ResourceType: Video)
```

---

## Step-by-Step Breakdown

### 1. Image Validation (`ImageValidator`)

Before any image reaches Cloudinary, it passes through a whitelist-based validator that checks four layers of security:

**Layer 1 — Presence and size**

The file must exist and not exceed the configured maximum size defined in `ImageSettings.MaxFileSizeInBytes`.

**Layer 2 — File name safety**

The file name is checked for path traversal characters (`/`, `\`, `..`) to prevent directory traversal attacks where a malicious file name could potentially influence storage paths.

**Layer 3 — MIME type whitelist**

The `Content-Type` header is checked against an allowed list defined in `ImageSettings.AllowedMimeTypes` (e.g., `image/jpeg`, `image/png`).

**Layer 4 — Binary signature (magic bytes)**

The first 2 bytes of the file stream are read and compared against the known file signatures for the declared MIME type, stored in `ImageSettings.FileSignatures`. This prevents a malicious file from bypassing validation by simply setting a fake `Content-Type` header — the actual binary content must match.

---

### 2. Image Upload (`CloudinaryService.UploadAsync`)

Once validation passes, the image is streamed directly to Cloudinary using `ImageUploadParams`:

| Parameter | Value |
|-----------|-------|
| `File` | File stream from `IFormFile` |
| `Folder` | Caller-supplied path (e.g., `users/{userId}` or `courses/{courseId}`) |
| `Transformation` | Width: 800, Height: 800, Crop: `limit` |

The `limit` crop mode resizes the image only if it exceeds 800x800, preserving aspect ratio and never upscaling. This keeps file sizes reasonable without distorting images that are already small.

On success, Cloudinary returns a `SecureUrl` (HTTPS) and a `PublicId` which are stored in the database for future reference and deletion.

---

### 3. Image Deletion (`CloudinaryService.DeleteAsync`)

When an image needs to be replaced or removed, the stored `PublicId` is passed to `DestroyAsync`. If the `PublicId` is null or empty, the method returns silently — this handles cases where an entity never had an image without throwing an error.

---

### 4. Video Upload (`VideoService.UploadAsync`)

Videos are uploaded with a random `Guid` as the `PublicId` to avoid naming collisions. Two **eager transformations** are configured and processed asynchronously by Cloudinary after the upload completes:

| Variant | Resolution | Crop | Audio | Purpose |
|---------|-----------|------|-------|---------|
| Playback | 720 x 480 | `fit` | Yes | Standard video playback |
| Thumbnail | 160 x 90 | `fill` | No | Preview thumbnail |

`EagerAsync = true` means Cloudinary processes these transformations in the background — the upload call returns as soon as the original file is stored, without waiting for all variants to be ready. The `SecureUrl` returned points to the original uploaded video over HTTPS.

---

### 5. Video Deletion (`VideoService.DeleteAsync`)

The stored `PublicId` is passed to Cloudinary's `DestroyAsync` with `ResourceType.Video`. Unlike image deletion, this method throws an `ArgumentException` if the `PublicId` is missing and throws an `Exception` if Cloudinary returns a non-200 response.

---

## Security Notes

| Concern | Mitigation |
|---------|-----------|
| Fake file type via MIME spoofing | Binary signature (magic bytes) check on every image upload |
| Path traversal via file name | File name checked for `/`, `\`, `..` characters |
| Oversized uploads | Max file size enforced before the stream is opened |
| Insecure media URLs | `_cloudinary.Api.Secure = true` and `SecureUrl` used throughout |
| Public ID collisions for videos | Random `Guid` assigned as PublicId on every upload |

---

## Folder Structure in Cloudinary

Images are organized into folders passed in by the calling service:

```
users/
  +-- {userId}/        <- profile pictures
courses/
  +-- {courseId}/      <- course thumbnails
```

Videos use flat storage with a GUID as the public ID.

---

## Configuration

```json
{
  "CloudinarySettings": {
    "CloudName": "<your-cloud-name>",
    "ApiKey": "<your-api-key>",
    "ApiSecret": "<your-api-secret>"
  }
}
```

> Never commit real API keys or secrets to source control. Use environment variables, `dotnet user-secrets` for local development, or a secrets manager such as Azure Key Vault in production.

---

## Key Classes

| Class / Interface | Responsibility |
|-------------------|---------------|
| `CloudinaryService` | Image upload and deletion via Cloudinary |
| `VideoService` | Video upload and deletion via Cloudinary |
| `ImageValidator` | Pre-upload validation (size, MIME type, binary signature) |
| `CloudinarySettings` | Strongly-typed Cloudinary configuration binding |
| `ImageSettings` | Allowed MIME types, file signatures, and size limits |