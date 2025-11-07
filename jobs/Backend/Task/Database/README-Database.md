# Exchange Rate Database Project

SQL Server Database Project for the Exchange Rate Updater application.

## Overview

This database project uses **SQL Server Data Tools (SSDT)** and follows a schema-first approach for database development. It provides complete data persistence for exchange rate providers, currency data, audit logs, and system configuration.

### Database Technology
- **Platform**: SQL Server 2019+ / Azure SQL Database
- **Compatibility**: SQL Server 160 (SQL Server 2022)
- **Tools**: SQL Server Data Tools (SSDT), SqlPackage CLI

### Design Principles
- **Schema-first development**: All changes are scripted and version controlled
- **Idempotent deployments**: Can be deployed multiple times safely
- **Referential integrity**: Foreign keys enforce data consistency
- **Audit trail**: Comprehensive logging of fetch operations and errors
- **Performance optimized**: Strategic indexing for common query patterns

---

## Database Structure

```
Database/
├── Table/
│   ├── Finance/
│   │   ├── Currency.sql                              # ISO 4217 currencies
│   │   └── ExchangeRate.sql                          # Exchange rate data
│   ├── Provider/
│   │   ├── ExchangeRateProvider.sql                  # Provider configuration
│   │   └── ExchangeRateProviderConfiguration.sql    # Provider settings (key-value)
│   ├── Audit/
│   │   └── ExchangeRateFetchLog.sql                 # Fetch operation audit log
│   ├── System/
│   │   ├── SystemConfiguration.sql                   # Global settings
│   │   └── ErrorLog.sql                             # Application error log
│   └── User/
│       └── User.sql                                  # User accounts
│
├── View/
│   ├── ExchangeRate/
│   │   ├── vw_LatestExchangeRates.sql               # Latest rate per currency pair
│   │   ├── vw_CurrentExchangeRates.sql              # Today's rates
│   │   ├── vw_ExchangeRateHistory.sql               # Historical rate trends
│   │   └── vw_CurrencyPairAvailability.sql          # Currency pair coverage
│   ├── Provider/
│   │   ├── vw_ProviderHealthStatus.sql              # Provider health metrics
│   │   ├── vw_ProviderConfigurationSummary.sql      # Provider config overview
│   │   └── vw_RecentFetchActivity.sql               # Recent fetch operations
│   └── System/
│       ├── vw_ErrorSummary.sql                       # Error statistics
│       └── vw_SystemHealthDashboard.sql             # System-wide health metrics
│
├── Procedure/
│   ├── ExchangeRate/
│   │   └── sp_BulkUpsertExchangeRates.sql           # Bulk import/update rates
│   └── Audit/
│       ├── sp_StartFetchLog.sql                      # Begin fetch tracking
│       └── sp_CompleteFetchLog.sql                   # Complete fetch with results
│
└── Script/
    ├── Index.sql                                     # Performance indexes
    ├── SeedCurrencies.sql                           # Initial currency data
    ├── SeedUsers.sql                                # Default user accounts
    ├── SeedExchangeRateProvidersAndConfiguration.sql # Provider setup
    └── SeedSystemConfiguration.sql                  # System defaults
```

---

## Tables

### Finance Schema

#### Currency
Stores ISO 4217 three-letter currency codes.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT IDENTITY | PK | Auto-incrementing identifier |
| Code | NVARCHAR(3) | NOT NULL, UNIQUE | ISO 4217 currency code (e.g., USD, EUR) |
| Created | DATETIMEOFFSET | NOT NULL, DEFAULT | Record creation timestamp |

**Business Rules:**
- Code must be exactly 3 characters
- Code is case-insensitive (stored uppercase)

**Sample Data:**
```sql
CZK, EUR, USD, GBP, JPY, ...
```

---

#### ExchangeRate
Stores historical exchange rate data from providers.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT IDENTITY | PK | Auto-incrementing identifier |
| ProviderId | INT | NOT NULL, FK → ExchangeRateProvider | Source provider |
| BaseCurrencyId | INT | NOT NULL, FK → Currency | Base currency |
| TargetCurrencyId | INT | NOT NULL, FK → Currency | Target currency |
| Multiplier | INT | NOT NULL, >0 | Rate denominator for precision |
| Rate | DECIMAL(19,6) | NOT NULL, >0 | Rate numerator |
| ValidDate | DATE | NOT NULL, ≤ TODAY | Date the rate is valid for |
| Created | DATETIMEOFFSET | NOT NULL, DEFAULT | Initial insert timestamp |
| Modified | DATETIMEOFFSET | NULL | Last update timestamp |

**Business Rules:**
- `Rate` and `Multiplier` must be positive
- `ValidDate` cannot be in the future
- `BaseCurrencyId` ≠ `TargetCurrencyId` (no self-referencing rates)
- **Unique constraint**: `(ProviderId, BaseCurrencyId, TargetCurrencyId, ValidDate)`
  - One rate per provider per currency pair per date

**Rate Calculation:**
```
Actual Exchange Rate = Rate / Multiplier
Example: 1 EUR = 25.50 CZK
  Stored as: Rate = 255, Multiplier = 10
  Actual = 255 / 10 = 25.5
```

**Indexes:**
- `IX_ExchangeRate_Lookup` on `(TargetCurrencyId, ValidDate DESC)` - Fast rate lookups
- `IX_ExchangeRate_Provider` on `(ProviderId, ValidDate DESC)` - Provider queries
- `IX_ExchangeRate_BaseCurrency` on `(BaseCurrencyId, ValidDate DESC)` - Base currency queries

---

### Provider Schema

