# ExchangeRateUpdater

ExchangeRateUpdater is an exercise solution that allows interacting with Czech National Bank (CNB) API to retrieve currencies exchange rates.

## Projects

### ExchangeRateUpdater.Api
The API layer of the application. It exposes HTTP endpoints to fetch exchange rates.

This is actually not needed to solve the exercise, but it serves as example of an API that can be consumed by other actors/services in the system.

### ExchangeRateUpdater.ApplicationServices
This project contains the Service that intermediates between domain layers and exposed API.
It contains the specification of `IExchangeRateRepository`, the inferface for the repository pattern, that allows the possibility to change data sources easily.

### ExchangeRateUpdater.Console
A console application used to showcase the exercise.

### ExchangeRateUpdater.Domain
This project contains the data model (domain entities) of the application.

### ExchangeRateUpdater.Infrastructure
This project is responsible for data access and external API integrations, including the interaction with the Czech National Bank API.
It contains the implementation of the Repository pattern interface.

### ExchangeRateUpdater.UnitTests
A small set of example unit tests that ensure correctness of the application.

### ExchangeRateUpdater.AcceptanceTests
This project defines a basic acceptance test that specifies the main scenario using Gherkin language and runs a end-to-end test with real instances.

It runs an instance of `ExchangeRateUpdater.Api`, retrieves today's exchange rates and also runs a query against CNB Api and ensures the results are the same.

Limitations: This only works on Windows in this version for demonstration purposes (the example use Windows operating system specific helpers to run processes).

## Future improvements

- **Documentation**: Only main classes and interfaces have been documented for demonstration purposes. In a production environment this should be revisited for completeness and correctness.

- **Logging & performance tracing**: It is essential in enterprise solutions to be able to monitor and measure applications performance and possible errors.
    - Examples: Serilog, NLog, etc. for logging. NewRelic, Application Insights, etc... for metrics.

- **Cache**: It usually makes sense to implement a cache for the data in the Repository layer (in this case in `CnbExchangeRateRepository`). 
    - This can be achieved using different persistence strategies, like in-Memory caching, or distributed caching (Redis), for example.

- **Retry policy**: It's very common to include a retry policy to exposed APIs.

- **Docker, CI/CD, IaC...**: This has not been included in this exercise but it would be necessary in a modern production environment.


## Running the Application

The application can be run through the Visual Studio IDE or the command line.
If you just run it from the Visual Studio IDE, the Console application is configured as startup project, and it will showcase the exchange rates exercise.

### API
- Run the command `dotnet run` in the `ExchangeRateUpdater.Api` project directory to run the API.
    - The API will be available at [https://localhost:5129](http://localhost:5129).
    - Access [http://localhost:5129/swagger/](http://localhost:5129/swagger/) to see the published `ExchangeRates` endpoint in Swagger.

### Console Application
- Running `dotnet run` into the `ExchangeRateUpdater.Console` folder will display the requested exchange rates.

### Testing
- Running `dotnet test` in any tests directory will execute the application tests.

### Configuration

`appsettings.json` file or environment variables are used to configure the application settings.
In this project, the only setting is the CNB API URL, which is already configured in `appsettings.json` file.