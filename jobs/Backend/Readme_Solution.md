# Solution Overview

This project implements an exchange rate provider for the Czech National Bank (CNB).
The provider downloads the daily `daily.txt` file from the official CNB endpoint, parses it, and maps the data to the domain `ExchangeRate` model.

## How it works

1. The provider downloads the TXT file using `HttpClient` (async internally).
2. The file is parsed into an internal `CnbRateRow` model (`Amount`, `Code`, `Rate`).
3. Only currencies requested by the caller and available in the CNB data source are returned.
4. CNB publishes rates in the format "foreign currency - CZK". The provider converts the CNB rate to “per one unit” using: value = Rate / Amount  
5. The provider does not create synthetic or reverse exchange rates.

## Design Notes

- The public API remains synchronous to match the original task skeleton. Internal download logic uses async for correctness.
- The `CnbRateRow` model is kept private because it represents a CNB-specific TXT structure and is not part of the domain model.
- Duplicate currency codes in the CNB data are validated to ensure data quality.
- The target currency is always CZK.
- Minimal debug logging is implemented directly inside the provider. This keeps the code simple for a small console application.
  In a real production service, structured logging (ILogger) would be used.
  
## Running

dotnet build
dotnet run


