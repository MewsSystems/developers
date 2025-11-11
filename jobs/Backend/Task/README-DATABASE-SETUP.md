# Database Setup Instructions

This guide explains how to install and start LocalDB (Windows) or SQL Server (Mac/Linux) for the ExchangeRateUpdater project.

## Windows - LocalDB

LocalDB is a lightweight version of SQL Server Express designed for development.

### Prerequisites
- Windows OS
- Administrator privileges

### Installation

**Option 1: Using PowerShell (Recommended)**
```powershell
# Run PowerShell as Administrator
.\setup-localdb.ps1
```

**Option 2: Using Batch File**
```cmd
REM Right-click and "Run as Administrator"
setup-localdb.bat
```

### Connection String
```
Server=(localdb)\MSSQLLocalDB;Integrated Security=true;Database=ExchangeRateUpdaterTest;
```

### Common Commands
```powershell
# List instances
SqlLocalDB.exe i

# Get instance info
SqlLocalDB.exe info MSSQLLocalDB

# Start instance
SqlLocalDB.exe start MSSQLLocalDB

# Stop instance
SqlLocalDB.exe stop MSSQLLocalDB

# Delete instance
SqlLocalDB.exe delete MSSQLLocalDB
```

## Mac / Linux - SQL Server in Docker

Since LocalDB is Windows-only, we use Docker to run SQL Server.

### Prerequisites
- Docker Desktop installed and running
- Bash shell

### Installation

```bash
# Make script executable
chmod +x setup-sqlserver.sh

# Run the script
./setup-sqlserver.sh
```

### Connection String
```
Server=localhost,1433;User Id=sa;Password=YourStrong@Passw0rd;Database=ExchangeRateUpdaterTest;TrustServerCertificate=True;
```

### Docker Commands
```bash
# View logs
docker logs sqlserver-localdb

# Stop container
docker stop sqlserver-localdb

# Start container
docker start sqlserver-localdb

# Remove container
docker rm -f sqlserver-localdb

# Connect using sqlcmd
docker exec -it sqlserver-localdb /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'YourStrong@Passw0rd'
```

## Updating Connection Strings

After setting up the database, update your connection strings in:

- `appsettings.json`
- `appsettings.Development.json`
- Environment variables

### Example for Windows (LocalDB):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Database=ExchangeRateUpdaterTest;"
  }
}
```

### Example for Mac/Linux (Docker):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;User Id=sa;Password=YourStrong@Passw0rd;Database=ExchangeRateUpdaterTest;TrustServerCertificate=True;"
  }
}
```

## Security Notes

### For Development (Docker):
- The default password in the script is for development only
- **Never commit passwords to source control**
- For production, use:
  - Strong passwords
  - Environment variables
  - Azure Key Vault or similar secret management
  - User Secrets for local development

### Change Docker Password:
```bash
# Edit the setup-sqlserver.sh file and change:
SA_PASSWORD="YourNewStrongPassword123!"
```

## Troubleshooting

### Windows LocalDB

**Issue: "SqlLocalDB.exe is not recognized"**
- LocalDB may not be in PATH
- Use full path: `"C:\Program Files\Microsoft SQL Server\160\Tools\Binn\SqlLocalDB.exe"`
- Or reinstall LocalDB

**Issue: "Instance already exists"**
- The script will start the existing instance
- To recreate: `SqlLocalDB.exe delete MSSQLLocalDB` then rerun script

### Mac/Linux Docker

**Issue: "Docker is not running"**
- Start Docker Desktop
- Wait for Docker to fully initialize

**Issue: "Port 1433 already in use"**
- Another SQL Server instance is using the port
- Stop the other instance or change the port in the script:
  ```bash
  SQL_PORT="14330"
  ```

**Issue: "Cannot connect to SQL Server"**
- Wait 30-60 seconds for SQL Server to fully start
- Check container logs: `docker logs sqlserver-localdb`
- Verify container is running: `docker ps`

**Issue: Password doesn't meet requirements**
- SQL Server requires strong passwords:
  - At least 8 characters
  - Contains uppercase, lowercase, numbers, and symbols
  - Example: `YourStrong@Passw0rd123`

**Issue: "sqlcmd not found" in container**
- SQL Server 2022 uses `/opt/mssql-tools18/bin/sqlcmd`
- The script auto-detects the correct path
- Manually check with: `docker exec sqlserver-localdb ls /opt/mssql-tools*/bin/`

## Database Deployment

After setting up LocalDB/SQL Server, deploy your database:

### Using Visual Studio
1. Open `Database.sqlproj`
2. Right-click project → Publish
3. Select target database
4. Click Publish

### Using SqlPackage (Command Line)
```bash
# Build the database project first
dotnet build Database/Database.sqlproj

# Publish to LocalDB (Windows)
SqlPackage.exe /Action:Publish \
  /SourceFile:Database/bin/Debug/Database.dacpac \
  /TargetConnectionString:"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;Database=ExchangeRateUpdaterTest;"

# Publish to Docker (Mac/Linux)
SqlPackage /Action:Publish \
  /SourceFile:Database/bin/Debug/Database.dacpac \
  /TargetConnectionString:"Server=localhost,1433;User Id=sa;Password=YourStrong@Passw0rd;Database=ExchangeRateUpdaterTest;TrustServerCertificate=True;"
```