#### ExchangeRateProvider
Stores exchange rate provider configurations and health status.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT IDENTITY | PK | Auto-incrementing identifier |
| Name | NVARCHAR(100) | NOT NULL | Human-readable provider name |
| Code | NVARCHAR(10) | NOT NULL, UNIQUE | Short provider code (uppercase) |
| Url | NVARCHAR(500) | NOT NULL | API endpoint URL |
| BaseCurrencyId | INT | NOT NULL, FK → Currency | Provider's base currency |
| RequiresAuthentication | BIT | NOT NULL, DEFAULT 0 | Whether API key is needed |
| ApiKeyVaultReference | NVARCHAR(255) | NULL | Azure Key Vault reference for API key |
| IsActive | BIT | NOT NULL, DEFAULT 1 | Provider enabled status |
| LastSuccessfulFetch | DATETIMEOFFSET | NULL | Timestamp of last successful fetch |
| LastFailedFetch | DATETIMEOFFSET | NULL | Timestamp of last failed fetch |
| ConsecutiveFailures | INT | NOT NULL, DEFAULT 0 | Counter for consecutive failures |
| Created | DATETIMEOFFSET | NOT NULL, DEFAULT | Record creation timestamp |
| Modified | DATETIMEOFFSET | NULL | Last update timestamp |

**Business Rules:**
- Automatically deactivated after 10 consecutive failures
- Health status derived from `ConsecutiveFailures` and `LastSuccessfulFetch`
- If `RequiresAuthentication = 1`, `ApiKeyVaultReference` should be populated

**Health Status Logic** (implemented in `vw_ProviderHealthStatus`):
- **Disabled**: `IsActive = 0`
- **Critical**: `ConsecutiveFailures >= 5`
- **Degraded**: `ConsecutiveFailures >= 3`
- **Never Fetched**: `LastSuccessfulFetch IS NULL`
- **Stale**: Last success > 24 hours ago
- **Warning**: Last success > 2 hours ago
- **Healthy**: Otherwise

**Indexes:**
- `IX_ExchangeRateProvider_Active` - Quick filtering by active status
- `IX_ExchangeRateProvider_Health` - Health monitoring queries
- `IX_ExchangeRateProvider_BaseCurrency` - Base currency filtering
- `IX_ExchangeRateProvider_Lookup` - Code-based lookups

---

#### ExchangeRateProviderConfiguration
Key-value configuration settings for providers.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT IDENTITY | PK | Auto-incrementing identifier |
| ProviderId | INT | NOT NULL, FK → ExchangeRateProvider (CASCADE DELETE) | Parent provider |
| SettingKey | NVARCHAR(100) | NOT NULL | Configuration key |
| SettingValue | NVARCHAR(MAX) | NOT NULL | Configuration value |
| Description | NVARCHAR(500) | NULL | Setting description |
| Created | DATETIMEOFFSET | NOT NULL, DEFAULT | Record creation timestamp |
| Modified | DATETIMEOFFSET | NULL | Last update timestamp |

**Business Rules:**
- **Unique constraint**: `(ProviderId, SettingKey)` - One value per key per provider
- **Cascade delete**: When provider is deleted, configs are automatically removed

**Common Settings:**
- `RequestTimeout` - HTTP request timeout (ms)
- `MaxRetries` - Maximum retry attempts
- `RateLimitPerMinute` - Rate limiting threshold
- `UserAgent` - Custom User-Agent header
- `CustomHeaders` - JSON object of additional headers

---

### Audit Schema

#### ExchangeRateFetchLog
Audit trail for all exchange rate fetch operations.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | BIGINT IDENTITY | PK | Auto-incrementing identifier |
| ProviderId | INT | NOT NULL, FK → ExchangeRateProvider | Provider being fetched |
| FetchStarted | DATETIMEOFFSET | NOT NULL, DEFAULT | Operation start time |
| FetchCompleted | DATETIMEOFFSET | NULL | Operation completion time |
| Status | NVARCHAR(20) | NOT NULL, DEFAULT 'Running' | Running/Success/Failed/PartialSuccess |
| RatesImported | INT | NULL | Number of new rates inserted |
| RatesUpdated | INT | NULL | Number of existing rates updated |
| ErrorMessage | NVARCHAR(MAX) | NULL | Error details if failed |
| RequestedBy | INT | NULL, FK → User | User who initiated fetch (NULL for scheduled) |
| DurationMs | AS `DATEDIFF(MILLISECOND, FetchStarted, FetchCompleted)` | COMPUTED, PERSISTED | Fetch duration in milliseconds |

**Business Rules:**
- Status must be one of: `Running`, `Success`, `Failed`, `PartialSuccess`
- `DurationMs` automatically calculated when `FetchCompleted` is set
- Log entry created with `Status = 'Running'` at fetch start
- Updated with final status and metrics upon completion

**Workflow:**
```sql
-- 1. Start fetch
EXEC sp_StartFetchLog @ProviderId = 1, @RequestedBy = NULL
  -- Returns @LogId

-- 2. Fetch rates from API...

-- 3. Complete fetch
EXEC sp_CompleteFetchLog
    @LogId = @LogId,
    @Status = 'Success',
    @RatesImported = 150,
    @RatesUpdated = 50
```

**Indexes:**
- `IX_FetchLog_Provider_Date` on `(ProviderId, FetchStarted DESC)` - Provider history queries

---

### System Schema

#### SystemConfiguration
Global application configuration (key-value store).

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Key | NVARCHAR(100) | PK | Configuration key |
| Value | NVARCHAR(MAX) | NOT NULL | Configuration value |
| Description | NVARCHAR(500) | NULL | Setting description |
| DataType | NVARCHAR(20) | NOT NULL, DEFAULT 'String' | String/Int/Bool/DateTime/Decimal |
| Modified | DATETIMEOFFSET | NOT NULL, DEFAULT | Last modification time |
| ModifiedBy | INT | NULL, FK → User | User who last modified |

