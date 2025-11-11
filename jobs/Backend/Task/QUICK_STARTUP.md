# Quick Startup Guide

Get the Exchange Rate API system up and running in minutes!

## Prerequisites

- .NET 9.0 SDK
- Visual Studio 2022 (or Visual Studio Code)
- SQL Server LocalDB (will be installed if not present)

---

## Step 1: Install SQL Server LocalDB

If you don't have SQL Server LocalDB installed, run the setup script:

```powershell
# Run PowerShell as Administrator
.\setup-localdb.ps1
```

This script will:
- Check if LocalDB is already installed
- Download and install SQL Server Express LocalDB if needed
- Create and start the `MSSQLLocalDB` instance
- Display the connection string

To verify LocalDB is running:
```powershell
SqlLocalDB.exe info MSSQLLocalDB
```

---

## Step 2: Publish the Database

The system uses **three separate databases** (one for each API):

- **ExchangeRateUpdaterTestREST** - For REST API
- **ExchangeRateUpdaterTestSOAP** - For SOAP API
- **ExchangeRateUpdaterTestGRPC** - For gRPC API

### Option A: Using Visual Studio

1. Open the solution in Visual Studio
2. Right-click on the **Database** project -> **Publish**
3. Set **Target database connection** to:
   ```
   Server=(localdb)\MSSQLLocalDB;Integrated Security=true;
   ```
4. Set **Database name** to: `ExchangeRateUpdaterTestREST`
5. Click **Publish**
6. Repeat steps 2-5 for `ExchangeRateUpdaterTestSOAP`
7. Repeat steps 2-5 for `ExchangeRateUpdaterTestGRPC`

### Option B: Using Command Line

```bash
# Build the database project first
cd Database
dotnet build

# Publish to REST database
sqlpackage /Action:Publish /SourceFile:"bin\Debug\Database.dacpac" /TargetConnectionString:"Server=(localdb)\MSSQLLocalDB;Database=ExchangeRateUpdaterTestREST;Integrated Security=true;"

# Publish to SOAP database
sqlpackage /Action:Publish /SourceFile:"bin\Debug\Database.dacpac" /TargetConnectionString:"Server=(localdb)\MSSQLLocalDB;Database=ExchangeRateUpdaterTestSOAP;Integrated Security=true;"

# Publish to gRPC database
sqlpackage /Action:Publish /SourceFile:"bin\Debug\Database.dacpac" /TargetConnectionString:"Server=(localdb)\MSSQLLocalDB;Database=ExchangeRateUpdaterTestGRPC;Integrated Security=true;"
```

**Note:** Each database will be automatically seeded with:
- Default currencies (EUR, USD, CZK, GBP, JPY, etc.)
- Exchange rate providers (ECB, CNB, BNR)
- Default admin user: `admin@example.com` / `simple`
- Default consumer user: `consumer@example.com` / `simple`

---

## Step 3: Configure Multiple Startup Projects

### In Visual Studio 2022:

1. Right-click on the **Solution** in Solution Explorer
2. Select **Configure Startup Projects...**
3. Select **Multiple startup projects**
4. Set these projects to **Start**:
   - `ApiLayer\REST` -> **Start**
   - `ApiLayer\SOAP` -> **Start**
   - `ApiLayer\gRPC` -> **Start**
   - `TestLayer\ConsoleTestApp` -> **Start**
5. Optionally, set the **start order** (REST -> SOAP -> gRPC -> ConsoleTestApp)
6. Click **OK**

### In Visual Studio Code:

Use the provided launch configuration (`.vscode/launch.json` if available) or start each project manually:

```bash
# Terminal 1 - REST API
cd ApiLayer/REST
dotnet run

# Terminal 2 - SOAP API
cd ApiLayer/SOAP
dotnet run

# Terminal 3 - gRPC API
cd ApiLayer/gRPC
dotnet run

# Terminal 4 - ConsoleTestApp
cd TestLayer/ConsoleTestApp
dotnet run
```

---

## Step 4: Start the System & Wait (~5 Minutes)

Press **F5** in Visual Studio or run all projects as shown above.

### What's happening during startup (~300 seconds)?

**Go make yourself a coffee! Here's what's going on:**

1. **APIs Starting** (0-30 seconds)
   - REST API starts on `http://localhost:5188`
   - SOAP API starts on `http://localhost:5002`
   - gRPC API starts on `http://localhost:5001`
   - ConsoleTestApp waits for APIs to be ready

2. **Database Initialization** (30-60 seconds)
   - Entity Framework applies migrations
   - Seeds currencies, providers, users, and system configuration
   - Creates indexes and views

3. **Background Jobs Starting** (60-120 seconds)
   - Hangfire initializes (used for scheduled tasks)
   - Provider health checks begin
   - Historical data fetch jobs queue up

4. **Initial Data Fetch** (120-300 seconds)
   - **Historical rates** are fetched for the last 90 days (configurable)
   - Data is fetched from:
     - **ECB** (European Central Bank) - EUR base rates
     - **CNB** (Czech National Bank) - CZK base rates
     - **BNR** (National Bank of Romania) - RON base rates
   - Approximately **~200+ exchange rates** are populated per day
   - This is a **one-time process** on first startup

### Pro Tips While You Wait:

- **Swagger UI** is available at:
  - REST: `http://localhost:5188/swagger`
  - SOAP: `http://localhost:5002` (WSDL available)
  - gRPC: Use tools like **BloomRPC** or **Postman** for testing

- **Hangfire Dashboard** (REST API only): `http://localhost:5188/hangfire`
  - Monitor background jobs and their status
  - View job history and failures
  - Manually trigger jobs

