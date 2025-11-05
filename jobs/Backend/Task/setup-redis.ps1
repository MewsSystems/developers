# PowerShell script to install and start Redis on Windows
# Redis has no official Windows support - this script uses Docker
# Run this script as Administrator for Docker operations

param(
    [string]$ContainerName = "redis-dev",
    [int]$Port = 6379,
    [string]$Password = "",
    [switch]$WithPersistence = $false
)

Write-Host "=== Redis Setup Script for Windows ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "Note: Redis doesn't have official Windows support." -ForegroundColor Yellow
Write-Host "This script uses Docker to run Redis." -ForegroundColor Yellow
Write-Host ""

# Check if Docker is installed
$dockerInstalled = $false
try {
    $dockerVersion = docker --version 2>$null
    if ($LASTEXITCODE -eq 0) {
        $dockerInstalled = $true
        Write-Host "Docker is installed: $dockerVersion" -ForegroundColor Green
    }
} catch {
    Write-Host "Docker is not installed." -ForegroundColor Red
}

if (-not $dockerInstalled) {
    Write-Host ""
    Write-Host "Please install Docker Desktop for Windows:" -ForegroundColor Yellow
    Write-Host "https://www.docker.com/products/docker-desktop" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Alternative: Use WSL2 and install Redis natively in Linux" -ForegroundColor Yellow
    exit 1
}

# Check if Docker is running
try {
    docker info >$null 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Docker is not running." -ForegroundColor Red
        Write-Host "Please start Docker Desktop and try again." -ForegroundColor Yellow
        exit 1
    }
    Write-Host "Docker is running." -ForegroundColor Green
} catch {
    Write-Host "Docker is not running." -ForegroundColor Red
    Write-Host "Please start Docker Desktop and try again." -ForegroundColor Yellow
    exit 1
}

Write-Host ""

# Check if port is already in use
$portInUse = Get-NetTCPConnection -LocalPort $Port -ErrorAction SilentlyContinue
if ($portInUse) {
    Write-Host "Warning: Port $Port is already in use." -ForegroundColor Yellow
    Write-Host "This may cause issues if not used by an existing Redis container." -ForegroundColor Yellow
    Write-Host ""
}

# Check if container already exists
$existingContainer = docker ps -a --filter "name=^${ContainerName}$" --format "{{.Names}}"

if ($existingContainer -eq $ContainerName) {
    Write-Host "Redis container '$ContainerName' already exists." -ForegroundColor Yellow

    # Check if it's running
    $runningContainer = docker ps --filter "name=^${ContainerName}$" --format "{{.Names}}"

    if ($runningContainer -eq $ContainerName) {
        Write-Host "Container is already running." -ForegroundColor Green
    } else {
        Write-Host "Starting existing container..." -ForegroundColor Yellow
        docker start $ContainerName

        if ($LASTEXITCODE -eq 0) {
            Write-Host "Container started successfully." -ForegroundColor Green
        } else {
            Write-Host "Error starting container." -ForegroundColor Red
            exit 1
        }
    }
} else {
    Write-Host "Creating and starting Redis container..." -ForegroundColor Cyan
    Write-Host ""

    # Build docker run command
    $dockerArgs = @(
        "run"
        "-d"
        "--name", $ContainerName
        "-p", "${Port}:6379"
    )

    # Add persistence if requested
    if ($WithPersistence) {
        Write-Host "Enabling persistence (AOF + RDB)..." -ForegroundColor Cyan
        $volumeName = "${ContainerName}-data"

        # Create volume if it doesn't exist
        docker volume create $volumeName >$null 2>&1

        $dockerArgs += "-v"
        $dockerArgs += "${volumeName}:/data"
    }

    # Add password if provided
    if ($Password) {
        Write-Host "Setting password protection..." -ForegroundColor Cyan
        $dockerArgs += "redis:7-alpine"
        $dockerArgs += "redis-server"
        $dockerArgs += "--requirepass"
        $dockerArgs += $Password

        if ($WithPersistence) {
            $dockerArgs += "--appendonly"
            $dockerArgs += "yes"
        }
    } else {
        Write-Host "Warning: No password set. Redis will accept connections without authentication." -ForegroundColor Yellow
        $dockerArgs += "redis:7-alpine"

        if ($WithPersistence) {
            $dockerArgs += "redis-server"
            $dockerArgs += "--appendonly"
            $dockerArgs += "yes"
        }
    }

    # Run the container
    $containerId = docker @dockerArgs

    if ($LASTEXITCODE -eq 0) {
        Write-Host "Redis container created and started successfully." -ForegroundColor Green
        Write-Host "Container ID: $containerId" -ForegroundColor Gray
    } else {
        Write-Host "Error creating container." -ForegroundColor Red
        exit 1
    }

    # Wait for Redis to start
    Write-Host ""
    Write-Host "Waiting for Redis to start..." -ForegroundColor Cyan
    Start-Sleep -Seconds 3
}

# Test Redis connection
Write-Host ""
Write-Host "Testing Redis connection..." -ForegroundColor Cyan

if ($Password) {
    $testResult = docker exec $ContainerName redis-cli -a $Password PING 2>$null
} else {
    $testResult = docker exec $ContainerName redis-cli PING 2>$null
}

if ($testResult -eq "PONG") {
    Write-Host "Redis is responding correctly!" -ForegroundColor Green
} else {
    Write-Host "Warning: Could not verify Redis is responding." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "=== Setup Complete ===" -ForegroundColor Green
Write-Host ""
Write-Host "Connection details:" -ForegroundColor Cyan
Write-Host "  Host: localhost" -ForegroundColor White
Write-Host "  Port: $Port" -ForegroundColor White
if ($Password) {
    Write-Host "  Password: $Password" -ForegroundColor White
} else {
    Write-Host "  Password: (none - insecure!)" -ForegroundColor Yellow
}
if ($WithPersistence) {
    Write-Host "  Persistence: Enabled (AOF + RDB)" -ForegroundColor White
} else {
    Write-Host "  Persistence: Disabled (data will be lost on restart)" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Connection string:" -ForegroundColor Cyan
if ($Password) {
    Write-Host "  localhost:${Port},password=${Password}" -ForegroundColor White
} else {
    Write-Host "  localhost:${Port}" -ForegroundColor White
}

Write-Host ""
Write-Host "Container management:" -ForegroundColor Cyan
Write-Host "  View logs:    docker logs $ContainerName" -ForegroundColor White
Write-Host "  Stop:         docker stop $ContainerName" -ForegroundColor White
Write-Host "  Start:        docker start $ContainerName" -ForegroundColor White
Write-Host "  Remove:       docker rm -f $ContainerName" -ForegroundColor White
if ($WithPersistence) {
    Write-Host "  Remove data:  docker volume rm ${ContainerName}-data" -ForegroundColor White
}

Write-Host ""
Write-Host "Redis CLI:" -ForegroundColor Cyan
if ($Password) {
    Write-Host "  docker exec -it $ContainerName redis-cli -a $Password" -ForegroundColor White
} else {
    Write-Host "  docker exec -it $ContainerName redis-cli" -ForegroundColor White
}

Write-Host ""