**Business Rules:**
- `DataType` must be one of: `String`, `Int`, `Bool`, `DateTime`, `Decimal`
- DataType is informational - helps application layer parse correctly

**Common Settings:**
- `FetchScheduleCron` - Cron expression for scheduled fetches
- `MaxConcurrentFetches` - Max parallel provider fetches
- `DataRetentionDays` - Days to keep old exchange rate data
- `HealthCheckIntervalMinutes` - Provider health check frequency

---

#### ErrorLog
Application-wide error and event logging.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | BIGINT IDENTITY | PK | Auto-incrementing identifier |
| Timestamp | DATETIMEOFFSET | NOT NULL, DEFAULT | Error occurrence time |
| Severity | NVARCHAR(20) | NOT NULL | Info/Warning/Error/Critical |
| Source | NVARCHAR(200) | NOT NULL | Component/class that logged |
| Message | NVARCHAR(MAX) | NOT NULL | Error message |
| Exception | NVARCHAR(MAX) | NULL | Exception type/details |
| StackTrace | NVARCHAR(MAX) | NULL | Full stack trace |
| UserId | INT | NULL, FK → User | Associated user (if applicable) |
| AdditionalData | NVARCHAR(MAX) | NULL | JSON with extra context |

**Business Rules:**
- Severity must be one of: `Info`, `Warning`, `Error`, `Critical`
- Consider archiving/purging old logs periodically

**Indexes:**
- `IX_ErrorLog_Timestamp` on `(Timestamp DESC)` - Recent error queries
- `IX_ErrorLog_Severity` on `(Severity, Timestamp DESC)` - Severity filtering

---

### User Schema

#### User
User accounts for authentication and authorization.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT IDENTITY | PK | Auto-incrementing identifier |
| Email | NVARCHAR(255) | NOT NULL, UNIQUE | User email (login) |
| PasswordHash | NVARCHAR(512) | NOT NULL | Hashed password |
| FirstName | NVARCHAR(255) | NOT NULL | First name |
| LastName | NVARCHAR(255) | NOT NULL | Last name |
| Role | NVARCHAR(255) | NOT NULL | Admin/Consumer |

**Business Rules:**
- Role must be one of: `Admin`, `Consumer`
- Email is case-insensitive (enforce in application)
- `PasswordHash` should use BCrypt or similar (never store plain text)

**Roles:**
- **Admin**: Full access - manage providers, view logs, system configuration
- **Consumer**: Read-only - view exchange rates, use API

**Indexes:**
- `IX_User_Email` - Fast email lookups for authentication
- `IX_User_Role` - Role-based queries

---

## Views

### Exchange Rate Views

#### vw_LatestExchangeRates
Returns the most recent exchange rate for each currency pair from active providers.

**Columns:**
- `Id`, `ProviderId`, `ProviderCode`, `ProviderName`
- `BaseCurrencyCode`, `TargetCurrencyCode`
- `Rate`, `Multiplier`, `RatePerUnit` (calculated)
- `ValidDate`, `DaysOld`, `FreshnessStatus`

**FreshnessStatus Logic:**
- `Current`: ValidDate = today
- `Recent`: 1 day old
- `Week Old`: 2-7 days old
- `Outdated`: > 7 days old

**Usage:**
```sql
-- Get latest EUR to CZK rate
SELECT * FROM vw_LatestExchangeRates
WHERE BaseCurrencyCode = 'EUR' AND TargetCurrencyCode = 'CZK';
```

---

#### vw_CurrentExchangeRates
Returns only today's exchange rates from active providers.

**Usage:**
```sql
-- Get all current rates
SELECT * FROM vw_CurrentExchangeRates
ORDER BY ProviderCode, TargetCurrencyCode;
```

---

#### vw_ExchangeRateHistory
Historical view of exchange rate changes over time.

**Usage:**
```sql
-- EUR to CZK trend over last 30 days
SELECT ValidDate, ProviderCode, RatePerUnit
FROM vw_ExchangeRateHistory
WHERE BaseCurrencyCode = 'EUR'
  AND TargetCurrencyCode = 'CZK'
  AND ValidDate >= DATEADD(DAY, -30, GETDATE())
ORDER BY ValidDate DESC;
```

---

#### vw_CurrencyPairAvailability
Shows which currency pairs are available from which providers.

**Usage:**
```sql
-- Find all providers offering USD rates
SELECT * FROM vw_CurrencyPairAvailability
WHERE TargetCurrencyCode = 'USD';
```

---

### Provider Views

#### vw_ProviderHealthStatus
Comprehensive provider health metrics including 30-day statistics.

**Key Metrics:**
- `HealthStatus`: Disabled/Critical/Degraded/Never Fetched/Stale/Warning/Healthy
- `HoursSinceLastSuccess`: Time since last successful fetch
- `TotalFetches30Days`: Total fetch attempts in last 30 days
- `SuccessfulFetches30Days`: Successful fetches in last 30 days
- `FailedFetches30Days`: Failed fetches in last 30 days
- `AvgFetchDurationMs`: Average successful fetch time

**Usage:**
```sql
-- Critical providers needing attention
SELECT Code, Name, HealthStatus, ConsecutiveFailures
FROM vw_ProviderHealthStatus
WHERE HealthStatus IN ('Critical', 'Degraded')
ORDER BY ConsecutiveFailures DESC;
```

---

#### vw_ProviderConfigurationSummary
Overview of provider configurations.

**Usage:**
```sql
-- View all provider configurations
SELECT * FROM vw_ProviderConfigurationSummary
ORDER BY ProviderCode, SettingKey;
```

---

#### vw_RecentFetchActivity
Recent fetch operations with details.

