# 🛍️ Buy & Sell - E-Commerce Backend

> A modern, scalable e-commerce platform built with **Clean Architecture** and **.NET 10**

---

## ⚡ Quick Start

### 1️⃣ Clone & Setup
```bash
git clone https://github.com/Callmesammy/Buy-Sell.git
cd Buy-Sell
```

### 2️⃣ Run Locally
```bash
# Using Docker (recommended)
docker-compose up

# Or locally with SQL Server
dotnet run
```

### 3️⃣ View API Docs
```
http://localhost:8080/swagger
```

---

## 🎯 What This Is

**Buy & Sell** is a full-featured e-commerce backend that lets:
- 👥 **Buyers** browse products, manage carts, and place orders
- 🏪 **Sellers** create stores, list products, and manage inventory
- 💬 **Everyone** write reviews and ratings

Built with industry best practices and production-ready architecture.

---

## ✨ Features

### 🛒 Shopping
- Product catalog with search & filtering
- Shopping cart management
- Order placement & tracking
- Order history

### 📦 Inventory
- Seller store management
- Product listings with images (Azure Blob Storage)
- Stock tracking
- Category management

### 👤 User Management
- Buyer & seller registration
- JWT authentication
- Secure password hashing (BCrypt)
- Role-based access control

### ⭐ Community
- Product reviews & ratings
- Review management
- Seller ratings

### 💳 Payments
- Stripe integration ready
- Payment processing

---

## 🏗️ Architecture

Clean Architecture with **5 layers**:

```
┌─────────────────────┐
│   API Layer         │  Controllers, Middleware
├─────────────────────┤
│ Application Layer   │  DTOs, Validators, Interfaces
├─────────────────────┤
│ Infrastructure      │  Repositories, Services, Database
├─────────────────────┤
│ Domain Layer        │  Entities, Business Logic
└─────────────────────┘
```

**Benefits:**
- ✅ Highly testable
- ✅ Easy to maintain
- ✅ Independent layers
- ✅ Clear separation of concerns

---

## 🧪 Testing

### 87 Automated Tests
- **Unit Tests**: 81 tests across 6 services
- **Integration Tests**: 11 end-to-end workflows
- **Coverage**: 100% test passing rate

Run tests locally:
```bash
dotnet test
```

---

## 📚 Documentation

| Guide | Purpose |
|-------|---------|
| **[CLUDE.md](CLUDE.md)** | Architecture & coding standards |
| **[FEATURES.md](FEATURES.md)** | Feature specifications |
| **[DOCKER_SETUP.md](DOCKER_SETUP.md)** | Local development guide |
| **[PLAN.md](PLAN.md)** | Development roadmap |

---

## 🚀 CI/CD Pipeline

Automated testing and Docker builds on every push.

**Features:**
- ✅ Runs 87 tests automatically
- ✅ Generates coverage reports
- ✅ Builds Docker images
- ✅ 5-7 minute feedback loop

[View CI/CD Details →](.github/CI-CD_SETUP.md)

---

## 🛠️ Tech Stack

| Layer | Technology |
|-------|-----------|
| **Runtime** | .NET 10 |
| **Database** | SQL Server 2022 |
| **ORM** | Entity Framework Core 10 |
| **Auth** | JWT + BCrypt |
| **Testing** | xUnit, Moq, FluentAssertions |
| **API Docs** | Swagger/OpenAPI |
| **Logging** | Serilog |
| **Storage** | Azure Blob Storage |
| **Payments** | Stripe |
| **Container** | Docker & Docker Compose |

---

## 📁 Project Structure

```
Buy-Sell/
├── Domain/                  Business entities & rules
├── Application/             Interfaces, DTOs, validation
├── Infrastructure/          Database, repositories
├── Buy&Sell/               ASP.NET Core API
├── TestProjectBuy/         Automated tests (87 tests)
├── .github/                CI/CD workflows & docs
├── Dockerfile              Container configuration
├── docker-compose.yml      Multi-container setup
└── README.md              This file
```

---

## 🚀 Getting Started

