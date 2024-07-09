# Exchange Rate Updater

This is a simple command line based solution for pulling the most current central bank exchange rate fixing data pulled from the Czech National Bank website. The application is currently hardcoded to pull all CZK exchange value against the following:
|Currency|
| ------ |
| USD |
| EUR |
| JPY |
| KES |
| RUB |
| THB |
| TRY |
| XYZ |

## Application Run Details
1. Navigate to `ExchangeRateUpdater` project directory
2. Set your environment variable in terminal to the following 

| Variable Name    | Value |
| -------- | ------- |
| `CNB_EXCHANGE_RATE_URL`  | `https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt`    |

3. run `dotnet build`
4. run `dotnet run`

## Running Tests
1. Navigate to `ExchangeRateUpdater.Tests` directory
2. run `dotnet test`

#### Code Coverage Report
3. run `dotnet test --settings coverlet.runsettings /p:CollectCoverage=true`

