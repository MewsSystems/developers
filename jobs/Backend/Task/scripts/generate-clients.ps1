# Store the initial directory (where the script was executed)
$initialDirectory = Get-Location

# Move up one level to reach the solution root (assuming script is in /scripts)
Set-Location ..

# Get the new solution root path
$solutionRoot = Get-Location
$swaggerJsonPath = "$solutionRoot\scripts\swagger\swagger.json"
$outputClientFile = "$solutionRoot\ExchangeRateUpdater.Client\Generated\ExchangeRateApiClient.cs"

try {
    # Ensure NSwag CLI is installed
    if (-not (Get-Command "nswag" -ErrorAction SilentlyContinue)) {
        Write-Host "Installing NSwag CLI..."
        dotnet tool install --global NSwag.ConsoleCore
    }

    # Restore .NET tools
    Write-Host "Restoring .NET tools..."
    dotnet tool restore

    # Verify if swagger.json exists
    if (-Not (Test-Path "$swaggerJsonPath")) {
        Write-Host "Error: swagger.json not found in scripts/swagger. Please ensure it exists."
        exit 1
    }

    # Generate API Client using NSwag
    Write-Host "Generating NSwag Client from $swaggerJsonPath..."
    nswag openapi2csclient /input:$swaggerJsonPath /output:$outputClientFile /classname:ExchangeRateApiClient /namespace:ExchangeRateUpdater.Client

    # Verify if client file was created
    if (-Not (Test-Path "$outputClientFile")) {
        Write-Host "Error: Failed to generate NSwag client!"
        exit 1
    }

    Write-Host "Successfully generated ExchangeRateApiClient.cs"
}
finally {
    # Navigate back to the original scripts folder
    Set-Location $initialDirectory
}
