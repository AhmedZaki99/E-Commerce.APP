# Phase 2 - security, authentication, and configuration

## Objective
Review the security posture of authentication, configuration, exception handling, and exposed operational surfaces.

## Key benchmark references
Context7 guidance used for comparison:
- ASP.NET Core recommends environment-specific exception handling and HSTS in production.
- ASP.NET Core authentication examples validate issuer/audience/signing keys and keep secrets out of source-controlled config.

## Findings

### 1. Sensitive secrets are committed in `appsettings.json`
File: `E-Commerce.APIs/appsettings.json`

Observed committed values include:
- SQL Server credentials in `ConnectionStrings`
- Redis credentials
- JWT signing key
- SMTP email/password

This is the single largest risk in the repository because it is already a compromise event, not just a bad practice.

### 2. OTP flows are effectively bypassable
File: `E-Commerce.App.Application/Service/Auth/AuthService.cs`

Relevant lines:
- registration OTP assignment around line 64
- forgot-password OTP assignment around line 108
- resend OTP assignment around line 222

Current behavior:
```csharp
user.Otp = "1234";
```

Impact:
- every OTP is predictable
- password reset and email verification are trivially bypassed
- there is no meaningful defense if the endpoint is reachable

### 3. Production errors leak internal implementation details
File: `E-Commerce.APIs/Middleware/ExceptionHandlerMiddleware.cs`

The default branch uses:
```csharp
var errorMessage = ex.ToString();
```
and returns it to the client. That exposes stack traces, type names, and potentially infrastructure details. Context7 ASP.NET Core guidance recommends generic production responses and server-side logging instead.

### 4. Exception inheritance causes unauthorized failures to masquerade as not-found errors
File: `E-Commerce.App.Application/Exception/UnAuthorizedExeption.cs`

`UnAuthorizedExeption` currently inherits from `NotFoundException`, which means the middleware routes it through the wrong branch and returns 404 semantics instead of 401/403 semantics.

### 5. Protected identity flows are incomplete
File: `E-Commerce-Api.Controller/Controllers/Account/AccountController.cs`

The only obvious current-user endpoint is commented out:
- commented `[Authorize]`
- commented `GetCurrentUser`

Across controllers, authorization usage is inconsistent:
- `OrdersController` is authorized
- `FavouriteController` is authorized
- `BasketController` is not authorized
- auth endpoints are public, which is expected, but user-context endpoints are incomplete

### 6. Null-forcing is used in security-sensitive claim access
File: `E-Commerce-Api.Controller/Controllers/Order/OrdersController.cs`

Examples:
```csharp
var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
await serviceManager.OrderService.CreateOrderAsync(buyerEmail!, orderDto);
```

If claims are absent or malformed, the API throws runtime errors instead of returning a controlled unauthorized/bad-request response.

### 7. Swagger is always exposed
File: `E-Commerce.APIs/Program.cs`

Swagger is enabled unconditionally:
```csharp
app.UseSwagger();
app.UseSwaggerUI();
```

That is convenient for development, but it should be intentionally controlled by environment or deployment policy.

### 8. Production hardening is incomplete
File: `E-Commerce.APIs/Program.cs`

Observed:
- `UseHttpsRedirection()` is enabled
- no production-only `UseHsts()`
- custom exception middleware is always used
- no visible CORS policy configuration

## Recommendations

### Most important to least important
1. **Rotate every committed secret immediately** and remove them from tracked configuration.
2. **Replace the hardcoded OTP implementation** with a secure generator and add retry/rate-limit protection.
3. **Stop returning `ex.ToString()` to clients**; return a generic 500 payload in production and keep details in logs only.
4. **Fix the authorization exception model** so unauthorized flows map correctly.
5. **Audit controller authorization requirements** and explicitly decide which basket/account endpoints must require an authenticated user.
6. **Guard all claim-based identity access** before using values in services.
7. **Restrict Swagger exposure** by environment or secured admin access.
8. **Add production security middleware/configuration** such as HSTS and explicit CORS policy handling.

### Easiest to hardest
1. Wrap Swagger in environment checks.
2. Fix `UnAuthorizedExeption` inheritance and middleware mapping.
3. Replace `.ToString()` client responses with generic problem payloads.
4. Replace OTP literals with secure random values.
5. Add null checks around claim extraction.
6. Move secrets to environment variables/user secrets/secret manager and scrub config history.
7. Add rate limiting/lockout to OTP verification flows.
8. Rework deployment and secret-management practices across environments.

## Notable hidden risks
- The restore step surfaced a dependency warning for `MimeKit 4.15.0` referenced by `E-Commerce.App.Application.Abstruction/E-Commerce.App.Application.Abstruction.csproj`.
- Because secrets are already committed, even a future code fix is not enough without credential rotation and history awareness.
