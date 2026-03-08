# Docker Setup Guide — Buy & Sell Backend

## Prerequisites
- Docker Desktop installed and running
- Docker Compose installed (included with Docker Desktop)

## Quick Start

### 1. Copy Environment Variables
```bash
cp .env.example .env
```
Update `.env` with your actual credentials (Stripe, Azure Blob, AI API keys).

### 2. Build and Run Containers
```bash
docker-compose up -d
```

This will:
- Start SQL Server container (listening on `localhost:1433`)
- Build the API image
- Start the API container (listening on `localhost:8080`)

### 3. Verify Services Are Running
```bash
docker-compose ps
```

Expected output:
```
CONTAINER ID   IMAGE              STATUS      PORTS
...            buysell-api        Up          0.0.0.0:8080->8080/tcp
...            buysell-sqlserver  Up (healthy) 0.0.0.0:1433->1433/tcp
```

### 4. Check API Logs
```bash
docker-compose logs -f api
```

---

## Database Connection Strings

### From Inside Container (Docker)
```
Server=sqlserver,1433;Initial Catalog=BuySellDb;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;
```

### From Local Machine
```
Server=localhost,1433;Initial Catalog=BuySellDb;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;
```

**SQL Server Credentials:**
- **Username:** `sa`
- **Password:** `YourStrong@Password123`
- **Port:** `1433`

---

## API Access

- **Base URL:** `http://localhost:8080`
- **Health Check:** `http://localhost:8080/health`

---

## Useful Commands

### View Logs
```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f api
docker-compose logs -f sqlserver
```

### Stop Containers
```bash
docker-compose down
```

### Remove Containers and Data
```bash
docker-compose down -v
```

### Rebuild Images
```bash
docker-compose build --no-cache
```

### Execute Command in Container
```bash
# Connect to SQL Server CLI
docker-compose exec sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P YourStrong@Password123

# Run migrations in API container
docker-compose exec api dotnet ef database update
```

---

## Troubleshooting

### SQL Server Fails to Start
- Check if port 1433 is already in use
- Increase Docker memory allocation (Settings → Resources)

### API Can't Connect to Database
- Verify `sqlserver` container is healthy: `docker-compose ps`
- Check logs: `docker-compose logs api`
- Ensure connection string uses `sqlserver` (Docker DNS) not `localhost`

### Port 8080 Already in Use
Edit `docker-compose.yml`:
```yaml
ports:
  - "8081:8080"  # Change 8081 to any available port
```

---

## Local Development (Without Docker)

### Prerequisites
- SQL Server or SQL Server Express installed locally
- Update connection string to local server

### Connection String
```
Server=.;Initial Catalog=BuySellDb;Integrated Security=true;TrustServerCertificate=True;
```

---

## Environment Variables Reference

See `.env.example` for all required variables.