### Prerequisites
- **.NET 10** SDK
- **Docker** (optional, but recommended)
- **Git**

### Setup

**Option 1: Docker (Easiest)**
```bash
docker-compose up
```

**Option 2: Local Development**
```bash
# Install dependencies
dotnet restore

# Create database
dotnet ef database update

# Run API
dotnet run
```

**Option 3: Tests Only**
```bash
dotnet test
```

---

## 📖 API Endpoints

| Endpoint | Purpose |
|----------|---------|
| `POST /api/auth/register` | Create account |
| `POST /api/auth/login` | Sign in |
| `GET /api/products` | Browse products |
| `POST /api/cart/add` | Add to cart |
| `POST /api/orders` | Place order |
| `POST /api/reviews` | Write review |

[Full API Docs](http://localhost:8080/swagger) (when running)

---

## 🤝 Contributing

1. Fork the repository
2. Create feature branch: `git checkout -b feature/xyz`
3. Make changes & test: `dotnet test`
4. Commit: `git commit -m "feat: description"`
5. Push: `git push origin feature/xyz`
6. Create Pull Request

**All PR tests must pass automatically!**

---

## 📊 Project Status

| Component | Status |
|-----------|--------|
| **Entities** | ✅ Complete |
| **Database** | ✅ Complete |
| **Services** | ✅ Complete |
| **Repositories** | ✅ Complete |
| **API Endpoints** | ✅ Complete |
| **Authentication** | ✅ Complete |
| **Tests** | ✅ 87/87 passing |
| **CI/CD** | ✅ Automated |

---

## 🐛 Troubleshooting

### Port Already in Use?
```bash
# Change port in docker-compose.yml
# From: 8080:8080
# To:   8081:8080
```

### Tests Failing?
```bash
# Verify SQL Server is running
docker ps

# Check connection string in appsettings.json
cat Buy\&Sell/appsettings.json
```

### Need Help?
Check [DOCKER_SETUP.md](DOCKER_SETUP.md) for detailed troubleshooting.

---

## 📝 License

MIT License - Feel free to use this project!

---

## 🎯 Next Steps

1. **Start the API**: `docker-compose up`
2. **Visit Swagger**: http://localhost:8080/swagger
3. **Read**: [FEATURES.md](FEATURES.md) for what you can do
4. **Explore**: Code in `Buy&Sell/Controllers/`
5. **Test**: Run `dotnet test` to see all tests pass

---

**Happy building!** 🚀

## 🚀 Quick Start

### Start Local Environment
```bash
docker-compose up -d
# API: http://localhost:8080
# Swagger: http://localhost:8080/swagger
# SQL Server: localhost:1433 (sa / YourStrong@Password123)
```

### Next Steps
1. Read `CLUDE.md` for architecture patterns
2. Read `PLAN.md` for feature roadmap
3. Create EF Entity Configurations (Fluent API)
4. Create Validators
5. Create Services
6. Create API Controllers

---

## 🎯 Architecture

```
API Controllers → Services → Repositories → DbContext → Database
  (Buy&Sell)      (Infrastructure)   (Infrastructure)
```

**Key Rules:**
- No business logic in controllers
- All I/O is async/await
- Return DTOs, never entities
- Use standard ApiResponse<T> wrapper
- Soft deletes only (IsDeleted flag)

See `CLUDE.md` for complete patterns.

---

## 💡 Key Features

✅ Soft delete support (data retention)
✅ Automatic timestamps (CreatedAt, UpdatedAt)
✅ Pagination support (PagedResult<T>)
✅ Standard response wrapper (ApiResponse<T>)
✅ Custom exception handling
✅ JWT authentication ready
✅ BCrypt password hashing ready
✅ Docker containerization ready

---

## 📋 Entities (10)

User, Store, Category, Product, Cart, CartItem, Order, OrderItem, Review, ProductView

---

## 🎉 Ready to Build

Everything is configured. Start coding Week 1 features.

**Reference:** `CLUDE.md` for patterns  
**Roadmap:** `PLAN.md` for what to build  
**Features:** `FEATURES.md` for specifications
