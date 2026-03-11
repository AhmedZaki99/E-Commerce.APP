# Identity & Authentication

The system uses **ASP.NET Core Identity** for user management and **JWT Bearer Tokens** for securing API endpoints.

## Components

### 1. ApplicationsUser
Extends `IdentityUser` to include custom properties (e.g., DisplayName).

### 2. AuthService
Handles the business logic for:
- User registration and login.
- JWT generation (using `JWTSettings` from configuration).
- Email verification (via `EmailService`).
- OTP management for password resets.

### 3. Middleware Configuration
Authentication is configured in `Program.cs` to use JWT Bearer defaults. The system validates:
- Issuer and Audience.
- Token Lifetime.
- Signing Key security.

## Protecting Endpoints
To secure a controller or action, use the `[Authorize]` attribute. For example:
```csharp
[Authorize]
public class OrdersController : BaseApiController { ... }
```

## DTOs
Authentication uses specific DTOs to prevent leaking sensitive identity fields:
- `LoginDto`
- `RegisterDto`
- `AuthResponseDto` (contains the Token and User info)