**Usage:**
```sql
-- Last 10 fetch operations
SELECT TOP 10 * FROM vw_RecentFetchActivity
ORDER BY FetchStarted DESC;
```

---

### System Views

#### vw_ErrorSummary
Aggregated error statistics by severity and source.

**Usage:**
```sql
-- Error summary for last 24 hours
SELECT * FROM vw_ErrorSummary
WHERE ErrorTimestamp >= DATEADD(HOUR, -24, GETDATE())
ORDER BY ErrorCount DESC;
```

---

#### vw_SystemHealthDashboard
Single-row view with system-wide health metrics.

**Metrics:**
- Provider counts (active/inactive/critical)
- Currency and rate counts
- Fetch statistics (24h)
- Error statistics (24h)
- User count
- Data freshness

**Usage:**
```sql
-- Quick system health check
SELECT * FROM vw_SystemHealthDashboard;
```

**Sample Output:**
```
ActiveProviders: 5
InactiveProviders: 1
CriticalProviders: 0
CurrentRatesCount: 145
SuccessfulFetches24h: 48
FailedFetches24h: 2
Errors24h: 3
LatestRateDate: 2025-11-05
```

---

## Stored Procedures

### sp_BulkUpsertExchangeRates
High-performance bulk insert/update of exchange rates from JSON.

**Parameters:**
- `@ProviderId INT` - Provider ID
- `@ValidDate DATE` - Date rates are valid for
- `@RatesJson NVARCHAR(MAX)` - JSON array of rates

**JSON Format:**
```json
[
  {"currencyCode": "USD", "rate": 24.335, "multiplier": 1},
  {"currencyCode": "EUR", "rate": 25.50, "multiplier": 1},
  {"currencyCode": "GBP", "rate": 29.123, "multiplier": 1}
]
```

**Features:**
- Validates provider exists
- Validates JSON format
- Validates rate positivity
- Uses `MERGE` for efficient upsert
- Skips currencies not in database
- Returns summary statistics
- Logs errors automatically
- Transactional (all or nothing)

**Returns:**
```sql
InsertedCount   UpdatedCount   SkippedCount   ProcessedCount   TotalInJson   Status
-------------   ------------   ------------   --------------   -----------   ------
150             50             5              200              205           SUCCESS
```

**Usage:**
```sql
DECLARE @RatesJson NVARCHAR(MAX) = N'[
  {"currencyCode":"USD","rate":24.335,"multiplier":1},
  {"currencyCode":"EUR","rate":25.50,"multiplier":1}
]';

EXEC sp_BulkUpsertExchangeRates
    @ProviderId = 1,
    @ValidDate = '2025-11-05',
    @RatesJson = @RatesJson;
```

**Error Handling:**
- Throws custom errors (50100-50199 range)
- Automatically logs to `ErrorLog` table
- Rolls back transaction on any error

---

### sp_StartFetchLog
Initiates a fetch operation audit log entry.

**Parameters:**
- `@ProviderId INT` - Provider being fetched
- `@RequestedBy INT` (optional) - User ID who requested (NULL for scheduled)

**Returns:**
```sql
LogId    FetchStarted                Status
-----    ------------------------    -------
12345    2025-11-05T10:30:00+00:00   SUCCESS
```

**Usage:**
```sql
DECLARE @LogId BIGINT;

EXEC sp_StartFetchLog @ProviderId = 1, @RequestedBy = NULL;
-- Capture @LogId from result set

-- ... perform fetch ...

EXEC sp_CompleteFetchLog @LogId = @LogId, @Status = 'Success', ...;
```

---

### sp_CompleteFetchLog
Completes a fetch operation and updates provider health.

**Parameters:**
- `@LogId BIGINT` - Log ID from `sp_StartFetchLog`
- `@Status NVARCHAR(20)` - Success/Failed/PartialSuccess
- `@RatesImported INT` (optional) - New rates inserted
- `@RatesUpdated INT` (optional) - Existing rates updated
- `@ErrorMessage NVARCHAR(MAX)` (optional) - Error details

**Provider Health Updates:**
- **Success**: Resets `ConsecutiveFailures` to 0, updates `LastSuccessfulFetch`
- **PartialSuccess**: Updates `LastSuccessfulFetch` but keeps failure count
- **Failed**: Increments `ConsecutiveFailures`, updates `LastFailedFetch`
  - Auto-disables provider after 10 consecutive failures
  - Logs critical event when auto-disabled

**Returns:**
```sql
LogId   Status    RatesImported   RatesUpdated   ProviderConsecutiveFailures   CompletionStatus
-----   -------   -------------   ------------   ---------------------------   ----------------
12345   Success   150             50             0                             SUCCESS
```

**Business Logic:**
```sql
IF Status = 'Failed' AND ConsecutiveFailures >= 10
    SET IsActive = 0  -- Auto-disable provider
    LOG 'Critical' event
```

**Usage:**
```sql
EXEC sp_CompleteFetchLog
    @LogId = 12345,
    @Status = 'Success',
    @RatesImported = 150,
    @RatesUpdated = 50;
```

---

## Indexing Strategy

### Performance Indexes

All indexes are created in `Script/Index.sql` and deployed automatically.

#### Exchange Rate Indexes
```sql
-- Fast rate lookups by target currency
IX_ExchangeRate_Lookup: (TargetCurrencyId, ValidDate DESC)
  INCLUDE (Rate, Multiplier, BaseCurrencyId, ProviderId)

-- Provider-specific queries
IX_ExchangeRate_Provider: (ProviderId, ValidDate DESC)

-- Base currency filtering
IX_ExchangeRate_BaseCurrency: (BaseCurrencyId, ValidDate DESC)
```

