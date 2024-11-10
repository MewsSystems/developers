# ExchangeRateUpdater

This project retrieves and displays exchange rates for specified currencies relative to the Czech Koruna (CZK) from the Czech National Bank (CNB) API.

## How to Build

### Prerequisites
- .NET 6 SDK: Make sure you have the .NET 6 SDK installed. You can download it [here](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).

To build the solution follow these steps:

1. Clone the Repository: Start by cloning the repository to your local machine.
2. Restore Dependencies: Go to the solution root folder and run the following command to restore all dependencies:
    ```bash
    dotnet restore
    ```
3. Build the Solution: Use the following command to build the project:
    ```bash
    dotnet build
    ```

## How to Run
You can run the ExchangeRateUpdater.Console application in two ways:

1. Using an IDE

    Open the solution in your preferred IDE (e.g., Visual Studio, Visual Studio Code, Rider) and run it.

2. Using Docker

    Ensure Docker is installed and running (you can install Docker from [here](https://docs.docker.com/get-started/get-docker/)), then use the commands below in the solution root folder:
   ```bash
    docker build -t rate-updater .
    docker run --rm rate-updater
   ```

## How to Test

You can run the tests using an IDE or via the command line, running the command:
```bash
dotnet test
```

The solution has two types of tests:
- Unit tests to test individual components / methods in isolation.
- Integration tests to test the entire application. To ensure reliable tests, the CNB API has been mocked using WireMock .NET, so to simulate different API behaviors (e.g., 200 OK, 500 Internal Server Error).

## How It Works
The application receives a list of currencies as arguments, and it fetches daily exchange rates from the CNB API (https://api.cnb.cz/cnbapi/exrates/daily) and outputs the rates for the specified valid currencies present in the CNB source.

### Design Decisions and Key Features

- **Clean Project Structure**
    
    The solution is structured in three layers: Console, Core and Infra. The Console layer acts as the application's entry point, it registers the dependencies needed and loads the configuration values.
    The Core layer contains the core domain logic, models, and interfaces. It represents the "business rules" of the application.
    The Infra layer provides implementations for external dependencies, such as the third-party provider API. 
    This structure allows to re-use Core and Infra with applications other than a Console, such as a web server.
- **Dependency Injection**

  In order to promote loose coupling and better testability, I used Dependency Injection (DI) in the application's entry point.

- **Resilient HTTP Client**

   The HttpClient is configured with a retry mechanism using exponential backoff for transient issues and a circuit breaker for faster failure response. The retry settings are hard-coded, but I would make them configurable in a production-ready scenario.
- **Result Pattern**

    To manage HTTP responses, I implemented the Result Pattern. This pattern helps handle success and failure scenarios clearly, by wrapping the HTTP response into a custom Result object, which encapsulates the data and any error information.

## Future Improvements
- Caching: As exchange rates change daily, adding a caching layer (e.g., in-memory or Redis) could reduce unnecessary API calls and improve performance.
- Better error handling: Error handling can be improved by returning more appropriate messages to the user at various stages of the application.
- Metrics and observability: Implement structured logging to capture detailed logs for all critical actions. Set up monitoring for key metrics (e.g., API call duration, error rates, number of retries).
- Enhanced testing, such as greater unit test coverage and more extensive E2E test scenarios.
- Setup CI/CD pipelines: Automate the build, test, and deployment processes using a CI/CD pipeline.  Set up automated tests, code quality checks, and vulnerability scans as part of the CI process.
