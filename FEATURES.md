# Backend Features — AI E-Commerce Marketplace

## Authentication & Users
- Buyer registration (email, password)
- Seller registration (email, password, store name)
- JWT login for both roles
- Role-based authorization (Buyer / Seller / Admin)
- Password hashing (BCrypt)
- Refresh token support (optional)

---

## Store Management (Sellers)
- Create and update store profile (name, description, logo)
- View store stats: total products, total orders, revenue
- Upload store logo to Azure Blob Storage

---

## Product Management (Sellers)
- Create product with title, description, price, stock quantity, category
- Upload multiple product images (Azure Blob Storage)
- Edit and delete products
- Mark products as active/inactive
- View all products in own store

---

## Category Management
- Hierarchical categories (parent → child, e.g. Electronics → Phones)
- Admin can create/edit/delete categories
- Filter products by category

---

## Product Discovery (Buyers)
- Browse all products (paginated)
- Filter by category, price range, rating
- Sort by newest, price (low/high), popularity
- Full-text search by product title and description
- View product details (images, description, reviews, seller info)
- Track product views (used for AI recommendations)

---

## Shopping Cart
- Add product to cart
- Update quantity
- Remove item from cart
- View cart with total price calculation
- Cart persists per logged-in buyer

---

## Checkout & Payments (Stripe)
- Create Stripe Payment Intent on checkout
- Confirm payment on frontend (Stripe.js)
- Stripe webhook: mark order as Paid on success
- Stripe webhook: handle failed payments
- Generate order on successful payment

---

## Order Management
- Buyer: view order history with status
- Buyer: view order details (items, total, shipping status)
- Seller: view all incoming orders
- Seller: update order status (Processing → Shipped → Delivered)
- Order status notifications (optional — email via SendGrid)

---

## Reviews & Ratings
- Buyer can leave a review after purchase
- Star rating (1–5) + comment
- Average rating calculated per product
- Seller cannot review own products

---

## AI Features
### 1. AI Product Recommendations
- Track product views per user
- On product detail page, return "You might also like" suggestions
- Use Claude API: pass product details + user's view history → get recommendations
- Fallback: return top-rated products in same category

### 2. AI Product Description Generator
- Seller provides: product name, key features (bullet points)
- Claude API generates: full marketing description, SEO-friendly title, key highlights
- Seller can accept, regenerate, or manually edit

### 3. AI Customer Support Chatbot
- Buyer asks questions in natural language
- Claude API answers based on: product info, order status, store policies
- Handles: "Where is my order?", "What is your return policy?", "Tell me about this product"

---

## Search
- Search products by keyword (title, description, tags)
- Filter results by category, price, rating, in-stock only
- Paginated results with total count

---

## Seller Dashboard (Stats API)
- Total products listed
- Total orders received
- Total revenue (all time / this month)
- Top 5 best-selling products
- Recent orders list

---

## Admin Features (Optional)
- View all users, stores, products
- Suspend/ban sellers or buyers
- Manage categories
- View platform-wide stats

---

## Non-Functional Features
- Global exception middleware (returns clean JSON errors)
- Request logging with Serilog
- Input validation with FluentValidation
- Pagination on all list endpoints
- Rate limiting on AI endpoints (prevent abuse)
- Swagger/OpenAPI documentation
- CORS configured for frontend origin
- Health check endpoint (`/health`)