#### Provider Indexes
```sql
-- Active provider filtering
IX_ExchangeRateProvider_Active: (IsActive)
  INCLUDE (Code, Name, BaseCurrencyId)

-- Health monitoring (filtered index on active only)
IX_ExchangeRateProvider_Health: (ConsecutiveFailures, LastSuccessfulFetch)
  WHERE IsActive = 1

-- Provider lookups by code
IX_ExchangeRateProvider_Lookup: (Code)
  INCLUDE (Id, Name, Url, BaseCurrencyId, IsActive)
```

#### Audit & Error Indexes
```sql
-- Recent fetch history
IX_FetchLog_Provider_Date: (ProviderId, FetchStarted DESC)

-- Error queries
IX_ErrorLog_Timestamp: (Timestamp DESC)
IX_ErrorLog_Severity: (Severity, Timestamp DESC)
```

#### User Indexes
```sql
-- Authentication lookups
IX_User_Email: (Email)
  INCLUDE (PasswordHash, Role, FirstName, LastName)

-- Role-based queries
IX_User_Role: (Role)
  INCLUDE (Email, FirstName, LastName)
```

### Index Maintenance
- **Rebuild**: Monthly for high-activity tables (ExchangeRate, FetchLog)
- **Statistics**: Auto-update enabled
- **Fragmentation**: Monitor indexes with >30% fragmentation

---

## Seed Data

Seed scripts are run automatically during deployment via `Script.PostDeployment.sql`.

### SeedCurrencies.sql
Seeds common ISO 4217 currency codes:
- CZK (Czech Koruna)
- EUR (Euro)
- USD (US Dollar)
- GBP (British Pound)
- JPY (Japanese Yen)
- And 30+ more

**Idempotent**: Only inserts if currency doesn't exist.

---

### SeedUsers.sql
Creates default user accounts for development/testing.

**Default Users:**
- **Admin**: admin@example.com (Role: Admin)
- **Consumer**: consumer@example.com (Role: Consumer)

**⚠️ IMPORTANT**: Change default passwords before production deployment!

---

### SeedExchangeRateProvidersAndConfiguration.sql
Seeds initial exchange rate providers with configuration.

**Example Providers:**
- European Central Bank (ECB)
- Czech National Bank (CNB)
- ExchangeRate-API
- Fixer.io

Each provider includes:
- Base currency
- API URL
- Authentication requirements
- Default configuration settings

---

### SeedSystemConfiguration.sql
Seeds global system configuration.

**Example Settings:**
- `FetchScheduleCron`: "0 0 9 * * ?" (Daily at 9 AM)
- `MaxConcurrentFetches`: 3
- `DataRetentionDays`: 90

---

## Database Deployment

### Prerequisites

**Windows:**
- SQL Server LocalDB / SQL Server Express / SQL Server Developer Edition
- Visual Studio 2022 with SQL Server Data Tools (SSDT)
- OR SqlPackage CLI

**Mac/Linux:**
- Docker with SQL Server container (see `../setup-sqlserver.sh`)
- SqlPackage CLI (cross-platform)

---

### Deployment Method 1: Visual Studio (Windows)

#### Publish to ApiLayer Database (ExchangeRateUpdaterTest)

1. **Open Solution**
   ```
   Open ExchangeRateUpdater.sln in Visual Studio
   ```

2. **Right-click Database Project** → **Publish**

3. **Configure Connection**
   - **Target database connection**: Click **Edit**
   - **Server name**:
     - LocalDB: `(localdb)\MSSQLLocalDB`
     - SQL Server: `localhost` or server name
     - Docker (Mac/Linux): `localhost,1433`
   - **Authentication**:
     - LocalDB: Windows Authentication
     - Docker: SQL Server Authentication
       - Username: `sa`
       - Password: `YourStrong@Passw0rd` (from setup script)
   - **Database name**: `ExchangeRateUpdaterTest`
   - Click **OK**

4. **Configure Deployment Options**
   - Check **Deploy**
   - Optional: Click **Advanced**
     - ✅ Include transactional scripts
     - ✅ Automatically close database connections
     - ✅ Block incremental deployment if data loss
   - Click **OK**

5. **Save Profile** (Optional)
   - Click **Save Profile As**
   - Save as: `Database/Publish/ExchangeRateUpdaterTest.publish.xml`

6. **Publish**
   - Click **Publish**
   - Monitor **Data Tools Operations** window
   - Wait for: **"Publish succeeded."**

7. **Verify**
   ```sql
   -- In SQL Server Object Explorer
   -- Expand: (localdb)\MSSQLLocalDB → Databases → ExchangeRateUpdaterTest
   -- Verify tables, views, stored procedures exist
   ```

#### Publish to MicroApiLayer Database (ExchangeRateUpdaterMicroTest)

Repeat steps 1-7 above, but in step 3:
- **Database name**: `ExchangeRateUpdaterMicroTest`

**Save Profile As**: `Database/Publish/ExchangeRateUpdaterMicroTest.publish.xml`

---

### Deployment Method 2: SqlPackage CLI (Cross-Platform)

SqlPackage is the command-line tool for deploying SSDT projects.

#### Install SqlPackage

**Windows:**
```powershell
# Install via dotnet tool
dotnet tool install -g microsoft.sqlpackage

# Or download from:
# https://aka.ms/sqlpackage-windows
```

**Mac:**
```bash
# Install via Homebrew
brew install sqlpackage

# Or install via dotnet tool
dotnet tool install -g microsoft.sqlpackage
```

**Linux:**
```bash
# Install via dotnet tool
dotnet tool install -g microsoft.sqlpackage

# Or download from:
# https://aka.ms/sqlpackage-linux
```

#### Build Database Project

First, build the DACPAC file:

```bash
# From the Task directory
cd Database

# Build the project
dotnet build Database.sqlproj -c Release

# DACPAC will be created at:
# bin/Release/Database.dacpac
```

