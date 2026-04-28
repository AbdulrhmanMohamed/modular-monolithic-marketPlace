# API Endpoints Reference

This document lists all available API endpoints with request/response examples.

---

## Authentication Endpoints

| Method | Endpoint | Auth Required | Description |
|--------|----------|-------------|-------------|
| POST | `/api/auth/register` | No | Register new user |
| POST | `/api/auth/login` | No | Login, get JWT token |
| POST | `/api/auth/refresh` | No | Refresh JWT token |

---

## Product Endpoints

| Method | Endpoint | Auth Required | Description |
|--------|----------|-------------|-------------|
| GET | `/api/product` | Yes | Get all products (paginated) |
| GET | `/api/product/{id}` | Yes | Get product by ID |
| POST | `/api/product` | Yes (Admin) | Create new product |
| PUT | `/api/product/{id}` | Yes (Admin) | Update product |
| DELETE | `/api/product/{id}` | Yes (Admin) | Delete product |
| GET | `/api/product/search?name=X` | Yes | Search by name |

---

## Cart Endpoints

| Method | Endpoint | Auth Required | Description |
|--------|----------|-------------|-------------|
| GET | `/api/cart?userId=X` | Yes | Get user's cart |
| POST | `/api/cart/items` | Yes | Add item to cart |
| PUT | `/api/cart/items/{itemId}` | Yes | Update cart item quantity |
| DELETE | `/api/cart/items/{itemId}` | Yes | Remove item from cart |
| DELETE | `/api/cart?userId=X` | Yes | Clear cart |

---

## Order Endpoints

| Method | Endpoint | Auth Required | Description |
|--------|----------|-------------|-------------|
| POST | `/api/order` | Yes | Place new order |
| GET | `/api/order/{id}` | Yes | Get order by ID |
| GET | `/api/order/by-number/{orderNumber}` | Yes | Get order by number |
| GET | `/api/order/user/{userId}` | Yes | Get user's orders |
| DELETE | `/api/order/{id}` | Yes | Cancel order |
| PATCH | `/api/order/{id}/status` | Yes (Admin) | Update order status |

---

## Category Endpoints

| Method | Endpoint | Auth Required | Description |
|--------|----------|-------------|-------------|
| GET | `/api/category` | Yes | Get all categories |
| GET | `/api/category/{id}` | Yes | Get category by ID |
| POST | `/api/category` | Yes (Admin) | Create category |
| PUT | `/api/category/{id}` | Yes (Admin) | Update category |
| DELETE | `/api/category/{id}` | Yes (Admin) | Delete category |
| PATCH | `/api/category/{id}/move` | Yes (Admin) | Move category to new parent |

---

## Payment Endpoints

| Method | Endpoint | Auth Required | Description |
|--------|----------|-------------|-------------|
| POST | `/api/payment/process` | Yes | Process payment |
| POST | `/api/payment/refund` | Yes (Admin) | Refund payment |
| GET | `/api/payment/order/{orderId}` | Yes | Get payment by order ID |

---

## Shipping Endpoints

| Method | Endpoint | Auth Required | Description |
|--------|----------|-------------|-------------|
| POST | `/api/shipping` | Yes | Create shipment |
| POST | `/api/shipping/ship` | Yes | Ship order |
| PUT | `/api/shipping/tracking` | Yes (Admin) | Update tracking |
| DELETE | `/api/shipping/{shipmentId}` | Yes (Admin) | Cancel shipment |
| GET | `/api/shipping/order/{orderId}` | Yes | Get shipment by order |

---

## Review Endpoints

| Method | Endpoint | Auth Required | Description |
|--------|----------|-------------|-------------|
| POST | `/api/review` | Yes | Create review |
| PUT | `/api/review/{reviewId}` | Yes | Update review |
| DELETE | `/api/review/{reviewId}` | Yes | Delete review |
| POST | `/api/review/approve` | Yes (Admin) | Approve review |
| GET | `/api/review/product/{productId}` | Yes | Get product reviews |

