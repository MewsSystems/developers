# Exchange Rate Updater

## Overview

The Exchange Rate Updater is a .NET 8 console application that retrieves exchange rates from the Czech National Bank (CNB) API. The project is developed as a showcase for the Mews backend assigment task.

## External sources

To get the exchange rates, the application uses the [Czech National Bank (CNB) API](https://api.cnb.cz/cnbapi/swagger-ui.html).

## Project Structure

The solution is organized into the following projects:

- **ExchangeRateUpdater.Console**: The main console application for testing the implementation of the exchange rate provider.
- **ExchangeRateUpdater.API**: Provides a API for accessing exchange rates.
- **ExchangeRateUpdater.Core**: Contains the core services, models, and abstractions for the exchange rate provider.

## Getting Started

### Prerequisites

- .NET 8 SDK
- Visual Studio 2022 or later

### Setup

1. Clone the repository

2. Open the solution in Visual Studio.

3. Build the solution to restore the NuGet packages and compile the projects.

### Configuration

The application uses an `appsettings.json` file for configuration. Ensure that this file is present when running the project.

### Running the Console Application

1. Set `ExchangeRateUpdater.Console` as the startup project.
2. Run the application. The console will display the retrieved exchange rates.

### Running the API

1. Set `ExchangeRateUpdater.API` as the startup project.
2. Run the application. The API will be available at `https://localhost:7010/swagger` for testing and documentation.

## Usage

The main functionality is provided by the `CnbExchangeRateProvider` class, which implements the `IExchangeRateProvider` interface. The `GetExchangeRates` method retrieves exchange rates for a list of specified currencies.

## Testing

Unit tests are provided in the `ExchangeRateUpdater.Tests` project. The tests use `Moq` for mocking dependencies and `Xunit` for assertions.

### Running Tests

1. Open the Test Explorer in Visual Studio.
2. Run all tests to ensure that the application behaves as expected.
