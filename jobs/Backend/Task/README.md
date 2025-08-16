# ExchangeRateUpdater

This is an app that accepts in an array of CurrencyCodes that adhere to the ISO 4217 standard and an optional ForDate.
A list of the exchange rate for each currency in relation to the Czech Koruna is returned after being obtained via the Czech National Bank api.

## How to run the application

You can run the application via docker or the dotnet run command.

## To use docker:

Open you terminal in the Task folder (project root), then run:

	docker build -t exchange-rate-updater-gt .
	
	docker run -d --name exchange-rate-updater-gt -p 8080:8080 exchange-rate-updater-gt
 
## To use the dotnet run cmd

Open your terminal in the Task\ExchangeRateUpdater.Api folder then run:

	dotnet run
 
Once the application or container is running, you can make requests to the ExchangeRateUpdate endpoint by sending a request to:
 http://localhost:8080/exchangerates
You will need to set a header value of "X-Api-Key" and the corresponding key value in the appsettings.Development.json file
Send a POST request with the following example payload:

	{
		"currencyCodes" : ["EUR","GBP", "USD"],
		"forDate": "2024-05-12T14:30:23.793Z"
	}

## To run the ApiTests

Open your terminal in the Task\ExchangeRateUpdater.ApiTests folder then run:

	dotnet test

## Future improvements to the application

I've currently implemented a global Exception Handler, which returns a 500 response for every error. I'd expand this out and add a Handler for BadRequest errors at the very least.

I'd make the endpoint more restful. As I couldn't be sure of the length of the currencyCodes array, I couldn't use a query param, so needed a request body and so have used a POST request as a GET.

Dependent on the use case of this application, I could look at caching the response as the exchange rates only change daily. As you can return past dated exchange rates, I haven't done that.