---

## Address Endpoints

| Method | Endpoint | Auth Required | Description |
|--------|----------|-------------|-------------|
| POST | `/api/address` | Yes | Create address |
| PUT | `/api/address/{addressId}` | Yes | Update address |
| DELETE | `/api/address/{addressId}` | Yes | Delete address |
| POST | `/api/address/default` | Yes | Set default address |
| GET | `/api/address/user/{userId}` | Yes | Get user addresses |

---

## Inventory Endpoints

| Method | Endpoint | Auth Required | Description |
|--------|----------|-------------|-------------|
| GET | `/api/inventory/product/{productId}` | Yes | Get stock |
| POST | `/api/inventory/decrement` | Yes (Admin) | Decrement stock |
| POST | `/api/inventory/increment` | Yes (Admin) | Increment stock |

---

## Wishlist Endpoints

| Method | Endpoint | Auth Required | Description |
|--------|----------|-------------|-------------|
| POST | `/api/wishlist` | Yes | Add to wishlist |
| DELETE | `/api/wishlist/{wishlistId}` | Yes | Remove from wishlist |
| GET | `/api/wishlist/user/{userId}` | Yes | Get user wishlist |

---

## Discount Endpoints

| Method | Endpoint | Auth Required | Description |
|--------|----------|-------------|-------------|
| POST | `/api/discount` | Yes (Admin) | Create discount |
| PUT | `/api/discount/{discountId}` | Yes (Admin) | Update discount |
| DELETE | `/api/discount/{discountId}` | Yes (Admin) | Delete discount |
| GET | `/api/discount/active` | Yes | Get active discounts |
| POST | `/api/discount/validate` | Yes | Validate discount code |

---

## Search Endpoints

| Method | Endpoint | Auth Required | Description |
|--------|----------|-------------|-------------|
| GET | `/api/search?q=X&page=1&pageSize=10` | No | Search products |
| GET | `/api/search/history/{userId}` | Yes | Get search history |

---

## Notification Endpoints

| Method | Endpoint | Auth Required | Description |
|--------|----------|-------------|-------------|
| POST | `/api/notification` | Yes (Admin) | Send notification |
| GET | `/api/notification/user/{userId}` | Yes | Get user notifications |

---

## Analytics Endpoints

| Method | Endpoint | Auth Required | Description |
|--------|----------|-------------|-------------|
| POST | `/api/analytics` | Yes | Track event |
| GET | `/api/analytics/dashboard` | Yes (Admin) | Get dashboard stats |

---

## Media Endpoints

| Method | Endpoint | Auth Required | Description |
|--------|----------|-------------|-------------|
| POST | `/api/media` | Yes | Upload media |
| DELETE | `/api/media/{mediaId}` | Yes | Delete media |
| GET | `/api/media/{mediaId}` | Yes | Get media by ID |

---

## Endpoint Details

### POST /api/auth/register

Register a new user account.

**Request**:
```http
POST http://localhost:5000/api/auth/register
Content-Type: application/json

{
  "username": "john",
  "password": "password123",
  "email": "john@example.com"
}
```

**Response** (201 Created):
```json
{
  "userId": 1,
  "username": "john",
  "email": "john@example.com",
  "role": "User",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Validation Error** (400 Bad Request):
```json
{
  "errors": {
    "username": ["Username is required"],
    "email": ["Invalid email format"],
    "password": ["Password must be at least 6 characters"]
  }
}
```

**Duplicate Username** (400 Bad Request):
```json
{
  "error": "Username is already taken"
}
```

---

### POST /api/auth/login

Login with credentials and receive JWT token.

**Request**:
```http
POST http://localhost:5000/api/auth/login
Content-Type: application/json

