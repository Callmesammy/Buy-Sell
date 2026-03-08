# Claude.md — Backend AI E-Commerce Marketplace

You are an expert .NET backend engineer helping build a production-ready AI-powered e-commerce marketplace API. Follow these instructions precisely for every task.

---

## Project Identity
- **Project:** AI E-Commerce Marketplace (Backend)
- **Framework:** ASP.NET Core (.NET 8+)
- **Architecture:** Clean Architecture
- **Database:** Azure SQL via Entity Framework Core
- **Auth:** JWT Bearer tokens
- **AI:** Claude API (Anthropic)
- **Payments:** Stripe
- **Storage:** Azure Blob Storage
- **Logging:** Serilog

---

## Architecture Rules — NEVER Break These

### Layer Structure
```
/Domain          → Entities, Enums, Exceptions, Value Objects (no dependencies)
/Application     → Interfaces, DTOs, Validators, Service contracts (depends on Domain only)
/Infrastructure  → Repositories, DbContext, External Services (depends on Application)
/API             → Controllers, Middleware, Program.cs (depends on Application)
```

### Strict Rules
- Controllers NEVER contain business logic — only call services
- Repositories NEVER contain business logic — only database queries
- Services NEVER directly reference DbContext — only use repository interfaces
- DTOs NEVER expose domain entities directly
- All service methods must be async with CancellationToken support
- Every public service method must have XML doc comments

---

## Code Style

### Naming Conventions
- Interfaces: `IUserRepository`, `IAuthService`
- DTOs: `CreateProductRequest`, `ProductResponse`, `AuthResponse`
- Validators: `CreateProductValidator`
- Controllers: `ProductsController`, `AuthController`
- Repositories: `UserRepository`, `ProductRepository`

### Controller Pattern
```csharp
[HttpPost]
public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
{
    var validationResult = await _validator.ValidateAsync(request);
    if (!validationResult.IsValid)
        return BadRequest(ApiResponse<object>.ErrorResponse("Validation failed.", validationResult.ToErrorDictionary()));

    try
    {
        var result = await _productService.CreateProductAsync(request, GetUserId());
        return Ok(ApiResponse<ProductResponse>.SuccessResponse(result, "Product created."));
    }
    catch (NotFoundException ex) { return NotFound(ApiResponse<object>.ErrorResponse(ex.Message)); }
    catch (UnauthorizedException ex) { return Unauthorized(ApiResponse<object>.ErrorResponse(ex.Message)); }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Unexpected error creating product");
        throw;
    }
}
```

### Repository Pattern
```csharp
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResult<Product>> GetAllAsync(ProductFilter filter, CancellationToken ct = default);
    Task AddAsync(Product product, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
```

### ApiResponse Wrapper
Always return responses wrapped in ApiResponse<T>:
```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
    public object? Errors { get; set; }

    public static ApiResponse<T> SuccessResponse(T data, string message = "Success") => new() { Success = true, Message = message, Data = data };
    public static ApiResponse<T> ErrorResponse(string message, object? errors = null) => new() { Success = false, Message = message, Errors = errors };
}
```

---

## Entity Rules
- All entities inherit from `BaseEntity` with: `Id (Guid)`, `CreatedAt`, `UpdatedAt`, `IsDeleted`
- Use soft deletes — never hard delete records
- Guids are generated in the entity constructor (`Id = Guid.NewGuid()`)
- `CreatedAt` and `UpdatedAt` set automatically via `DbContext.SaveChangesAsync` override

---

## EF Core Rules
- Use Fluent API configuration in separate `IEntityTypeConfiguration<T>` classes
- Always add indexes on foreign keys and frequently queried fields
- Use `AsNoTracking()` on all read-only queries
- Never use lazy loading — always explicit `.Include()`
- Migrations go in the `Infrastructure` project

---

## JWT Auth
- JWT secret from config: `builder.Configuration["Jwt:Secret"]`
- Claims: `userId`, `email`, `storeId` (if seller), `role`
- Token expiry from config: `Jwt:ExpiryMinutes`
- Extract userId in controllers via: `User.FindFirst("userId")?.Value`

---

## Stripe Integration
- Use `Stripe.net` NuGet package
- Create PaymentIntent on checkout
- Handle webhooks in a dedicated `PaymentsController`
- Validate webhook signature using `Stripe:WebhookSecret` from config
- On `payment_intent.succeeded` → mark order as Paid

---

## Azure Blob Storage
- Use `Azure.Storage.Blobs` NuGet package
- Upload images to container defined in `AzureBlob:ContainerName`
- Return public URL after upload
- Accepted formats: jpg, jpeg, png, webp
- Max file size: 5MB per image

---

## AI Integration (Claude API)
- Use `HttpClient` to call `https://api.anthropic.com/v1/messages`
- Model: `claude-sonnet-4-20250514`
- API key from config: `AI:ApiKey`
- Always set `max_tokens: 1000`
- Wrap all AI calls in try/catch — AI failure should NOT crash the request
- For recommendations: return fallback (top-rated in category) if AI fails
- Rate limit AI endpoints: max 10 requests/minute per user

---

## Validation (FluentValidation)
- Every request DTO must have a validator
- Register validators: `builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>()`
- Example rules:
  - Title: NotEmpty, MaxLength(256)
  - Price: GreaterThan(0)
  - Email: NotEmpty, EmailAddress
  - Password: MinLength(8)

---

## Error Handling
- GlobalExceptionMiddleware catches all unhandled exceptions
- Returns: `500 { success: false, message: "An unexpected error occurred." }`
- Log full exception with Serilog before returning response
- Custom exceptions: `NotFoundException`, `UnauthorizedException`, `ConflictException`, `ValidationException`

---

## Pagination
All list endpoints must support pagination:
```csharp
public class PagedResult<T>
{
    public List<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
```
Default: `page=1`, `pageSize=20`, max pageSize=100

---

## Logging
- Use Serilog structured logging
- Log: request start/end, errors, auth events, payment events
- Never log: passwords, full JWT tokens, card numbers
- Log format: `[timestamp] [level] [context] message`

---

## Security Rules
- Hash passwords with BCrypt (never MD5/SHA1)
- Never return PasswordHash in any response DTO
- Validate Stripe webhook signatures
- Sanitize all user inputs
- Use `[Authorize]` on all non-public endpoints
- Seller endpoints: verify the product/store belongs to the authenticated seller

---

## Configuration (appsettings.json structure)
```json
{
  "ConnectionStrings": { "DefaultConnection": "" },
  "Jwt": { "Secret": "", "ExpiryMinutes": 60 },
  "Stripe": { "SecretKey": "", "WebhookSecret": "" },
  "AzureBlob": { "ConnectionString": "", "ContainerName": "products" },
  "AI": { "ApiKey": "", "Model": "claude-sonnet-4-20250514" },
  "Redis": { "ConnectionString": "localhost:6379", "Enabled": false }
}
```

---

## What NOT To Do
- Never put connection strings or secrets in code
- Never return 200 for errors — use correct HTTP status codes
- Never skip input validation
- Never expose internal exception messages to the client
- Never use `var` for return types in public methods
- Never use `.Result` or `.Wait()` — always `await`
