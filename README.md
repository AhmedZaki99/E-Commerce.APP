# E-Commerce Monolith System

A robust, enterprise-grade E-commerce backend built with .NET 8, following Clean Architecture principles. This project provides a scalable foundation for managing products, customer baskets, ordering workflows, and secure user authentication.

## 🏗 Architecture

The system is built as a monolith using an N-Tier / Clean Architecture approach to ensure maintainability and testability:

- **API Layer**: ASP.NET Core Controllers providing RESTful endpoints.
- **Application Layer**: Business logic, DTO mapping, and service orchestration.
- **Domain Layer**: Core business entities and the Specification pattern for flexible querying.
- **Infrastructure Layer**: Data persistence using Entity Framework Core and Identity management.

## ✨ Key Features

- **Product Management**: Search and filter products using the Specification pattern (Brand, Category, Pagination).
- **Basket System**: High-performance basket management.
- **Ordering System**: End-to-end order processing with delivery method selection.
- **Identity & Security**: Secure authentication using ASP.NET Core Identity and JWT Bearer tokens.
- **Automated Seeding**: Built-in data seeding for Categories, Vendors, and Products to get started quickly.

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or Docker instance)

### Installation
1. Clone the repository.
2. Update the connection strings in `E-Commerce.APIs/appsettings.json`.
3. Build the solution:
   ```bash
   dotnet build
   ```
4. Run the API:
   ```bash
   dotnet run --project E-Commerce.APIs/E-Commerce.APIs.csproj
   ```

## 📚 Documentation
Detailed documentation for different system components can be found in the [`docs/`](./docs/) directory:
- [Architecture & Design](./docs/architecture.md)
- [Persistence & Data](./docs/persistence.md)
- [Identity & Auth](./docs/identity.md)
- [Application Logic](./docs/application-logic.md)

## 🛠 Tech Stack
- **Language**: C# 12
- **Framework**: .NET 8 / ASP.NET Core
- **ORM**: Entity Framework Core
- **Mapping**: AutoMapper
- **Patterns**: Unit of Work, Repository, Specification Pattern.

---
*Maintained by the Senior Development Team.*
