# Mews.ExchangeRateUpdater

Environment | Status
------------ | -------------
Production | [<img src="https://vsrm.dev.azure.com/ctaima/_apis/public/Release/badge/f5858e83-99c5-43f1-a568-fd09db7cfcf1/225/764"/>](https://dev.azure.com/ctaima/Ctaima/_release?_a=releases&view=mine&definitionId=225) 

Mews.ExchangeRateUpdater is a solution designed to fetch and provide the current day’s currency exchange rates, particularly from the Czech National Bank (CNB). The application is structured following the Clean Architecture principles and is encapsulated within six different projects, ensuring a clear separation of concerns and facilitating easier maintenance and scalability.

## Projects Overview

1. **Mews.ExchangeRateUpdater.Api**: The API layer of the application, responsible for exposing HTTP endpoints to fetch exchange rates.
2. **Mews.ExchangeRateUpdater.Application**: This project contains the application's core logic and services, orchestrating the flow of data between the API/ConsoleApp and Domain layers.
3. **Mews.ExchangeRateUpdater.ConsoleApp**: A console application that serves as an alternative interface for users to interact with and fetch exchange rates.
4. **Mews.ExchangeRateUpdater.Domain**: The domain layer, which holds the entities of the application.
5. **Mews.ExchangeRateUpdater.Infrastructure**: This project is responsible for data access and external API integrations, including the interaction with the Czech National Bank API.
6. **Mews.ExchangeRateUpdater.UnitTests**: A collection of unit tests ensuring the application’s reliability and correctness.

## Key Features

- **Clean Architecture**: The application adheres to the Clean Architecture principles, promoting independence from the UI, external frameworks, and databases. This separation enhances maintainability and allows for easier unit testing.

- **.NET 7**: The solution is upgraded to .NET 7, ensuring it utilizes the latest and most efficient features, performance improvements, and security enhancements offered by the .NET platform.

- **In-Memory Caching**: To optimize performance and provide resilience during potential downtimes of the CNB API, the application implements in-memory caching. This ensures that exchange rate data, once fetched for the day, is stored and readily available without needing to make additional API calls.

- **Retry Policy with Exponential Backoff**: The integration with the CNB API implements a retry policy using the Polly library. This approach adds robustness to the application, gracefully handling transient errors and short downtimes of the external API.

- **Repository Pattern**: Data access and interactions with the CNB API are abstracted behind repository interfaces, adhering to the Repository Pattern. This design choice simplifies unit testing and provides flexibility to change data sources without impacting the application logic.

- **Docker Support**: The application includes a Dockerfile, allowing for containerization, which simplifies deployments and ensures consistency across different environments.

- **Continuous Integration and Deployment (CI/CD)**: The `azure-pipelines.yml` file in the repository configures Azure DevOps pipelines for continuous integration and building the application. Additionally, Continuous Deployment is set up to automatically deploy the API to the desired environment upon successful builds.

- **Infrastructure as Code (IaC)**: The solution includes Terraform files, enabling the definition and provision of required infrastructure directly from code. This practice ensures that the infrastructure is versioned, easily reproducible, and consistent across different stages of development.

- **Options Pattern for Configuration**: The application leverages the Options Pattern for handling configurations, providing a strongly-typed way to access settings in the application.

## Getting Started

### Prerequisites

- .NET 7 SDK
- Docker (Optional, for containerization)
- Terraform (For IaC)

### Running the Application

#### API

1. Navigate to the Mews.ExchangeRateUpdater.Api project directory.
2. Run the command `dotnet run`.
3. The API will be available at `https://localhost:5001`.

#### Console App

1. Navigate to the Mews.ExchangeRateUpdater.ConsoleApp project directory.
2. Run the command `dotnet run`.
3. It will display the exchange rates on-screen.

### Testing

Navigate to the Mews.ExchangeRateUpdater.UnitTests project directory and run the command `dotnet test` to execute the unit tests.

### CI/CD and Infrastructure

Follow the instructions in Azure DevOps to set up the CI/CD pipelines and deploy the infrastructure using the provided `azure-pipelines.yml` and Terraform files.

## Configuration

Utilize the appsettings.json file or environment variables to configure the application settings. Ensure to set the correct API endpoint for the CNB API and other necessary configurations.