#### Publish to ExchangeRateUpdaterTest (REST)

**Windows (LocalDB):**
```powershell
sqlpackage /Action:Publish `
  /SourceFile:"Database\bin\Release\Database.dacpac" `
  /TargetConnectionString:"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;Database=ExchangeRateUpdaterTestREST;" `
  /p:IncludeTransactionalScripts=True `
  /p:BlockOnPossibleDataLoss=True
```

**Mac/Linux (Docker):**
```bash
sqlpackage /Action:Publish \
  /SourceFile:"Database/bin/Release/Database.dacpac" \
  /TargetConnectionString:"Server=localhost,1433;User Id=sa;Password=YourStrong@Passw0rd;Database=ExchangeRateUpdaterTestREST;TrustServerCertificate=True;" \
  /p:IncludeTransactionalScripts=True \
  /p:BlockOnPossibleDataLoss=True
```

#### Publish to ExchangeRateUpdaterMicroTest (gRPC)

**Windows (LocalDB):**
```powershell
sqlpackage /Action:Publish `
  /SourceFile:"Database\bin\Release\Database.dacpac" `
  /TargetConnectionString:"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;Database=ExchangeRateUpdaterTestgRPC;" `
  /p:IncludeTransactionalScripts=True `
  /p:BlockOnPossibleDataLoss=True
```

**Mac/Linux (Docker):**
```bash
sqlpackage /Action:Publish \
  /SourceFile:"Database/bin/Release/Database.dacpac" \
  /TargetConnectionString:"Server=localhost,1433;User Id=sa;Password=YourStrong@Passw0rd;Database=ExchangeRateUpdaterTestgRPC;TrustServerCertificate=True;" \
  /p:IncludeTransactionalScripts=True \
  /p:BlockOnPossibleDataLoss=True
```

#### Publish to ExchangeRateUpdaterMicroTest (SOAP)

**Windows (LocalDB):**
```powershell
sqlpackage /Action:Publish `
  /SourceFile:"Database\bin\Release\Database.dacpac" `
  /TargetConnectionString:"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;Database=ExchangeRateUpdaterTestSOAP;" `
  /p:IncludeTransactionalScripts=True `
  /p:BlockOnPossibleDataLoss=True
```

**Mac/Linux (Docker):**
```bash
sqlpackage /Action:Publish \
  /SourceFile:"Database/bin/Release/Database.dacpac" \
  /TargetConnectionString:"Server=localhost,1433;User Id=sa;Password=YourStrong@Passw0rd;Database=ExchangeRateUpdaterTestSOAP;TrustServerCertificate=True;" \
  /p:IncludeTransactionalScripts=True \
  /p:BlockOnPossibleDataLoss=True
```

#### Publish Options Explained

| Option | Description |
|--------|-------------|
| `/Action:Publish` | Deploy DACPAC to database |
| `/SourceFile` | Path to compiled DACPAC file |
| `/TargetConnectionString` | SQL Server connection string |
| `/p:IncludeTransactionalScripts` | Wrap deployment in transaction |
| `/p:BlockOnPossibleDataLoss` | Prevent destructive changes |

**Common Options:**
```bash
/p:DropObjectsNotInSource=False          # Don't drop objects missing in DACPAC
/p:AllowIncompatiblePlatform=True        # Allow deployment to different SQL versions
/p:CreateNewDatabase=True                # Create database if doesn't exist
/p:IgnoreColumnOrder=True                # Ignore column order changes
```

---

### Deployment Method 3: PowerShell Script (Windows)

Create a reusable deployment script.

**deploy-database.ps1:**
```powershell
param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("Test", "MicroTest")]
    [string]$Environment,

    [string]$Server = "(localdb)\MSSQLLocalDB"
)

$DatabaseName = if ($Environment -eq "Test") {
    "ExchangeRateUpdaterTest"
} else {
    "ExchangeRateUpdaterMicroTest"
}

Write-Host "Deploying to: $DatabaseName on $Server" -ForegroundColor Cyan

# Build DACPAC
Write-Host "Building Database project..." -ForegroundColor Yellow
dotnet build Database/Database.sqlproj -c Release

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}

# Publish
Write-Host "Publishing to database..." -ForegroundColor Yellow
sqlpackage /Action:Publish `
  /SourceFile:"Database/bin/Release/Database.dacpac" `
  /TargetConnectionString:"Server=$Server;Integrated Security=true;Database=$DatabaseName;" `
  /p:IncludeTransactionalScripts=True `
  /p:BlockOnPossibleDataLoss=True

if ($LASTEXITCODE -eq 0) {
    Write-Host "Deployment successful!" -ForegroundColor Green
} else {
    Write-Host "Deployment failed!" -ForegroundColor Red
    exit 1
}
```

**Usage:**
```powershell
# Deploy to ApiLayer database
.\deploy-database.ps1 -Environment Test

# Deploy to MicroApiLayer database
.\deploy-database.ps1 -Environment MicroTest

# Deploy to custom server
.\deploy-database.ps1 -Environment Test -Server "localhost\SQLEXPRESS"
```

---

### Deployment Method 4: Bash Script (Mac/Linux)

**deploy-database.sh:**
```bash
#!/bin/bash

set -e

ENVIRONMENT=$1
SERVER=${2:-"localhost,1433"}
SA_PASSWORD=${3:-"YourStrong@Passw0rd"}

if [ "$ENVIRONMENT" != "Test" ] && [ "$ENVIRONMENT" != "MicroTest" ]; then
    echo "Usage: $0 {Test|MicroTest} [server] [password]"
    exit 1
fi

DATABASE_NAME="ExchangeRateUpdater${ENVIRONMENT}"

echo "========================================="
echo "Deploying to: $DATABASE_NAME on $SERVER"
echo "========================================="

# Build DACPAC
echo ""
echo "Building Database project..."
dotnet build Database/Database.sqlproj -c Release

# Publish
echo ""
echo "Publishing to database..."
sqlpackage /Action:Publish \
  /SourceFile:"Database/bin/Release/Database.dacpac" \
  /TargetConnectionString:"Server=$SERVER;User Id=sa;Password=$SA_PASSWORD;Database=$DATABASE_NAME;TrustServerCertificate=True;" \
  /p:IncludeTransactionalScripts=True \
  /p:BlockOnPossibleDataLoss=True

echo ""
echo "Deployment successful!"
```

