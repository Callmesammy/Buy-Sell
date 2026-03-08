# Backend Plan — AI E-Commerce Marketplace

## Stack
- **Framework:** ASP.NET Core (.NET 8+)
- **Database:** Azure SQL (Entity Framework Core)
- **Auth:** JWT (same pattern as TeamFlow)
- **Storage:** Azure Blob Storage (product images)
- **Payments:** Stripe
- **AI:** Claude API (Anthropic) or OpenAI
- **Caching:** In-memory (Redis optional later)
- **Deployment:** Azure App Service

---

## Architecture
Clean Architecture — same as TeamFlow:
- `Domain` — Entities, Enums, Exceptions
- `Application` — Interfaces, DTOs, Validators, Services
- `Infrastructure` — Repositories, DbContext, External Services
- `API` — Controllers, Middleware, Program.cs

---

## Week-by-Week Plan

### Week 1 — Auth, Users, Products, Categories
- [ ] Project setup (Clean Architecture scaffold)
- [ ] Auth system: Buyer & Seller registration/login (JWT)
- [ ] Organization = Store (reuse TeamFlow pattern)
- [ ] Product entity: title, description, price, stock, images
- [ ] Category entity: name, slug, parent category (nested)
- [ ] Product CRUD endpoints (seller only)
- [ ] Image upload to Azure Blob Storage
- [ ] EF Migrations & Azure SQL deployment

### Week 2 — Cart, Orders, Stripe Payments
- [ ] Cart entity: CartItem per buyer
- [ ] Add/remove/update cart endpoints
- [ ] Order entity: OrderItems, status (Pending, Paid, Shipped, Delivered)
- [ ] Stripe integration: create payment intent, confirm payment
- [ ] Stripe webhooks: handle payment success/failure
- [ ] Order history endpoints (buyer & seller views)
- [ ] Email notifications (optional — SendGrid)

### Week 3 — AI Features, Search, Polish
- [ ] AI product recommendations (collaborative filtering + Claude API)
- [ ] AI product description generator (seller provides basic info, AI enriches)
- [ ] AI customer support chatbot endpoint
- [ ] Full-text product search (Azure SQL or Elasticsearch)
- [ ] Product reviews & ratings
- [ ] Seller dashboard stats (total sales, revenue, top products)
- [ ] Global exception middleware, logging (Serilog)
- [ ] Swagger documentation

---

## Database Schema (Key Entities)
- `Users` (Id, Email, PasswordHash, Role: Buyer/Seller, CreatedAt)
- `Stores` (Id, Name, Description, SellerId, LogoUrl)
- `Products` (Id, Title, Description, Price, Stock, CategoryId, StoreId, ImageUrls)
- `Categories` (Id, Name, Slug, ParentCategoryId)
- `Carts` (Id, BuyerId)
- `CartItems` (Id, CartId, ProductId, Quantity)
- `Orders` (Id, BuyerId, Status, TotalAmount, StripePaymentIntentId)
- `OrderItems` (Id, OrderId, ProductId, Quantity, PriceAtPurchase)
- `Reviews` (Id, ProductId, BuyerId, Rating, Comment)
- `ProductViews` (Id, ProductId, UserId, ViewedAt) — for AI recommendations

---

## API Endpoints Overview
### Auth
- POST /api/auth/register-buyer
- POST /api/auth/register-seller
- POST /api/auth/login

### Products
- GET /api/products (paginated, filterable)
- GET /api/products/{id}
- POST /api/products (seller)
- PUT /api/products/{id} (seller)
- DELETE /api/products/{id} (seller)

### Cart
- GET /api/cart
- POST /api/cart/items
- PUT /api/cart/items/{id}
- DELETE /api/cart/items/{id}

### Orders
- POST /api/orders/checkout
- GET /api/orders (buyer history)
- GET /api/orders/seller (seller orders)
- PUT /api/orders/{id}/status (seller)

### AI
- GET /api/ai/recommendations/{productId}
- POST /api/ai/generate-description
- POST /api/ai/chat

### Stripe
- POST /api/payments/create-intent
- POST /api/payments/webhook

---

## Environment Variables Needed
```
ConnectionStrings__DefaultConnection=
Jwt__Secret=
Jwt__ExpiryMinutes=60
Stripe__SecretKey=
Stripe__WebhookSecret=
AzureBlob__ConnectionString=
AzureBlob__ContainerName=
AI__ApiKey=
AI__Model=claude-sonnet-4-20250514
ASPNETCORE_ENVIRONMENT=Production
```
