# Phase 2 Code Review: Domain Layer & Persistence

## Overview
This phase reviews the core Domain entities, the Specification pattern, and the Infrastructure/Persistence implementation (EF Core, Repository, Unit of Work).

---

## 🚩 Critical Findings

### 1. Specification Evaluator Ordering Bug
**Importance: High | Difficulty: Easy**
In `SpecificationsEvaluator.cs`, the `Include` statements are applied *after* `Skip` and `Take`.
- **Issue**: In some EF Core versions and database providers, applying `Include` after paging can lead to inefficient queries or unexpected behavior. More importantly, the current implementation applies `OrderBy`/`Skip`/`Take` and THEN aggregates Includes.
- **Recommendation**: Apply `Includes` before `Skip` and `Take` to ensure the execution plan is optimized for the full entity graph being retrieved.

### 2. Leaky Abstractions in Domain (Expressions)
**Importance: Medium | Difficulty: Medium**
The `ISpecifications` interface and `BaseSpecifications` use `System.Linq.Expressions`.
- **Issue**: While common in .NET, it tightly couples the Domain's contract to a specific way of querying that is heavily geared towards LINQ-to-Entities.
- **Recommendation**: This is acceptable for the current stage, but be aware that it makes the Domain layer less "pure".

### 3. Missing Soft Delete & Audit Consistency
**Importance: Medium | Difficulty: Medium**
`BaseEntity` has `CreatedOn`, `LastModifideOn` (note typo), etc., but there is no automated way in the `GenericRepository` or `DbContext` to ensure these are always populated (e.g., via Interceptors or overriding `SaveChangesAsync`).
- **Issue**: Manual population of audit fields is error-prone.
- **Recommendation**: Implement or verify the `CustomSaveChangesInterceptor` (found in the file tree) to automatically set these fields for all entities.

---

## ⚠️ Code Quality & Design Issues

### 4. Typos in Domain and Persistence
**Importance: Medium | Difficulty: Easy**
- `LastModifideOn` in `BaseEntity`.
- `GenericRepositiries` (class name typo).
- `IGenericRepositieries` (interface name typo).
- `UpdateDateBase` in `IStoreIdentityContextIntializer` (should be `UpdateDatabase`).
- `vendor` property in `Product` is lowercase, while `Category` is Uppercase (inconsistent C# conventions).

### 5. Lowercase Properties in Entities
**Importance: Low | Difficulty: Easy**
In `Product.cs`, the `vendor` navigation property starts with a lowercase 'v'.
- **Issue**: Violates standard C# PascalCase naming conventions for properties.
- **Recommendation**: Rename to `Vendor`.

### 6. Lazy Loading Enabled by Default
**Importance: Medium | Difficulty: Easy**
`UseLazyLoadingProxies()` is enabled in `DependencyInjection.cs`.
- **Issue**: Lazy loading can lead to the "N+1" query problem, especially in a web API where a serializer might trigger unexpected database calls while traversing the object graph.
- **Recommendation**: Disable lazy loading and stick to Eager Loading via the Specification pattern's `Includes`.

---

## ✅ Recommendations Summary

| Topic | Recommendation | Importance | Difficulty |
| :--- | :--- | :--- | :--- |
| **Bug** | Reorder `SpecificationsEvaluator` to apply Includes before Paging. | High | Easy |
| **Convention** | Fix navigation property naming (e.g., `vendor` -> `Vendor`). | Medium | Easy |
| **Architecture** | Disable Lazy Loading Proxies to prevent N+1 issues. | Medium | Easy |
| **Standards** | Global rename of `Repositiries` and `Repositieries` to `Repositories`. | Medium | Easy |
| **Automation** | Ensure `CustomSaveChangesInterceptor` handles audit fields automatically. | Medium | Medium |
