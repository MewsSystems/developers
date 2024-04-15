# ExchangeRateUpdater

ExchangeRateUpdater is a simple application that provides available exchange rates provided by the Czech National Bank [API](https://api.cnb.cz/).

## Prerequisites

Recommended:

- Visual Studio 2022 or Visual Studio Code
- Install the latest [.NET 8 SDK](https://github.com/dotnet/installer#installers-and-binaries)

## Quick Start

### Download Project

```
git clone https://github.com/vojtechrojicek/developers.git
```

### How to run Console application

-   Use the command `dotnet run` or use your IDE to run the `src/ExchangeRateUpdater.ConsoleApp` project.
-   The application provides exchange rates for the current date.

### How to run Web API

- Use the command `dotnet run` or use your IDE to run the `src/ExchangeRateUpdater.Api` project.
- The application is listening on `http://localhost:5142`.
- Swagger documentation is available at `.../swagger`.
- Endpoints are accessible at `.../api/v1/exchange-rates`.

## Unit tests

The solution contains a `ProductsWebApi.Test` project using xUnit.

- You can run tests, for example, in Visual Studio with  [TestExplorer](https://docs.microsoft.com/en-us/visualstudio/test/run-unit-tests-with-test-explorer?view=vs-2022).
- Or by using the `dotnet test` command in the `tests/ExchangeRateUpdater.Domain.Tests` project folder.

## Project structure

The application is implemented via Clean Architecture and ASP.NET Core. It is separated into multiple projects, each with its distinct goal.

- **ExchangeRateUpdater.Api**

Provides API endpoints responsible for request serialization, deserialization, etc.

- **ExchangeRateUpdater.Application**

Represents the Application layer and contains all business logic, implementing CQRS (Command Query Responsibility Segregation) with each business use case.

- **ExchangeRateUpdater.ConsoleApp**

Alternative interface for gathering exchange rates.

- **ExchangeRateUpdater.Domain**

Represents the Domain layer, containing domain logic and entities/value objects.

- **ExchangeRateUpdater.Infrastructure**

Represents the Infrastructure layer, containing classes for accessing external resources such as the Czech National Bank API.

- **ExchangeRateUpdater.DomainTests**

Domain unit tests.

## Technologies

### Api

-   [Swagger](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) is a tool used to document and visualize APIs.
-   [Swagger FluentValidation](https://github.com/micro-elements/MicroElements.Swashbuckle.FluentValidation) integrates FluentValidation with Swagger to automatically generate Swagger documentation based on validation rules defined in FluentValidation validators.
-   [ApiVersioning](https://github.com/dotnet/aspnet-api-versioning) is a library for versioning ASP.NET Core Web APIs. It allows developers to version their APIs to manage changes and compatibility with clients.

### Application

-   [AutoMapper](https://github.com/AutoMapper/AutoMapper) is a library used for object-to-object mapping. It simplifies the mapping process between different object types.
-   [MediatR](https://github.com/jbogard/MediatR) is a library that implements the mediator pattern in C#. It helps decouple components in an application by allowing them to communicate through requests and handlers, making the codebase more maintainable and testable.
-   [FluentValidation](https://github.com/FluentValidation/FluentValidation) is a validation library that provides a fluent interface for defining validation rules.

### Infrastructure

-   [Refit](https://github.com/reactiveui/refit) is a library that simplifies the consumption of RESTful APIs in .NET. It generates API clients from interface definitions.
-   [Polly](https://github.com/App-vNext/Polly) is a library for resilience and transient-fault-handling in .NET. It provides utilities for implementing policies such as retries.

### Tests

-   [xUnit](https://github.com/xunit/xunit) is a testing framework for .NET. It is used for writing and executing unit tests in .NET applications.
-   [NSubstitute](https://github.com/nsubstitute/NSubstitute) is a mocking library for .NET. It is used to create mock objects and stubs in unit tests.
-   [fluentassertions](https://github.com/fluentassertions/fluentassertions) is a library for fluent assertion syntax in .NET.