**Usage:**
```bash
# Make executable
chmod +x deploy-database.sh

# Deploy to ApiLayer database
./deploy-database.sh Test

# Deploy to MicroApiLayer database
./deploy-database.sh MicroTest

# Deploy with custom server/password
./deploy-database.sh Test "myserver,1433" "MyPassword123!"
```

---

### Verify Deployment

After deployment, verify the database was created correctly:

**SQL Query:**
```sql
-- Check tables
SELECT TABLE_SCHEMA, TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_SCHEMA, TABLE_NAME;

-- Check views
SELECT TABLE_SCHEMA, TABLE_NAME
FROM INFORMATION_SCHEMA.VIEWS
ORDER BY TABLE_SCHEMA, TABLE_NAME;

-- Check stored procedures
SELECT ROUTINE_SCHEMA, ROUTINE_NAME
FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_TYPE = 'PROCEDURE'
ORDER BY ROUTINE_SCHEMA, ROUTINE_NAME;

-- Check seed data
SELECT 'Currencies' AS DataType, COUNT(*) AS Count FROM Currency
UNION ALL
SELECT 'Users', COUNT(*) FROM [User]
UNION ALL
SELECT 'Providers', COUNT(*) FROM ExchangeRateProvider
UNION ALL
SELECT 'System Config', COUNT(*) FROM SystemConfiguration;
```

**Expected Results:**
- **Tables**: 8 (Currency, ExchangeRate, ExchangeRateProvider, ExchangeRateProviderConfiguration, ExchangeRateFetchLog, ErrorLog, SystemConfiguration, User)
- **Views**: 9 (vw_LatestExchangeRates, vw_CurrentExchangeRates, etc.)
- **Stored Procedures**: 3 (sp_BulkUpsertExchangeRates, sp_StartFetchLog, sp_CompleteFetchLog)
- **Currencies**: 35+
- **Users**: 2 (admin, consumer)
- **Providers**: 3-5
- **System Config**: 5+

---

## Common Queries

### Get Latest Rates for a Currency

```sql
-- Get all latest rates for EUR
SELECT
    TargetCurrencyCode,
    RatePerUnit,
    ValidDate,
    ProviderCode
FROM vw_LatestExchangeRates
WHERE BaseCurrencyCode = 'EUR'
ORDER BY TargetCurrencyCode;
```

### Check Provider Health

```sql
-- Health status of all providers
SELECT
    Code,
    Name,
    HealthStatus,
    ConsecutiveFailures,
    HoursSinceLastSuccess,
    SuccessfulFetches30Days,
    FailedFetches30Days
FROM vw_ProviderHealthStatus
ORDER BY
    CASE HealthStatus
        WHEN 'Critical' THEN 1
        WHEN 'Degraded' THEN 2
        WHEN 'Stale' THEN 3
        WHEN 'Warning' THEN 4
        WHEN 'Healthy' THEN 5
        WHEN 'Never Fetched' THEN 6
        WHEN 'Disabled' THEN 7
    END;
```

### Recent Fetch History

```sql
-- Last 20 fetch operations
SELECT TOP 20
    fl.Id,
    p.Code AS ProviderCode,
    fl.Status,
    fl.FetchStarted,
    fl.DurationMs,
    fl.RatesImported,
    fl.RatesUpdated,
    fl.ErrorMessage
FROM ExchangeRateFetchLog fl
INNER JOIN ExchangeRateProvider p ON fl.ProviderId = p.Id
ORDER BY fl.FetchStarted DESC;
```

### System Health Dashboard

```sql
-- Overall system status
SELECT * FROM vw_SystemHealthDashboard;
```

### Find Missing Currency Pairs

```sql
-- Currency pairs not available for today
WITH AllPairs AS (
    SELECT
        bc.Code AS BaseCurrency,
        tc.Code AS TargetCurrency
    FROM Currency bc
    CROSS JOIN Currency tc
    WHERE bc.Id <> tc.Id
)
SELECT
    ap.BaseCurrency,
    ap.TargetCurrency
FROM AllPairs ap
LEFT JOIN vw_CurrentExchangeRates cer
    ON ap.BaseCurrency = cer.BaseCurrencyCode
    AND ap.TargetCurrency = cer.TargetCurrencyCode
WHERE cer.Id IS NULL
ORDER BY ap.BaseCurrency, ap.TargetCurrency;
```

### Error Analysis

```sql
-- Error breakdown by severity (last 7 days)
SELECT
    Severity,
    COUNT(*) AS ErrorCount,
    COUNT(DISTINCT Source) AS AffectedSources,
    MIN(Timestamp) AS FirstOccurrence,
    MAX(Timestamp) AS LastOccurrence
FROM ErrorLog
WHERE Timestamp >= DATEADD(DAY, -7, GETDATE())
GROUP BY Severity
ORDER BY
    CASE Severity
        WHEN 'Critical' THEN 1
        WHEN 'Error' THEN 2
        WHEN 'Warning' THEN 3
        WHEN 'Info' THEN 4
    END;
```

---

## Maintenance

### Data Retention

Consider implementing data retention policies:

