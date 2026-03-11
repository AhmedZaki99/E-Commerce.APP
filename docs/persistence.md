# Persistence & Data Layer

This document describes how data is managed and stored in the application.

## Database Contexts

The application uses two separate DbContexts to isolate concerns:

1. **StoreDbContext**: Manages core business data (Products, Categories, Vendors, Orders).
2. **StorIdentityDbContext**: Manages user authentication and identity data (Users, Roles, Addresses).

## Repository Implementation

We utilize a **Generic Repository** pattern (`IGenericRepositieries<TEntity, TKey>`) which provides standard methods:
- `GetAsync(id)`
- `GetAllAsync()`
- `GetWithSpecAsync(spec)`
- `GetAllSpecAsync(spec)`

The **Unit of Work** (`IUnitOfWork`) manages the lifecycle of these repositories and ensures that `SaveChangesAsync()` is called in a transactional manner.

## Specification Evaluator

The `SpecificationsEvaluator` is a static helper that translates our Domain Specifications into IQueryable expressions for Entity Framework. It handles:
- **Criteria**: Where clauses.
- **Includes**: Eager loading of navigation properties.
- **Ordering**: Ascending and Descending.
- **Pagination**: Skip and Take logic.

## Data Seeding

On application startup, the `StoreContextIntializer` and `StoreIdentityContextIntializer` perform the following:
1. Apply any pending migrations.
2. Check for existing data.
3. If empty, seed data from JSON files located in `E-Commerce.APIs/Seeds` or Infrastructure seed directories.

### Seed Files:
- `categories.json`
- `brands.json` / `vendors.json`
- `products.json`
- `delivery.json`
