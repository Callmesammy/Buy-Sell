# ✅ Phase 2: Application Layer — Validators

## Current Status

### ✅ Complete
- Domain Layer (10 entities, 2 enums, 3 exceptions)
- Infrastructure Entity Configurations (Fluent API for all 10 entities)
- Build successful
- Database schema ready (pending migration)

---

## Next Phase: Application Validators

**Location:** `Application/Validators/`

**What to create:**
```
Application/Validators/
├── Auth/
│   ├── RegisterBuyerValidator.cs
│   ├── RegisterSellerValidator.cs
│   └── LoginValidator.cs
├── Product/
│   └── CreateProductValidator.cs
└── (Other validators as needed)
```

---

## Validators Required

### Auth Validators

1. **RegisterBuyerValidator**
   - Email: Required, valid email format, unique (not in DB)
   - Password: Required, min 8 chars, strong password
   - FirstName: Required, max 100 chars
   - LastName: Required, max 100 chars

2. **RegisterSellerValidator**
   - Email: Required, valid email format, unique (not in DB)
   - Password: Required, min 8 chars, strong password
   - FirstName: Required, max 100 chars
   - LastName: Required, max 100 chars
   - StoreName: Required, max 256 chars
   - StoreDescription: Optional, max 1000 chars

3. **LoginValidator**
   - Email: Required, valid email format
   - Password: Required, min 8 chars

### Product Validators

1. **CreateProductValidator**
   - Title: Required, max 256 chars
   - Description: Required, max 4000 chars
   - Price: Required, greater than 0, decimal max 2 places
   - Stock: Required, minimum 0
   - CategoryId: Required, must exist in DB

---

## Pattern (FluentValidation)

```csharp
public class RegisterBuyerValidator : AbstractValidator<RegisterBuyerRequest>
{
    private readonly IUserRepository _userRepository;

    public RegisterBuyerValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be valid")
            .MustAsync(async (email, ct) => 
            {
                var user = await _userRepository.GetByEmailAsync(email, ct);
                return user == null;
            }).WithMessage("Email already exists");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters");
    }
}
```

---

## Implementation Order

1. Create Auth validators first (required for auth service)
2. Create Product validators (required for product service)
3. Register validators in Program.cs later

---

## Ready to code validators?

Let me know and we'll create them all! 🚀