**Archive Old Exchange Rates:**
```sql
-- Archive rates older than 90 days to separate table
INSERT INTO ExchangeRateArchive
SELECT * FROM ExchangeRate
WHERE ValidDate < DATEADD(DAY, -90, GETDATE());

DELETE FROM ExchangeRate
WHERE ValidDate < DATEADD(DAY, -90, GETDATE());
```

**Purge Old Logs:**
```sql
-- Delete fetch logs older than 1 year
DELETE FROM ExchangeRateFetchLog
WHERE FetchStarted < DATEADD(YEAR, -1, GETDATE());

-- Delete error logs older than 6 months
DELETE FROM ErrorLog
WHERE Timestamp < DATEADD(MONTH, -6, GETDATE())
  AND Severity NOT IN ('Critical'); -- Keep critical errors longer
```

### Index Maintenance

**Rebuild Fragmented Indexes:**
```sql
-- Find fragmented indexes
SELECT
    OBJECT_NAME(ips.object_id) AS TableName,
    i.name AS IndexName,
    ips.avg_fragmentation_in_percent
FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'LIMITED') ips
INNER JOIN sys.indexes i ON ips.object_id = i.object_id
    AND ips.index_id = i.index_id
WHERE ips.avg_fragmentation_in_percent > 30
  AND ips.page_count > 1000
ORDER BY ips.avg_fragmentation_in_percent DESC;

-- Rebuild indexes
ALTER INDEX IX_ExchangeRate_Lookup ON ExchangeRate REBUILD;
```

### Update Statistics

```sql
-- Update statistics for key tables
UPDATE STATISTICS ExchangeRate WITH FULLSCAN;
UPDATE STATISTICS ExchangeRateProvider WITH FULLSCAN;
UPDATE STATISTICS ExchangeRateFetchLog WITH FULLSCAN;
```

---

## Troubleshooting

### Deployment Fails with "Object Already Exists"

**Problem:** Database has objects not in DACPAC.

**Solution:**
```bash
# Option 1: Drop and recreate database
sqlpackage /Action:Publish /p:DropObjectsNotInSource=True

# Option 2: Manually drop conflicting objects
```

### Seed Data Not Inserted

**Problem:** Seed scripts didn't run.

**Cause:** `Script.PostDeployment.sql` not executed.

**Solution:**
- Ensure DACPAC includes post-deployment script
- Check build output for `Database.dacpac`
- Verify `Script.PostDeployment.sql` is marked as **PostDeploy** in `.sqlproj`

### Performance Issues

**Symptoms:**
- Slow rate queries
- Long fetch log queries

**Diagnosis:**
```sql
-- Check missing indexes
SELECT * FROM sys.dm_db_missing_index_details
ORDER BY avg_user_impact * (user_seeks + user_scans) DESC;

-- Check index usage
SELECT
    OBJECT_NAME(s.object_id) AS TableName,
    i.name AS IndexName,
    s.user_seeks,
    s.user_scans,
    s.user_lookups,
    s.user_updates
FROM sys.dm_db_index_usage_stats s
INNER JOIN sys.indexes i ON s.object_id = i.object_id
    AND s.index_id = i.index_id
WHERE OBJECTPROPERTY(s.object_id, 'IsUserTable') = 1
ORDER BY s.user_seeks + s.user_scans + s.user_lookups DESC;
```

### Connection Issues (Docker)

**Problem:** Cannot connect to `localhost,1433`

**Solutions:**
```bash
# Check container is running
docker ps | grep sqlserver

# Check port mapping
docker port sqlserver-localdb

# Verify connection string includes TrustServerCertificate=True
```

### Foreign Key Violations

**Problem:** Cannot insert data due to FK constraints.

**Cause:** Missing parent records (e.g., Currency not in database).

**Solution:**
```sql
-- Check if currency exists before using
IF NOT EXISTS (SELECT 1 FROM Currency WHERE Code = 'XYZ')
BEGIN
    INSERT INTO Currency (Code) VALUES ('XYZ');
END
```

---

## Schema Evolution

### Adding New Columns

1. Add column to table definition
2. Set default value for existing rows
3. Deploy via SqlPackage or Visual Studio

**Example:**
```sql
-- Add Priority column to ExchangeRateProvider
ALTER TABLE ExchangeRateProvider
ADD Priority INT NOT NULL DEFAULT 0;
```

### Adding New Tables

1. Create table SQL file
2. Add to `Database.sqlproj` as `<Build Include>`
3. Rebuild and publish

### Updating Stored Procedures

- Procedures are redeployed automatically
- Changes are applied via `ALTER PROCEDURE`
- No data loss

### Updating Views

- Views are redeployed automatically
- Changes are applied via `ALTER VIEW`
- Dependent views may need refresh

---

## Best Practices

1. **Always use DACPAC deployment** - Never manually modify production databases
2. **Test schema changes** - Deploy to test database first
3. **Version control everything** - All schema changes in Git
4. **Use transactions** - Enable `/p:IncludeTransactionalScripts=True`
5. **Backup before deployment** - Create backup of production database
6. **Monitor performance** - Check query execution plans regularly
7. **Archive old data** - Implement retention policies
8. **Document changes** - Update this README when schema changes
9. **Use views for complex queries** - Avoid duplicating logic
10. **Validate data** - Use CHECK constraints and foreign keys

---

## References

- [SQL Server Data Tools (SSDT)](https://learn.microsoft.com/en-us/sql/ssdt/sql-server-data-tools)
- [SqlPackage CLI Reference](https://learn.microsoft.com/en-us/sql/tools/sqlpackage)
- [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)
- [Database Project Best Practices](https://learn.microsoft.com/en-us/sql/ssdt/sql-server-database-project-best-practices)
- [T-SQL Reference](https://learn.microsoft.com/en-us/sql/t-sql/language-reference)
