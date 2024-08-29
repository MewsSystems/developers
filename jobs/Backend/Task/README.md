# Exchange rate updater (.NET Backend test)

## Start the project

1. Clone the repository.
2. Locate your terminal in the root directory of the solution. (/jobs/Backend/Task)
3. Execute `docker-compose up`

## Usage

This project can be tested in different ways, such as:

- HTTP file: In `ExchangeRateUpdater.Api/Controllers/ExchangeRates` we can find the `ExchangeRatesController.http` file, that contains an example of the GetExchangeRates endpoint. It can be run in the IDE. In Rider, it can be run by clicking in the different green arrows found next to each endpoint.
- Curl: Example `curl -H 'Content-Type: application/json' -d '{"CurrencyCodes": ["EUR","USD"], "Date": "2024-08-25"}' -X POST http://localhost:5299/exchangerates`
- API clients, such as Postman.
