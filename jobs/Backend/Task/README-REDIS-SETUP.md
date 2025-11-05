# Redis Setup Instructions

This guide explains how to install and start Redis for development across different platforms.

## Quick Start

### Windows
```powershell
# Basic setup (no password, no persistence)
.\setup-redis.ps1

# With password and persistence
.\setup-redis.ps1 -Password "YourRedisPassword123" -WithPersistence

# Custom port
.\setup-redis.ps1 -Port 6380
```

### Mac
```bash
# Native installation (recommended for Mac)
./setup-redis.sh

# Using Docker
./setup-redis.sh --docker

# With password and persistence
./setup-redis.sh --docker --password "YourRedisPassword123" --persistence
```

### Linux
```bash
# Native installation (recommended for Linux)
./setup-redis.sh

# Using Docker
./setup-redis.sh --docker

# With options
./setup-redis.sh --docker --password "YourRedisPassword123" --persistence --port 6380
```

## Windows Setup

Redis doesn't have official Windows support. The setup script uses Docker Desktop.

### Prerequisites
- Windows 10/11
- Docker Desktop installed and running
- PowerShell

### Installation

**Option 1: Using PowerShell**
```powershell
# Run PowerShell (Administrator not required unless Docker needs it)
.\setup-redis.ps1
```

**Option 2: Using Batch File**
```cmd
REM Double-click or run from command prompt
setup-redis.bat
```

### PowerShell Parameters

```powershell
# Container name (default: redis-dev)
.\setup-redis.ps1 -ContainerName "my-redis"

# Port (default: 6379)
.\setup-redis.ps1 -Port 6380

# Password (default: none - WARNING: insecure!)
.\setup-redis.ps1 -Password "YourSecurePassword"

# Enable persistence (default: false)
.\setup-redis.ps1 -WithPersistence

# Combined example
.\setup-redis.ps1 -Password "Redis123!" -WithPersistence -Port 6380
```

### Connection String (Windows)
```
# Without password
localhost:6379

# With password
localhost:6379,password=YourPassword

# C# StackExchange.Redis
ConfigurationOptions.Parse("localhost:6379,password=YourPassword")
```

## Mac Setup

### Native Installation (Recommended)

Using Homebrew is the easiest method on Mac:

```bash
# Make script executable
chmod +x setup-redis.sh

# Install and start Redis
./setup-redis.sh
```

This will:
1. Check if Homebrew is installed
2. Install Redis via Homebrew if not present
3. Start Redis as a background service
4. Enable Redis to start on system boot

### Docker Installation

If you prefer Docker or don't have Homebrew:

```bash
./setup-redis.sh --docker --password "YourPassword" --persistence
```

### Mac Commands

**Native (Homebrew):**
```bash
# Stop Redis
brew services stop redis

# Start Redis
brew services start redis

# Restart Redis
brew services restart redis

# Redis CLI
redis-cli

# Configuration
vim $(brew --prefix)/etc/redis.conf
```

**Docker:**
```bash
# Stop container
docker stop redis-dev

# Start container
docker start redis-dev

# View logs
docker logs redis-dev

# Redis CLI
docker exec -it redis-dev redis-cli
```

## Linux Setup

### Native Installation (Recommended)

The script automatically detects your package manager:

```bash
# Make script executable
chmod +x setup-redis.sh

# Install and start Redis
./setup-redis.sh
```

Supports:
- **Debian/Ubuntu**: Uses `apt-get`
- **RHEL/CentOS/Fedora**: Uses `yum`

### Docker Installation

```bash
./setup-redis.sh --docker --password "YourPassword" --persistence
```

### Linux Commands

**Native (systemd):**
```bash
# Stop Redis
sudo systemctl stop redis-server  # Debian/Ubuntu
sudo systemctl stop redis          # RHEL/CentOS/Fedora

# Start Redis
sudo systemctl start redis-server

# Restart Redis
sudo systemctl restart redis-server

# Status
sudo systemctl status redis-server

# Redis CLI
redis-cli

# Configuration
sudo vim /etc/redis/redis.conf     # Debian/Ubuntu
sudo vim /etc/redis.conf           # RHEL/CentOS/Fedora
```

**Docker:**
Same as Mac Docker commands above.

## Configuration Options

### Password Protection

**Why use a password?**
- Prevents unauthorized access
- Required for production environments
- Good security practice

**Setting a password:**

**Windows/Docker:**
```powershell
.\setup-redis.ps1 -Password "YourStrongPassword"
```

**Mac/Linux Docker:**
```bash
./setup-redis.sh --docker --password "YourStrongPassword"
```

**Native installation:**
Edit the configuration file and add:
```conf
requirepass YourStrongPassword
```

Then restart Redis.

### Data Persistence

By default, Redis stores data in memory only. Data is lost on restart.

**Enable persistence:**

**Docker:**
```powershell
# Windows
.\setup-redis.ps1 -WithPersistence

# Mac/Linux
./setup-redis.sh --docker --persistence
```

**Native installation:**
Edit configuration file:
```conf
# Enable AOF (Append-Only File)
appendonly yes
appendfsync everysec

# RDB snapshots (default - already enabled)
save 900 1
save 300 10
save 60 10000
```

**Persistence modes:**
- **RDB**: Periodic snapshots (faster, but may lose recent data)
- **AOF**: Logs every write operation (slower, but more durable)
- **Both**: Maximum durability (recommended for production)

