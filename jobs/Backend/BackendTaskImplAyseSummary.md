### Summary
Solution provides exchange rates fetching from resource CNB

> Note: `https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt` endpoint has been used for retrieving exchange rates from czech national bank

## About Project Architecture
Project has been developed based on n-tier architecture includes following Class Libraries, WebAPIs,Tests
- Business
- DataAccess
- Entities
- Common
- Tests
- WebAPI

> Note: Basic authentication with ApiKey has been applied. ApiKey is included in appsettings.json file under WebAPI layer

Recommended improvements
- Logging
- Caching
- Rate Limitter
- Integration testing
- Dockerizing