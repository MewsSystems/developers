# Owen Gannon's Technical Test

Thanks for allowing me to take part in the Mews technical test. It was an interesting challenge and fun to develop. It's by no means complete as a definition of done would need more information about the users and the hosting environment, but I think it's setup pretty nicely for any small modifications needed, for example to Dockerize it or change the presentation layer to a WebAPI.

The majority of this project was done after my current work hours (5pm UK/6pm CZ), so I didn't want to post too much into Slack. 

---

## ▶️ How to run the application

Simply open it Visual Studio, build it, select your launch profile and hit Run. After a second or two, you should see a list of exchange rates. If you have run it with the development profile, you should see some info and debug logs.

---

## 📜 About the application

The application is a simple console application that is architected with an n-tier style, which would allow it to be quickly adapted into a WebAPI, a worker service or integrated into an MVC app.

I've tried to stick to Microsoft basics and best practices when it comes to logging, configuration and dependency injection, as well as following DRY and SOLID principles and trying not to overcomplicate it.

I have added the default host builder with Services configured as singletons and since this is designed to be built and run once before exiting, the service lifecycle isn't too consequential.

The tiers are quite simple and each have a single purpose:
- console app client just calls the exchange rate provider and outputs the response
- the exchange rate provider calls the foreign exchange service and converts the response into more readable types
- the foreign exchange service calls the CNB API and deserializes the response

The app is designed to be modified using appsettings instead of code changes, so in `appsettings.{environment}.json`, you'll find settings for the data source API. This paid off when I realised the text API was legacy and I updated the code to use the modern JSON API.

### Data Source

I've used the Czech National Bank's RESTful [API which can be found here](https://api.cnb.cz/cnbapi/swagger-ui.html). I originally built the Foreign Exchange Service to handle the legacy plain text API, [found here](https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt), but discovered later that a JSON endpoint exists. It's probably lower on my Google because I'm in the UK and it's a Czech website! 😅

I use the daily endpoint in this app and while I'm aware of the monthly endpoint, I chose to exclude it for simplicity.

### Unit tests

Each class has unit tests and these can be found in a separate project. I've created this as separate project to allow for GitHub/TeamCity to execute tests on commit/build depending on a potential real world scenario.

---

## ❔ Assumptions / Non-assumptions

In a real world scenario I would clarify things with my team or the PM, but since this is a small test of my thought process and code skill, I've made a number of assumptions for this app:
- Since we're meant to use the Czech National Bank's exchange rates, I'm assuming the users would be Czech-based or would have Czech as their first language
- This is a run once and exit app, so I didn't add any in-memory or external caching
- I chose not to Dockerize the app. Without knowing how this would be implemented or hosted in an environment, I opted to leave it out. It would be fairly straightforward to adapt this to run with Docker though.

---

## 🔧 Future improvements

Depending on how this app would be required to evolve, there is a few things I would add to it depending on the scenario:

- Caching with an expiry date if used in a WebAPI or web app - so the Exchange Rate Service is not making unncessary requests to the CNB API since it's only updated once a day, after 2.30pm.
- Polly for fault tolerant requests to the CNB API
- Dockerize the app and use Docker Compose for development to minic a database or cache

---

## 📚 References

Here's the list of resources I used to develop the app:
1. CNB's [legacy API](https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt) and the [modern API](https://api.cnb.cz/cnbapi/swagger-ui.html) 
2. [Setting up a Generic Host (also describes how configuration is loaded into the app)](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-hos)
3. [Injecting a typed client into the Foreign Exchange Service for the CNB API](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#how-to-use-typed-clients-with-ihttpclientfactory)
4. [Why I thought really hard about the decision to add in Docker support](https://developers.mews.com/distributed-systems-docker-and-developer-experience/) and add in a [separate app to call the CNB API](https://developers.mews.com/synchronous-communication-with-grpc/) before deciding that was probably overcomplicating a project meant to take a few hours
5. Why I return null instead of throwing exceptions - [they're expensive and not needed for common scenarios](https://learn.microsoft.com/en-us/dotnet/standard/exceptions/best-practices-for-exceptions#handle-common-conditions-without-throwing-exceptions)