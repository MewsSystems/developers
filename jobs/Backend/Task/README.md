# Usage

Run the project
`dotnet run --project ExchangeRateUpdater/ExchangeRateUpdater.csproj`

Run all the tests
`dotnet test`

# Notes

behaviours
- inputs a list of currencies and returns a list of exchange rates for each pair in the list
- ignores unknown currency codes
- ignores any pair that doesn't have a exchange rate from the source (CNB API)
- ignores duplicated inputted currencies

example output
source/target=x
1 source = x amount of target
USD/CZK=23.354
for every 1 usd we get 23.354 czk

## API

- More info here https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/
- API returns rates for x currency to Czech Koruna
- API docs https://api.cnb.cz/cnbapi/swagger-ui.html#//exrates
  - `/cnbapi/exrates/daily` for daily rates for most common currencies
  - `/cnbapi/fxrates/daily-month` for monthly rates of other currencies not in the above
- CNB API can return rates in different amounts so will need to convert to 1 (example below)

`/cnbapi/exrates/daily` example response
```json
{
  "rates": [
    {
      "validFor": "2024-04-10",
      "order": 70,
      "country": "Australia",
      "currency": "dollar",
      "amount": 1,
      "currencyCode": "AUD",
      "rate": 15.465
    },
    {
      "validFor": "2024-04-10",
      "order": 70,
      "country": "Turkey",
      "currency": "lira",
      "amount": 100,
      "currencyCode": "TRY",
      "rate": 72.398
    }
  ]
}
``` 
This is saying 
- for every 1 AUD we get 15.465 CZK 
- for every 100 TRY we get 72.398 CZK
