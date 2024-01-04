# Exchange Rate Updater

## Overview
This application fetches and displays the latest exchange rates from the Czech National Bank API for the requested currencies in the Program.cs file.

## Prerequisites
- To run this prject you will need the .NET 6.0 SDK or a new version.

## Running the Application
1. Clone the repository to your local machine.
2. Navigate to the project directory and build the project:
dotnet build
3. Run the application:
dotnet run --project ExchangeRateUpdater

## Running Tests
Execute the test suite with:
dotnet test

## Dependencies

The main application does not rely on external NuGet packages beyond what is included in the standard .NET SDK.

The `ExchangeRateUpdater.Tests` project relies on the following NuGet packages for unit testing:

- **MSTest.TestFramework**: The testing framework used to define and run unit tests.
- **MSTest.TestAdapter**: The test adapter that allows the test framework to work with the test runner.
- **Moq**: A library used for mocking objects in unit tests, allowing for isolated testing of components.

These packages are only required if you intend to run the unit tests. They are not necessary for the execution of the main application.

## Notes
- Ensure internet connectivity for the API calls to the Czech National Bank.