- **Check startup progress** in the console logs:
  - Look for messages like: `"Starting historical data fetch for 90 days..."`
  - Background jobs will show: `"Fetching from ECB...", "Fetching from CNB...", etc.`

- **Health Checks**:
  - `http://localhost:5188/health` - Overall health
  - `http://localhost:5188/health/ready` - Readiness check
  - `http://localhost:5188/health/live` - Liveness check

---

## Step 5: Test the System

Once the **ConsoleTestApp** starts, you'll see a welcome screen:

```
===================================================
  Exchange Rate API Test Console
===================================================
Test and compare REST, SOAP, and gRPC APIs side-by-side
Type 'help' for available commands
```

### Quick Test Commands:

```bash
# 1. Login to all APIs at once (using default admin credentials)
> login-all

# 2. Get latest exchange rates from REST API
> latest rest

# 3. Get latest exchange rates from SOAP API
> latest soap

# 4. Get latest exchange rates from gRPC API
> latest grpc

# 5. Compare all three APIs side-by-side
> compare latest
```

### More Test Commands:

```bash
# View system status
> status

# Get historical rates (last 7 days)
> historical rest
> historical soap
> historical grpc

# Convert currency
> convert rest EUR USD 100
> convert soap GBP JPY 50

# Test all endpoints for a protocol
> test-all rest

# Solo mode (test one API without specifying protocol each time)
> solo soap
> login
> latest
> historical
> exit-solo

# Get help
> help
```

---

## Default Credentials

The system comes with pre-configured users:

### Admin User
- **Email:** `admin@example.com`
- **Password:** `simple`
- **Role:** Admin
- **Permissions:** Full access to all operations

### Consumer User
- **Email:** `consumer@example.com`
- **Password:** `simple`
- **Role:** Consumer
- **Permissions:** Read-only access to exchange rates

---

## Troubleshooting

### APIs fail to start?

**Check if ports are already in use:**
```bash
netstat -ano | findstr "5188"
netstat -ano | findstr "5002"
netstat -ano | findstr "5001"
```

**Change ports in `launchSettings.json`** if needed:
- `ApiLayer/REST/Properties/launchSettings.json`
- `ApiLayer/SOAP/Properties/launchSettings.json`
- `ApiLayer/gRPC/Properties/launchSettings.json`

### LocalDB connection fails?

**Verify LocalDB is running:**
```powershell
SqlLocalDB.exe info MSSQLLocalDB
SqlLocalDB.exe start MSSQLLocalDB
```

**Connection string in `appsettings.json`:**
```json
"DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ExchangeRateUpdaterTestREST;Integrated Security=true;Connection Timeout=30;MultipleActiveResultSets=true;"
```

### Databases not created?

**Manually publish using Visual Studio:**
1. Right-click **Database** project -> **Publish**
2. Set target to `(localdb)\MSSQLLocalDB`
3. Set database name and click **Publish**

**Or use sqlpackage CLI** (see Step 2 above)

### Historical data fetch is slow?

**Disable historical fetch on startup** in `appsettings.json`:
```json
"BackgroundJobs": {
  "FetchHistoricalOnStartup": false,
  "HistoricalDataDays": 90
}
```

You can manually trigger it later via Hangfire dashboard.

### ConsoleTestApp shows "Connection refused"?

**Wait for APIs to fully start** - they need time to:
1. Initialize databases
2. Start Hangfire
3. Begin accepting connections

**Check API health endpoints:**
```bash
curl http://localhost:5188/health
curl http://localhost:5002/health
curl http://localhost:5001/health
```

---

## System Architecture Overview

```
┌─────────────────────────────────────────────────────────┐
│                   ConsoleTestApp                         │
│  (Interactive CLI for testing all three APIs)           │
└────────────┬────────────┬────────────┬──────────────────┘
             │            │            │
    ┌────────▼───┐  ┌────▼────┐  ┌───▼──────┐
    │  REST API  │  │ SOAP API │  │ gRPC API │
    │  :5188     │  │  :5002   │  │  :5001   │
    └────────┬───┘  └────┬────┘  └───┬──────┘
             │            │            │
             └────────────┴────────────┘
                         │
             ┌───────────▼───────────┐
             │  Application Layer    │
             │  (CQRS + MediatR)    │
             └───────────┬───────────┘
                         │
             ┌───────────▼───────────┐
             │   Domain Layer        │
             │   (Business Logic)    │
             └───────────┬───────────┘
                         │
             ┌───────────▼───────────┐
             │  Infrastructure Layer │
             │  (EF Core, Hangfire)  │
             └───────────┬───────────┘
                         │
      ┌──────────────────┴──────────────────┐
      │         SQL Server LocalDB           │
      │  ┌────────┬────────┬──────────┐     │
      │  │REST DB │SOAP DB │gRPC DB   │     │
      │  └────────┴────────┴──────────┘     │
      └──────────────────────────────────────┘
```

---

## Next Steps

Once the system is running:

1. **Explore the Swagger UI** - Interactive API documentation
2. **Check Hangfire Dashboard** - View background job status
3. **Test different endpoints** - Use ConsoleTestApp to compare APIs
4. **Monitor provider health** - Use `provider-health` command
5. **Review the logs** - Check console output for detailed information

---

## Need Help?

- Check the main **README.md** for detailed documentation
- Review **ApiLayer/REST/README.md**, **ApiLayer/SOAP/README.md**, **ApiLayer/gRPC/README.md** for API-specific details
- See **TestLayer/ConsoleTestApp/README.md** for testing guide
- Examine logs in the console windows for error details

---

**Happy Testing!**
