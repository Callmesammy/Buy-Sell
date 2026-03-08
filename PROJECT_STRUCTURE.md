# Project Structure — Buy & Sell Backend

## Directory Layout

```
Buy&Sell/
├── Domain/                          # Domain Layer (Entities, Enums, Exceptions)
│   ├── Common/
│   │   └── BaseEntity.cs           # Base class for all entities
│   ├── Enums/
│   │   ├── UserRole.cs             # Buyer, Seller, Admin
│   │   └── OrderStatus.cs          # Pending, Paid, Processing, Shipped, Delivered, Cancelled
│   ├── Entities/
│   │   ├── User.cs
│   │   ├── Store.cs
│   │   ├── Product.cs
│   │   ├── Category.cs
│   │   ├── Cart.cs
│   │   ├── CartItem.cs
│   │   ├── Order.cs
│   │   ├── OrderItem.cs
│   │   ├── Review.cs
│   │   └── ProductView.cs
│   └── Exceptions/
│       ├── NotFoundException.cs
│       ├── UnauthorizedException.cs
│       └── ConflictException.cs
│
├── Application/                     # Application Layer (DTOs, Interfaces, Validators, Services)
│   ├── Common/
│   │   ├── ApiResponse.cs          # Standard API response wrapper
│   │   └── PagedResult.cs          # Pagination wrapper
│   ├── DTOs/
│   │   ├── Auth/
│   │   │   ├── RegisterBuyerRequest.cs
│   │   │   ├── RegisterSellerRequest.cs
│   │   │   ├── LoginRequest.cs
│   │   │   └── AuthResponse.cs
│   │   ├── Product/
│   │   │   ├── CreateProductRequest.cs
│   │   │   └── ProductResponse.cs
│   │   ├── Store/
│   │   │   └── StoreResponse.cs
│   │   ├── Cart/ (to be created)
│   │   ├── Order/ (to be created)
│   │   └── Review/ (to be created)
│   ├── Interfaces/
│   │   ├── IUserRepository.cs
│   │   ├── IProductRepository.cs
│   │   ├── IStoreRepository.cs
│   │   ├── ICategoryRepository.cs
│   │   ├── ICartRepository.cs
│   │   ├── IOrderRepository.cs
│   │   ├── IReviewRepository.cs
│   │   └── IProductViewRepository.cs
│   ├── Validators/ (to be created)
│   └── Services/ (to be created)
│       ├── IAuthService.cs
│       ├── IProductService.cs
│       ├── ICartService.cs
│       ├── IOrderService.cs
│       ├── IAIService.cs
│       └── IPaymentService.cs
│
├── Infrastructure/                  # Infrastructure Layer (Repositories, DbContext, External Services)
│   ├── Persistence/
│   │   ├── ApplicationDbContext.cs  # EF Core DbContext
│   │   └── EntityConfigurations/
│   │       ├── UserConfiguration.cs
│   │       ├── StoreConfiguration.cs
│   │       ├── ProductConfiguration.cs
│   │       ├── CategoryConfiguration.cs
│   │       └── ... (other entity configs)
│   ├── Repositories/
│   │   ├── BaseRepository.cs
│   │   ├── UserRepository.cs
│   │   ├── ProductRepository.cs
│   │   ├── StoreRepository.cs
│   │   ├── CategoryRepository.cs
│   │   ├── CartRepository.cs
│   │   ├── OrderRepository.cs
│   │   ├── ReviewRepository.cs
│   │   └── ProductViewRepository.cs
│   └── ExternalServices/
│       ├── AzureBlobStorageService.cs
│       ├── StripePaymentService.cs
│       ├── ClaudeAIService.cs
│       └── JwtTokenService.cs
│
├── Shardkarnel/                     # API Layer (ASP.NET Core API)
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── ProductsController.cs
│   │   ├── CategoriesController.cs
│   │   ├── CartController.cs
│   │   ├── OrdersController.cs
│   │   ├── ReviewsController.cs
│   │   ├── AIController.cs
│   │   ├── PaymentsController.cs
│   │   └── HealthController.cs
│   ├── Middleware/
│   │   ├── GlobalExceptionMiddleware.cs
│   │   └── RequestLoggingMiddleware.cs
│   ├── Extensions/
│   │   ├── ServiceCollectionExtensions.cs
│   │   └── ApplicationBuilderExtensions.cs
│   ├── Program.cs
│   └── appsettings.json
│
├── TestBuy&Sell/                    # Test Project (xUnit)
│   ├── Common/
│   │   ├── BaseIntegrationTest.cs
│   │   └── TestDataFactory.cs
│   ├── Domain/
│   │   └── BaseEntityTests.cs
│   ├── Application/
│   │   └── DTOTests.cs
│   ├── Infrastructure/
│   │   └── RepositoryTests.cs
│   └── (API tests to be added)
│
├── Dockerfile
├── docker-compose.yml
├── .dockerignore
├── .env.example
├── PLAN.md                         # Project roadmap
├── FEATURES.md                     # Feature specifications
├── CLUDE.md                        # Architecture & coding standards
└── DOCKER_SETUP.md                 # Docker guide
```

---

## Layer Descriptions

### **Domain Layer** (No Dependencies)
- **Pure business logic**
- Contains entities, enums, exceptions, and value objects
- No references to databases, APIs, or external services
- All entities inherit from `BaseEntity` with `Id`, `CreatedAt`, `UpdatedAt`, `IsDeleted`

### **Application Layer** (Depends on Domain)
- DTOs for request/response
- Service interfaces and contracts
- Validators using FluentValidation
- Business logic orchestration
- No direct database access (uses repositories)

### **Infrastructure Layer** (Depends on Application)
- EF Core DbContext and entity configurations
- Repository implementations
- External service integrations (Azure Blob, Stripe, Claude API)
- Database migrations

### **API Layer** (Depends on Application)
- ASP.NET Core controllers
- HTTP request/response handling
- Middleware (error handling, logging, authentication)
- Program.cs configuration
- Dependency injection setup

---

## File Naming Conventions

| Element | Pattern | Example |
|---------|---------|---------|
| Interfaces | `I{Name}` | `IUserRepository`, `IAuthService` |
| Entities | `{Name}` | `User`, `Product` |
| DTOs (Requests) | `{Name}Request` | `CreateProductRequest` |
| DTOs (Responses) | `{Name}Response` | `ProductResponse` |
| Validators | `{Name}Validator` | `CreateProductValidator` |
| Repositories | `{Name}Repository` | `UserRepository` |
| Services | `{Name}Service` | `AuthService` |
| Controllers | `{Name}Controller` | `ProductsController` |

---

## Key Files to Review

1. **CLUDE.md** — Architecture rules and coding standards (MUST READ)
2. **PLAN.md** — Week-by-week implementation roadmap
3. **FEATURES.md** — Complete feature specifications
4. **DOCKER_SETUP.md** — Local development with Docker

---

## Current Status

✅ **Completed:**
- Domain Layer (all entities, enums, exceptions)
- Application Layer (DTOs, repository interfaces, common classes)
- Infrastructure Layer (DbContext, base repository, individual repositories)
- Docker configuration
- Test project structure

⏳ **Next Steps:**
- Create EF Core entity configurations (Fluent API)
- Implement service interfaces and validators
- Build API controllers and middleware
- Add authentication logic
- Implement external service integrations