{
  "username": "john",
  "password": "password123"
}
```

**Response** (200 OK):
```json
{
  "userId": 1,
  "username": "john",
  "email": "john@example.com",
  "role": "User",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Error Response** (401 Unauthorized):
```json
{
  "error": "Invalid username or password"
}
```

---

### POST /api/auth/refresh

Refresh JWT token using refresh token.

**Request**:
```http
POST http://localhost:5000/api/auth/refresh
Content-Type: application/json

{
  "refreshToken": "abc123def456..."
}
```

**Response** (200 OK):
```json
{
  "userId": 1,
  "username": "john",
  "email": "john@example.com",
  "role": "User",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Error Response** (401 Unauthorized):
```json
{
  "error": "Invalid refresh token"
}
```

**Error Response** (401 Unauthorized):
```json
{
  "error": "Refresh token expired"
}
```

---

### GET /api/product

Get all products with pagination.

**Request**:
```http
GET http://localhost:5000/api/product?page=1&pageSize=10
Authorization: Bearer <token>
```

**Query Parameters**:
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| page | int | 1 | Page number |
| pageSize | int | 10 | Items per page (max 100) |

**Response** (200 OK):
```json
{
  "items": [
    {
      "id": 1,
      "name": "Laptop",
      "price": 999.99,
      "description": "High-performance laptop",
      "stock": 50,
      "createdAt": "2024-01-01T00:00:00Z",
      "updatedAt": "2024-01-01T00:00:00Z"
    }
  ],
  "page": 1,
  "pageSize": 10,
  "totalCount": 1
}
```

---

### GET /api/product/{id}

Get a specific product by ID.

**Request**:
```http
GET http://localhost:5000/api/product/1
Authorization: Bearer <token>
```

**Response** (200 OK):
```json
{
  "id": 1,
  "name": "Laptop",
  "price": 999.99,
  "description": "High-performance laptop",
  "stock": 50,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

**Error Response** (404 Not Found):
```json
{
  "error": "Product not found"
}
```

---

### POST /api/product

Create a new product.

**Request**:
```http
POST http://localhost:5000/api/product
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "New Product",
  "price": 99.99,
  "description": "Product description",
  "stock": 25
}
```

**Response** (201 Created):
```json
{
  "id": 3,
  "name": "New Product",
  "price": 99.99,
  "description": "Product description",
  "stock": 25,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

**Validation**:
- `name`: Required, max 100 characters
- `price`: Required, must be positive
- `description`: Optional, max 500 characters
- `stock`: Required, must be >= 0

---

### PUT /api/product/{id}

Update an existing product.

**Request**:
```http
PUT http://localhost:5000/api/product/1
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "Updated Product",
  "price": 79.99,
  "description": "Updated description",
  "stock": 30
}
```

**Response** (200 OK):
```json
{
  "id": 1,
  "name": "Updated Product",
  "price": 79.99,
  "description": "Updated description",
  "stock": 30,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-02T00:00:00Z"
}
```

---

### DELETE /api/product/{id}

Delete a product.

**Request**:
```http
DELETE http://localhost:5000/api/product/1
Authorization: Bearer <token>
```

**Response** (204 No Content):
```
(empty response)
```

---

### GET /api/product/search

Search products by name.

**Request**:
```http
GET http://localhost:5000/api/product/search?name=laptop
Authorization: Bearer <token>
```

**Response** (200 OK):
```json
[
  {
    "id": 1,
    "name": "Laptop",
    "price": 999.99,
    "description": "High-performance laptop",
    "stock": 50
  }
]
```

---

### GET /api/cart

Get user's cart with items.

**Request**:
```http
GET http://localhost:5000/api/cart?userId=1
Authorization: Bearer <token>
```

**Response** (200 OK):
```json
{
  "cart": {
    "id": 1,
    "userId": 1
  },
  "items": [
    {
      "id": 1,
      "productId": 1,
      "productName": "Laptop",
      "quantity": 2,
      "unitPrice": 999.99
    }
  ],
  "itemCount": 2,
  "subtotal": 1999.98
}
```

---

### POST /api/cart/items

Add item to cart.

**Request**:
```http
POST http://localhost:5000/api/cart/items
Authorization: Bearer <token>
Content-Type: application/json

{
  "userId": 1,
  "productId": 1,
  "quantity": 1
}
```

**Response** (200 OK):
```json
{
  "cart": { "id": 1, "userId": 1 },
  "items": [...],
  "itemCount": 1,
  "subtotal": 999.99
}
```

---

### PUT /api/cart/items/{itemId}

Update cart item quantity.

**Request**:
```http
PUT http://localhost:5000/api/cart/items/1
Authorization: Bearer <token>
Content-Type: application/json

{
  "quantity": 3
}
```

**Response** (200 OK):
```json
{
  "cart": { "id": 1, "userId": 1 },
  "items": [...],
  "itemCount": 3,
  "subtotal": 2999.97
}
```

---

### DELETE /api/cart/items/{itemId}

Remove item from cart.

**Request**:
```http
DELETE http://localhost:5000/api/cart/items/1
Authorization: Bearer <token>
```

**Response** (204 No Content):
```
(empty response)
```

---

### DELETE /api/cart

Clear all items from cart.

**Request**:
```http
DELETE http://localhost:5000/api/cart?userId=1
Authorization: Bearer <token>
```

**Response** (204 No Content):
```
(empty response)
```

---

### POST /api/order

Place a new order.

**Request**:
```http
POST http://localhost:5000/api/order
Authorization: Bearer <token>
Content-Type: application/json

{
  "userId": 1,
  "addressId": 1
}
```

**Response** (201 Created):
```json
{
  "order": {
    "id": 1,
    "orderNumber": "ORD-20240430-001",
    "userId": 1,
    "status": "Pending",
    "totalAmount": 1999.98,
    "createdAt": "2024-04-30T10:00:00Z"
  }
}
```

---

### GET /api/order/{id}

Get order by ID.

**Request**:
```http
GET http://localhost:5000/api/order/1
Authorization: Bearer <token>
```

**Response** (200 OK):
```json
{
  "order": {
    "id": 1,
    "orderNumber": "ORD-20240430-001",
    "userId": 1,
    "status": "Pending",
    "totalAmount": 1999.98,
    "items": [...],
    "createdAt": "2024-04-30T10:00:00Z"
  }
}
```

---

### GET /api/order/by-number/{orderNumber}

Get order by order number.

**Request**:
```http
GET http://localhost:5000/api/order/by-number/ORD-20240430-001
Authorization: Bearer <token>
```

**Response** (200 OK):
```json
{
  "order": {
    "id": 1,
    "orderNumber": "ORD-20240430-001",
    "userId": 1,
    "status": "Pending",
    "totalAmount": 1999.98
  }
}
```

---

### GET /api/order/user/{userId}

Get all orders for a user.

**Request**:
```http
GET http://localhost:5000/api/order/user/1
Authorization: Bearer <token>
```

**Response** (200 OK):
```json
[
  {
    "id": 1,
    "orderNumber": "ORD-20240430-001",
    "userId": 1,
    "status": "Pending",
    "totalAmount": 1999.98,
    "createdAt": "2024-04-30T10:00:00Z"
  }
]
```

**Error Response** (404 Not Found):
```json
{
  "message": "No orders found for this user"
}
```

---

### DELETE /api/order/{id}

Cancel an order.

**Request**:
```http
DELETE http://localhost:5000/api/order/1
Authorization: Bearer <token>
```

**Response** (200 OK):
```json
{
  "order": {
    "id": 1,
    "status": "Cancelled",
    "totalAmount": 1999.98
  }
}
```

---

### PATCH /api/order/{id}/status

Update order status (Admin only).

**Request**:
```http
PATCH http://localhost:5000/api/order/1/status
Authorization: Bearer <token>
Content-Type: application/json

{
  "status": "Confirmed"
}
```

**Response** (200 OK):
```json
{
  "order": {
    "id": 1,
    "status": "Confirmed"
  }
}
```

---

### POST /api/category

Create a new category.

**Request**:
```http
POST http://localhost:5000/api/category
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "Electronics",
  "description": "Electronic devices",
  "imageUrl": "https://example.com/electronics.jpg",
  "parentId": null
}
```

**Response** (201 Created):
```json
{
  "id": 1,
  "name": "Electronics",
  "slug": "electronics",
  "description": "Electronic devices",
  "imageUrl": "https://example.com/electronics.jpg",
  "isActive": true
}
```

---

### GET /api/category

Get all categories.

**Request**:
```http
GET http://localhost:5000/api/category
Authorization: Bearer <token>
```

**Response** (200 OK):
```json
[
  {
    "id": 1,
    "name": "Electronics",
    "slug": "electronics",
    "description": "Electronic devices",
    "isActive": true
  }
]
```

---

### POST /api/payment/process

Process payment for an order.

**Request**:
```http
POST http://localhost:5000/api/payment/process
Authorization: Bearer <token>
Content-Type: application/json

