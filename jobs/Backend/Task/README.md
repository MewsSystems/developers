# ExchangeRateUpdater

## Running locally
You can run the ExchangeRateUpdater
	1. using an IDE of your choice (Visual Studio, Visual Studio Code, Rider)
	2. or if you want to use the provided Docker file you can use these commands in the folder with the dockerfile :
	`docker build -t exchangerateupdater .`
	`docker run --rm exchangerateupdater`
	You need to have Docker installed and running for this to work

## How it works:
The list of currencies we are requesting exchange rates to be retrieved for is in `appsettings.json` file and it is loaded from there at runtime and injected into the AppService class. 
We are retrieving the exchange rates for the day from the CNB api endpoint: `https://api.cnb.cz/cnbapi/exrates/daily`. Then we display the rates for all the valid currency codes requested that were present in the CNB source.

# Notes
Regarding the structure of the solution: I usually use Clean Architecture model for my solutions but since this is a very simple console app I chose to structure my project using basic folders named suggestively.

Keeping in mind that this was supposed to be implemented as a service that should be production ready, I chose to take advantage of the .NET Core 6 (I have not updated the project to the latest version) DI mechanism
so I moved the already provided code from the Program.cs class into the AppService class which is now the entry point for the app and it is registered and starts as an IHostedService.

For retrieving the exchange rates I used the CNB api endpoint: `https://api.cnb.cz/cnbapi/exrates/daily` as it seemed most appropriate. I configured the http client to have a retry mechanism with exponential backoff for 
transient issues and the number of retries can be configured in the appsettings. When implementing this, since the CNB documentation for their APIs doesn't mention this, I tested to see if the endpoint has some form of rate limiting.
I hit the endpoint a few thousand times in 2 minutes and I was not limited. Therefore I did not implement a circuit breaker for this.

Since this solution is for providing the exchange rates between different currencies and CZK I decided to keep this simple and not overengineer it at this time but should we want to extend this to support other conversions
we can make the ExchangeRateMapper and the ExchangeRateProvider more generic to accept the target currencies dinamically and make these passable from appsettings as well. For now I tried to follow the KISS and YAGNI principles 
and keep the solution simpler.

Things that can be improved if this service was to run in real world:
1. Since the exchange rates are not changing very often we can implement a caching mechanism to store the rates and improve performance. The number of rates returned by the CNB api is relatively small so we could 
even use an in memory cache and if the number increases we can use something else like Redis
2. Add some metrics for better observability (for example the calls duration to the external APIs etc)
3. Have the strings representing the log messages centralised in a static class so we would change them in one place if needed but for now I wanted to keep things simple.
4. Better unit test coverage, integration testing if there are other services in the pipeline, mutation testing, end-to-end testing.
