# Phase 3 - application layer, data access, and performance

## Objective
Review the micro-level code quality of services, mapping, repository usage, and EF Core behavior.

## Key benchmark references
Context7 guidance used for comparison:
- EF Core warns that lazy loading can create N+1 query patterns and recommends explicit query shaping for predictable performance.
- AutoMapper guidance favors safe null handling and mapping validation, especially where custom member resolvers are used.

## Findings

### 1. Async blocking exists in the auth flow
File: `E-Commerce.App.Application/Service/Auth/AuthService.cs`

In `GetCurrentUser`:
```csharp
var user = userManager.FindByEmailAsync(Email!).Result;
```

This mixes async and blocking behavior, which is a classic scalability and deadlock risk.

### 2. Several code paths rely on null-forcing instead of explicit validation
Files:
- `E-Commerce.App.Application/Service/Auth/AuthService.cs`
- `E-Commerce.App.Application/Mapping/MappingProfile.cs`
- `E-Commerce.App.Application/Service/OrderService/OrderService.cs`
- `E-Commerce-Api.Controller/Controllers/Order/OrdersController.cs`

Examples:
- `Email!`
- `user!`
- `src.vendor!.Name`
- `product.vendor!.Name`

These suppress compiler warnings without proving the values are actually safe.

### 3. Auth and email flows are not resilient
Files:
- `E-Commerce.App.Application/Service/Auth/AuthService.cs`
- `E-Commerce.App.Application/Service/Auth/EmailService.cs`

Observed issues:
- `IEmailService.SendEmail` is synchronous and returns `void`
- SMTP operations are blocking
- calling code assumes email succeeded if no exception bubbles up
- OTP/email workflows do not track retries, failures, or delivery state

### 4. Order creation can silently degrade instead of failing fast
File: `E-Commerce.App.Application/Service/OrderService/OrderService.cs`

In the order-item loop, missing products are skipped:
```csharp
if (product != null)
{
    ...
    orderItems.Add(orderItem);
}
```

That can produce incomplete orders without explicitly telling the caller that basket contents were invalid.

### 5. Order creation trusts current product state too directly
File: `E-Commerce.App.Application/Service/OrderService/OrderService.cs`

The order item price is taken from the current product record at creation time:
```csharp
Price = product.Price
```

That may be intentional, but if the business expectation is “checkout uses the current catalog value,” it should be documented. If not, price consistency rules need to be made explicit.

### 6. Global lazy loading undermines the specification/repository pattern
File: `E_Commerce.App.Infrastructre.presistent/DependencyInjection.cs`

Both DbContexts enable:
```csharp
.UseLazyLoadingProxies()
```

This weakens one of the main benefits of the specification pattern: explicit, reviewable query shape. The risk is especially visible in mappings and services that traverse navigation properties.

### 7. Generic repository specification methods ignore the `WithTracking` intent
File: `E_Commerce.App.Infrastructre.presistent/Repositieries/Generic Repository/GenericRepositiries.cs`

`GetAllSpecAsync(ISpecifications<TEntity, TKey> spec, bool WithTracking = false)` accepts a tracking parameter but does not use it when applying the specification query.

That is a correctness/maintainability smell because the method signature promises behavior it does not implement.

### 8. Mapping configuration contains null-sensitive expressions
File: `E-Commerce.App.Application/Mapping/MappingProfile.cs`

Examples:
- `src.vendor!.Name`
- `src.Category!.Name`
- `src.DeliveryMethod!.ShortName`

AutoMapper best-practice guidance explicitly warns that custom member resolution needs safe null handling or validation.

### 9. Unit of work repository caching is clever but not especially clean
File: `E_Commerce.App.Infrastructre.presistent/Repositieries/UnitOfWork.cs`

The use of `ConcurrentDictionary` is not inherently wrong, but it adds complexity in a scoped service where simpler lifecycle management would likely be enough.

### 10. Pagination guards are partial, not complete
File: `E-Commerce.App.Application.Abstruction/Models/Product/ProductSpecParams.cs`

Current behavior clamps large page sizes to `PageMaxSize`, which is good, but it does not guard negative or zero values explicitly. It is a decent prototype safeguard, not a full request-validation strategy.

## Recommendations

### Most important to least important
1. **Remove `.Result` and other sync-over-async patterns** in service code.
2. **Replace null-forcing with real validation or safe mapping logic** in auth, mapping, and order flows.
3. **Decide whether global lazy loading is acceptable**; if not, turn it off and use explicit includes/specifications.
4. **Make repository method contracts truthful** by honoring the tracking flag or removing it.
5. **Harden order creation** so missing catalog data causes a controlled failure instead of silent partial behavior.
6. **Modernize email delivery code** to use async APIs and clearer failure semantics.
7. **Validate AutoMapper configuration during development** and review custom resolvers for null safety.
8. **Tighten request parameter validation** for pagination and basket identifiers.

### Easiest to hardest
1. Replace `.Result` with `await`.
2. Add null checks before claim and navigation access.
3. Add AutoMapper validation in development builds/tests.
4. Fix misleading repository signatures.
5. Add request validation rules for product/basket/order DTOs and parameters.
6. Refactor email sending to async.
7. Rework lazy-loading usage and specification boundaries.
8. Revisit repository/unit-of-work abstractions if EF Core usage remains heavily query-driven.

## Micro-quality summary
The application layer is readable enough to follow, but it is not yet robust. The main problems are not exotic algorithms; they are reliability fundamentals: null-safety, async correctness, consistent validation, and predictable data-access behavior.
