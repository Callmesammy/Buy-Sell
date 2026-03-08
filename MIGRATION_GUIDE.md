# Migration & Database Setup

## ✅ Infrastructure Layer Complete

All 10 Entity Configurations created successfully!

---

## 🔧 Next Steps (Manual in Package Manager Console or Terminal)

### Step 1: Create Initial Migration

Open **Package Manager Console** in Visual Studio and run:

```powershell
Add-Migration InitialCreate -Project Infrastructure -StartupProject Buy&Sell
```

Or in terminal:
```bash
cd path/to/Buy&Sell
dotnet ef migrations add InitialCreate --project ../Infrastructure --startup-project Buy&Sell
```

**Expected Output:**
- Creates `Infrastructure/Migrations/` folder
- Creates `[timestamp]_InitialCreate.cs` file
- Creates `ApplicationDbContextModelSnapshot.cs`

### Step 2: Update Database

```powershell
Update-Database -Project Infrastructure -StartupProject Buy&Sell
```

Or in terminal:
```bash
dotnet ef database update --project ../Infrastructure --startup-project Buy&Sell
```

**Expected Output:**
- Creates all 10 tables in SQL Server
- Runs migration successfully

---

## What Gets Created

### Tables
- ✅ Users
- ✅ Stores
- ✅ Categories
- ✅ Products
- ✅ Carts
- ✅ CartItems
- ✅ Orders
- ✅ OrderItems
- ✅ Reviews
- ✅ ProductViews

### Features
- ✅ Primary keys on all tables
- ✅ Foreign key relationships
- ✅ Unique indexes (Email, Store per seller, Category slug, etc.)
- ✅ Performance indexes (FK columns, frequently queried fields)
- ✅ Column constraints (max lengths, decimals, defaults)
- ✅ Soft delete support (IsDeleted column)
- ✅ Timestamps (CreatedAt, UpdatedAt)

---

## Connection String

The migration will connect using:

```
Server=localhost,1433;Initial Catalog=BuySellDb;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;
```

**Requires:**
- SQL Server running (or Docker container: `docker-compose up -d`)

---

## Verification

After migration, verify in SQL Server:

```sql
SELECT * FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'dbo' 
ORDER BY TABLE_NAME;
```

Should show 10 tables.

---

## Next Phase After Migration

Once database is created:

1. ✅ **Application Layer** — Create Validators
2. ✅ **Infrastructure Layer** — Create Services
3. ✅ **API Layer** — Create Controllers
4. ✅ **Program.cs** — Wire everything together

---

## Status

- ✅ Domain Layer — Complete
- ✅ Entity Configurations — Complete
- ⏳ Database Migration — Pending (manual action)
- ⏳ Validators — To create
- ⏳ Services — To create
- ⏳ Controllers — To create