{
  "orderId": 1,
  "method": "CreditCard",
  "amount": 1999.98
}
```

**Response** (200 OK):
```json
{
  "id": 1,
  "orderId": 1,
  "status": "Completed",
  "amount": 1999.98,
  "method": "CreditCard",
  "transactionId": "txn_abc123",
  "processedAt": "2024-04-30T10:05:00Z"
}
```

---

### POST /api/payment/refund

Refund a payment.

**Request**:
```http
POST http://localhost:5000/api/payment/refund
Authorization: Bearer <token>
Content-Type: application/json

{
  "paymentId": 1
}
```

**Response** (200 OK):
```json
{
  "id": 1,
  "status": "Refunded",
  "amount": 1999.98
}
```

---

### POST /api/shipping

Create a new shipment.

**Request**:
```http
POST http://localhost:5000/api/shipping
Authorization: Bearer <token>
Content-Type: application/json

{
  "orderId": 1
}
```

**Response** (201 Created):
```json
{
  "id": 1,
  "orderId": 1,
  "status": "Pending",
  "carrier": null,
  "trackingNumber": null
}
```

---

### POST /api/shipping/ship

Ship an order.

**Request**:
```http
POST http://localhost:5000/api/shipping/ship
Authorization: Bearer <token>
Content-Type: application/json

