# System Architecture

The E-Commerce system follows **Clean Architecture** (also known as Onion Architecture) principles. The primary goal is to keep the core business logic (Domain) independent of external concerns like databases or UI frameworks.

## Layers

### 1. Domain Layer (`E-Commerce.App.Domain`)
The innermost layer, containing:
- **Entities**: Business objects (e.g., `Product`, `Order`).
- **Interfaces**: Contracts for repositories and infrastructure services.
- **Specifications**: Reusable query logic implementation.

### 2. Application Layer (`E-Commerce.App.Application`)
Handles business rules and orchestrates the flow of data:
- **Services**: Implementations like `ProductService`, `OrderService`.
- **DTOs**: Data Transfer Objects for communication with the API.
- **Mapping**: AutoMapper profiles to convert between Entities and DTOs.
- **Exceptions**: Domain-specific exceptions handled by middleware.

### 3. Infrastructure Layer (`E_Commerce.App.Infrastructre.presistent`)
Deals with external concerns:
- **Data Persistence**: EF Core DbContexts (`StoreDbContext`, `StorIdentityDbContext`).
- **Repositories**: Generic and specific repository implementations.
- **Migrations**: Database schema version control.

### 4. API Layer (`E-Commerce.APIs` & `E-Commerce-Api.Controller`)
The entry point of the application:
- **Controllers**: Handle HTTP requests and return responses.
- **Middleware**: Global exception handling, authentication, and logging.
- **Configuration**: Dependency injection setup and app settings.

## Design Patterns

### Repository & Unit of Work
We use a **Generic Repository** to encapsulate CRUD operations, paired with a **Unit of Work** to ensure atomic transactions across multiple repositories.

### Specification Pattern
Instead of writing complex LINQ queries in the services, we use the **Specification Pattern**. This allows us to encapsulate query logic (Criteria, Includes, Ordering, Pagination) into small, testable classes.

### Service Manager
The `IServiceManager` pattern is used to centralize service access, reducing constructor bloat in controllers.
