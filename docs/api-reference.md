# API Reference & Error Handling

## Base API Controller
All controllers inherit from `BaseApiController`, which provides a standardized route: `api/[controller]`.

## Standard Endpoints

### Products
- `GET /api/product`: Get all products (supports filtering and pagination).
- `GET /api/product/{id}`: Get product details.
- `GET /api/product/brands`: List all vendors.
- `GET /api/product/categories`: List all categories.

### Basket
- `GET /api/basket?id={id}`: Retrieve a basket.
- `POST /api/basket`: Update or create a basket.
- `DELETE /api/basket?id={id}`: Remove a basket.

### Orders
- `POST /api/orders`: Create a new order.
- `GET /api/orders`: List orders for the current user.
- `GET /api/orders/{id}`: Get order details.

## Error Handling

The API returns standardized error responses:

### 1. ApiResponse
Generic error format with a status code and message.
```json
{
  "statusCode": 404,
  "message": "Resource not found"
}
```

### 2. ApiValidationsErrorResponse
Used for 400 Bad Request when model validation fails.
```json
{
  "errors": [
    { "field": "Email", "errors": ["The Email field is required."] }
  ]
}
```

### 3. ApiExceptionResponse
Used for 500 Internal Server Errors (includes stack trace in Development environment).
