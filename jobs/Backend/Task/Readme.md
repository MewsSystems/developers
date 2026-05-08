# Quick Setup

Go to the [Currency API](https://app.currencyapi.com/api-keys) and get an API key. You will need to register for an account and get free calls.

- Enable User Secrets on the app project (from the project folder):
  - `cd ExchangeRateUpdater`
  - `dotnet user-secrets init`
  - `dotnet user-secrets set "CurrencyApi:ApiKey" "<your_api_key>"`

- Secrets/config shape (add via User Secrets):
  ```json
  {
    "CurrencyApi": {
      "ApiKey": "<your_api_key>",
      "BaseUrl": "https://api.currencyapi.com/v3/"
    },
    "Currencies":  ["USD", "GBP", "CZK"]
  }
  ```

- You can configure currencies: change `Currencies` for which you want to get the exchange rates against the base currency CZK


The App is a Hosted Service that runs once and outputs the exchange rates to the console.