{
  "orderId": 1,
  "carrier": "UPS",
  "trackingNumber": "1Z999AA1234567890"
}
```

**Response** (200 OK):
```json
{
  "id": 1,
  "orderId": 1,
  "status": "Shipped",
  "carrier": "UPS",
  "trackingNumber": "1Z999AA1234567890",
  "shippedAt": "2024-04-30T12:00:00Z"
}
```

---

### POST /api/review

Create a product review.

**Request**:
```http
POST http://localhost:5000/api/review
Authorization: Bearer <token>
Content-Type: application/json

{
  "productId": 1,
  "rating": 5,
  "title": "Great product!",
  "comment": "Highly recommended"
}
```

**Response** (201 Created):
```json
{
  "id": 1,
  "productId": 1,
  "userId": 1,
  "rating": 5,
  "title": "Great product!",
  "comment": "Highly recommended",
  "isApproved": false
}
```

---

### POST /api/address

Create a new address.

**Request**:
```http
POST http://localhost:5000/api/address
Authorization: Bearer <token>
Content-Type: application/json

{
  "userId": 1,
  "label": "Home",
  "street": "123 Main St",
  "city": "New York",
  "state": "NY",
  "postalCode": "10001",
  "country": "USA"
}
```

**Response** (201 Created):
```json
{
  "id": 1,
  "userId": 1,
  "label": "Home",
  "street": "123 Main St",
  "city": "New York",
  "state": "NY",
  "postalCode": "10001",
  "country": "USA",
  "isDefault": false
}
```

---

### POST /api/wishlist

Add product to wishlist.

**Request**:
```http
POST http://localhost:5000/api/wishlist
Authorization: Bearer <token>
Content-Type: application/json

