# PowerShell script to install and start SQL Server LocalDB on Windows
# Run this script as Administrator

param(
    [string]$InstanceName = "MSSQLLocalDB",
    [string]$DatabaseName = "ExchangeRateDB"
)

Write-Host "=== SQL Server LocalDB Setup Script ===" -ForegroundColor Cyan
Write-Host ""

# Check if LocalDB is already installed
$localDbInstalled = $false
try {
    $sqlLocalDbInfo = & SqlLocalDB.exe i
    if ($LASTEXITCODE -eq 0) {
        $localDbInstalled = $true
        Write-Host "LocalDB is already installed." -ForegroundColor Green
    }
} catch {
    Write-Host "LocalDB is not installed." -ForegroundColor Yellow
}

# Install LocalDB if not present
if (-not $localDbInstalled) {
    Write-Host "Installing SQL Server Express LocalDB..." -ForegroundColor Yellow
    Write-Host ""

    # Download SQL Server Express LocalDB
    $downloadUrl = "https://download.microsoft.com/download/3/8/d/38de7036-2433-4207-8eae-06e247e17b25/SqlLocalDB.msi"
    $installerPath = "$env:TEMP\SqlLocalDB.msi"

    Write-Host "Downloading LocalDB installer..." -ForegroundColor Yellow
    try {
        Invoke-WebRequest -Uri $downloadUrl -OutFile $installerPath -UseBasicParsing
        Write-Host "Download complete." -ForegroundColor Green
    } catch {
        Write-Host "Error downloading LocalDB installer: $_" -ForegroundColor Red
        Write-Host ""
        Write-Host "Please download and install SQL Server Express LocalDB manually from:" -ForegroundColor Yellow
        Write-Host "https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb" -ForegroundColor Cyan
        exit 1
    }

    # Install LocalDB
    Write-Host "Installing LocalDB (this may take a few minutes)..." -ForegroundColor Yellow
    try {
        Start-Process msiexec.exe -ArgumentList "/i `"$installerPath`" /qn IACCEPTSQLLOCALDBLICENSETERMS=YES" -Wait -NoNewWindow
        Write-Host "LocalDB installed successfully." -ForegroundColor Green

        # Clean up installer
        Remove-Item $installerPath -Force
    } catch {
        Write-Host "Error installing LocalDB: $_" -ForegroundColor Red
        exit 1
    }
}

Write-Host ""
Write-Host "Checking LocalDB instances..." -ForegroundColor Cyan

# List existing instances
$instances = & SqlLocalDB.exe i
Write-Host "Existing instances: $($instances -join ', ')" -ForegroundColor Gray

# Check if instance exists
$instanceExists = $instances -contains $InstanceName

if (-not $instanceExists) {
    Write-Host "Creating LocalDB instance: $InstanceName" -ForegroundColor Yellow
    & SqlLocalDB.exe create $InstanceName -s

    if ($LASTEXITCODE -eq 0) {
        Write-Host "Instance created and started successfully." -ForegroundColor Green
    } else {
        Write-Host "Error creating instance." -ForegroundColor Red
        exit 1
    }
} else {
    # Check instance status
    $instanceInfo = & SqlLocalDB.exe info $InstanceName

    # Start instance if not running
    Write-Host "Starting LocalDB instance: $InstanceName" -ForegroundColor Yellow
    & SqlLocalDB.exe start $InstanceName

    if ($LASTEXITCODE -eq 0) {
        Write-Host "Instance started successfully." -ForegroundColor Green
    } else {
        Write-Host "Instance may already be running or error occurred." -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "Getting instance information..." -ForegroundColor Cyan
& SqlLocalDB.exe info $InstanceName

Write-Host ""
Write-Host "=== Setup Complete ===" -ForegroundColor Green
Write-Host ""
Write-Host "Connection string:" -ForegroundColor Cyan
Write-Host "Server=(localdb)\$InstanceName;Integrated Security=true;Database=$DatabaseName;" -ForegroundColor White
Write-Host ""
Write-Host "To stop the instance, run:" -ForegroundColor Cyan
Write-Host "SqlLocalDB.exe stop $InstanceName" -ForegroundColor White
Write-Host ""
Write-Host "To delete the instance, run:" -ForegroundColor Cyan
Write-Host "SqlLocalDB.exe delete $InstanceName" -ForegroundColor White
Write-Host ""
