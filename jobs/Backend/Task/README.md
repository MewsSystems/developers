# Exchange Rates exercise

This is a .NET worker type of application that fetches the exchange rates from the Czech National Bank APIs and displays them for a subset of currencies.

## Running the application

You can run this application with dotnet or with Docker.

Running with dotnet:

```shell
dotnet run
```

Running with Docker:

```shell
docker build -f ./src/ExchangeRateUpdater/Dockerfile -t exchangerates .
docker run exchangerates
```