{
  "userId": 1,
  "productId": 1
}
```

**Response** (201 Created):
```json
{
  "id": 1,
  "userId": 1,
  "productId": 1,
  "productName": "Laptop",
  "addedAt": "2024-04-30T10:00:00Z"
}
```

---

### POST /api/discount

Create a new discount.

**Request**:
```http
POST http://localhost:5000/api/discount
Authorization: Bearer <token>
Content-Type: application/json

{
  "code": "SAVE20",
  "description": "20% off",
  "discountPercent": 20,
  "minOrderAmount": 100,
  "validFrom": "2024-01-01T00:00:00Z",
  "validUntil": "2024-12-31T23:59:59Z"
}
```

**Response** (201 Created):
```json
{
  "id": 1,
  "code": "SAVE20",
  "description": "20% off",
  "discountPercent": 20,
  "isActive": true
}
```

---

### GET /api/search

Search products.

**Request**:
```http
GET http://localhost:5000/api/search?q=laptop&page=1&pageSize=10
```

**Response** (200 OK):
```json
[
  {
    "id": 1,
    "name": "Laptop",
    "price": 999.99,
    "description": "High-performance laptop"
  }
]
```

---

### POST /api/analytics

Track an analytics event.

**Request**:
```http
POST http://localhost:5000/api/analytics
Authorization: Bearer <token>
Content-Type: application/json

{
  "eventType": "ProductViewed",
  "userId": 1,
  "productId": "1",
  "metadata": "{\"source\": \"search\"}"
}
```

**Response** (200 OK):
```
(empty response)
```

---

### POST /api/media

Upload media file.

**Request**:
```http
POST http://localhost:5000/api/media
Authorization: Bearer <token>
Content-Type: application/json

{
  "fileName": "product.jpg",
  "contentType": "image/jpeg",
  "size": 102400,
  "productId": 1
}
```

**Response** (201 Created):
```json
{
  "id": 1,
  "fileName": "product.jpg",
  "contentType": "image/jpeg",
  "size": 102400,
  "url": "https://example.com/media/product.jpg"
}
```

---

## Error Responses

### 400 Bad Request

```json
{
  "errors": {
    "field": ["error message"]
  }
}
```

### 401 Unauthorized

```json
{
  "error": "Invalid username or password"
}
```

or

```json
{
  "error": "Authorization has been denied for this request"
}
```

### 403 Forbidden

```json
{
  "error": "You do not have permission to access this resource"
}
```

### 404 Not Found

```json
{
  "error": "Resource not found"
}
```

### 500 Internal Server Error

```json
{
  "statusCode": 500,
  "message": "An error occurred while processing your request."
}
```

---

## HTTP Status Codes Summary

| Code | Meaning |
|------|--------|
| 200 | OK - Success |
| 201 | Created - Resource created |
| 204 | No Content - Success, no response body |
| 400 | Bad Request - Validation error |
| 401 | Unauthorized - Invalid credentials |
| 403 | Forbidden - No permission |
| 404 | Not Found - Resource doesn't exist |
| 500 | Internal Server Error |

---

## Testing with http Files

Use the provided http files:

```bash
# auth.http - Auth endpoints
@baseUrl = http://localhost:5000

### Register
POST {{baseUrl}}/api/auth/register
Content-Type: application/json
...
```

```bash
# product.http - Product endpoints
@baseUrl = http://localhost:5000

### Get all
GET {{baseUrl}}/api/product
Authorization: Bearer YOUR_TOKEN_HERE
...
```

---

## Next Steps

- See [REQUEST_FLOW.md](./REQUEST_FLOW.md) for complete request flow
- See [QUICK_REFERENCE.md](./QUICK_REFERENCE.md) for commands
