# Future Roadmap & Upcoming Components

As the project evolves from its current prototype stage, the following architectural and feature enhancements are planned:

## 1. CQRS Implementation
While currently utilizing a Service/Repository pattern, there is an intention to move towards **CQRS (Command Query Responsibility Segregation)** using **MediatR**.
- **Commands**: Separate logic for state-changing operations (Create Order, Update Basket).
- **Queries**: Optimized read logic using Dapper or specialized Specification projections.

## 2. Advanced Caching
- Integration of **Redis** for distributed caching of the Basket and frequently accessed Product data.

## 3. Microservices Transition
The current monolith is designed with clean boundaries to facilitate a future transition to microservices if needed (e.g., splitting Identity, Ordering, and Catalog into separate services).

## 4. Enhanced Monitoring
- Implementation of structured logging with Serilog.
- Integration with OpenTelemetry for distributed tracing.

## 5. Frontend Integration
- Development of a Client SPA (Single Page Application) or Mobile app to consume these APIs.
