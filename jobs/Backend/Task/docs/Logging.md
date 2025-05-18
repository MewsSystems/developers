# Logging in Exchange Rate Updater

This document describes the logging setup for the Exchange Rate Updater application.

## Overview

The Exchange Rate Updater uses Serilog for structured logging with different configurations for development and production environments:

- **Development**: Human-readable console output with verbose debugging information
- **Production**: JSON-formatted logs for easier machine parsing and integration with log management systems

## Configuration

Logging configuration is stored in:

- `appsettings.json` - Base configuration
- `appsettings.Development.json` - Development-specific overrides

## Log Levels

| Environment | Default Level | Microsoft/System Level |
|-------------|---------------|------------------------|
| Development | Debug         | Information            |
| Production  | Information   | Warning                |

## Log Outputs

The following log outputs are configured:

1. **Console**
    - Development: Human-readable format with timestamp, level, and message
    - Production: Compact JSON format

2. **File**
    - Path: `logs/exchange-rate-updater-.log` (with date suffix)
    - Format: Text in development, JSON in production
    - Rolling: Daily

## Enrichment

All logs are enriched with:

- Environment name
- Machine name
- Application name
- Correlation IDs (for HTTP requests)

## Usage Examples

### Basic logging in components

```csharp
public class SomeService
{
    private readonly ILogger<SomeService> _logger;

    public SomeService(ILogger<SomeService> logger)
    {
        _logger = logger;
    }

    public void DoSomething()
    {
        _logger.LogInformation("Operation started");
        
        try
        {
            // Operation code
            _logger.LogInformation("Operation completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during operation");
            throw;
        }
    }
}
```

### Structured logging

```csharp
_logger.LogInformation("Processing exchange rate request for {CurrencyPair} on {Date}", 
    currencyPair, 
    date);
```

## Viewing Logs

- **Development**: Check the console output or log files
- **Production**: Use a log management tool compatible with JSON formatted logs (e.g., Elasticsearch, Splunk, etc.) 