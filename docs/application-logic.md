# Application Logic & Services

The Application layer contains the "brain" of the system, implementing the business use cases.

## Service Manager
To maintain clean controllers, we use the `IServiceManager`. Instead of injecting multiple services into a controller, we inject the manager which provides access to:
- `ProductService`
- `BasketService`
- `OrderService`
- `AuthService`

## Mapping (AutoMapper)
We use AutoMapper to strictly separate Domain Entities from the data sent to the client (DTOs).

### Complex Mapping
Some mappings require custom logic, implemented via **Value Resolvers**:
- **ProductPictureUrlResolver**: Prepends the server base URL to product image paths.
- **VendorPictureUrlResolver**: Handles image paths for vendors/brands.

## Exception Handling
The application uses custom exception classes to signal business logic failures:
- `NotFoundException`: When an entity doesn't exist.
- `BadRequestException`: For invalid business requests.
- `ValidationException`: For data validation failures.

These are automatically caught by the `ExceptionHandlerMiddleware` and converted into standardized `ApiResponse` formats.