### Custom Port

**Docker:**
```powershell
# Windows
.\setup-redis.ps1 -Port 6380

# Mac/Linux
./setup-redis.sh --docker --port 6380
```

**Native:**
Edit configuration and change:
```conf
port 6380
```

## Testing Redis

### Using redis-cli

**Check if Redis is running:**
```bash
# Without password
redis-cli PING
# Expected output: PONG

# With password
redis-cli -a YourPassword PING
```

**Basic operations:**
```bash
# Connect to Redis
redis-cli -a YourPassword

# Set a value
SET mykey "Hello Redis"

# Get a value
GET mykey

# List all keys
KEYS *

# Delete a key
DEL mykey

# Exit
exit
```

### Using C# (StackExchange.Redis)

```csharp
using StackExchange.Redis;

// Connect
var redis = ConnectionMultiplexer.Connect("localhost:6379,password=YourPassword");
var db = redis.GetDatabase();

// Set value
db.StringSet("mykey", "Hello Redis");

// Get value
var value = db.StringGet("mykey");
Console.WriteLine(value); // Output: Hello Redis

// Check if key exists
bool exists = db.KeyExists("mykey");
```

## Connection Strings

### Without Password
```
localhost:6379
```

### With Password
```
localhost:6379,password=YourPassword
```

### Advanced Configuration
```
localhost:6379,password=YourPassword,ssl=false,abortConnect=false,connectTimeout=5000
```

### appsettings.json Example
```json
{
  "Redis": {
    "Configuration": "localhost:6379,password=YourPassword",
    "InstanceName": "ExchangeRateApp:"
  }
}
```

## Security Best Practices

### Development
- Use passwords even in development
- Don't commit passwords to source control
- Use different passwords for each environment

### Production
- **Always** use password authentication
- Enable TLS/SSL encryption
- Bind to localhost only (unless clustering)
- Use firewall rules to restrict access
- Enable AOF persistence
- Regular backups
- Monitor Redis memory usage

### Password Management
- Use environment variables
- Azure Key Vault or similar secret management
- .NET User Secrets for local development
- Minimum 16 characters for production passwords

## Troubleshooting

### Windows

**Issue: "Docker is not running"**
- Start Docker Desktop
- Wait for Docker to fully initialize
- Check system tray for Docker icon

**Issue: "Port 6379 already in use"**
- Another Redis instance is running
- Check: `docker ps` to see running containers
- Stop other instance or use different port: `.\setup-redis.ps1 -Port 6380`

**Issue: Container won't start**
- Check Docker Desktop is running
- View logs: `docker logs redis-dev`
- Remove and recreate: `docker rm -f redis-dev` then rerun script

### Mac

**Issue: "Homebrew not found"**
- Install Homebrew: `/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"`
- Or use Docker: `./setup-redis.sh --docker`

**Issue: "Permission denied"**
- Make script executable: `chmod +x setup-redis.sh`

**Issue: Redis won't start**
- Check logs: `brew services list`
- View logs: `tail -f $(brew --prefix)/var/log/redis.log`

### Linux

**Issue: "Package manager not found"**
- Install Docker and use: `./setup-redis.sh --docker`
- Or compile Redis from source

**Issue: "Permission denied"**
- Use sudo for native installation (script prompts automatically)
- Make script executable: `chmod +x setup-redis.sh`

**Issue: Redis service won't start**
- Check status: `sudo systemctl status redis-server`
- View logs: `sudo journalctl -u redis-server -n 50`
- Check port conflict: `sudo netstat -tlnp | grep 6379`

### General

**Issue: Can't connect to Redis**
- Verify Redis is running: `redis-cli PING` or `docker ps`
- Check firewall settings
- Verify correct host/port
- Check password (if set)

**Issue: "NOAUTH Authentication required"**
- Redis requires password but none provided
- Use: `redis-cli -a YourPassword`

**Issue: Out of memory**
- Redis stores data in RAM
- Check memory: `redis-cli INFO memory`
- Configure maxmemory: `redis-cli CONFIG SET maxmemory 256mb`
- Configure eviction policy: `redis-cli CONFIG SET maxmemory-policy allkeys-lru`

## Cleanup

### Remove Docker Container and Data

**Windows:**
```powershell
docker rm -f redis-dev
docker volume rm redis-dev-data
```

**Mac/Linux:**
```bash
docker rm -f redis-dev
docker volume rm redis-dev-data
```

### Uninstall Native Redis

**Mac:**
```bash
brew services stop redis
brew uninstall redis
```

**Linux (Debian/Ubuntu):**
```bash
sudo systemctl stop redis-server
sudo systemctl disable redis-server
sudo apt-get remove --purge redis-server
sudo apt-get autoremove
```

**Linux (RHEL/CentOS/Fedora):**
```bash
sudo systemctl stop redis
sudo systemctl disable redis
sudo yum remove redis
```

## Additional Resources

- [Redis Official Documentation](https://redis.io/documentation)
- [Redis Commands](https://redis.io/commands)
- [StackExchange.Redis Documentation](https://stackexchange.github.io/StackExchange.Redis/)
- [Redis Data Types](https://redis.io/docs/data-types/)
- [Redis Persistence](https://redis.io/docs/management/persistence/)
- [Redis Security](https://redis.io/docs/management/security/)
