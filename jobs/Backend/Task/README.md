# Exchange Rate Updater
This solution is part of the [Mews backend developer task](https://github.com/MewsSystems/developers/blob/master/jobs/Backend/DotNet.md)..

## Key Features
It allows to extract for the current date the exchange rates data from the [Czech National Bank public Api](https://api.cnb.cz/cnbapi), and filter it by specific currencies.

## Technology stack
1. Base: .NET 6, C# 10
2. Testing: xUnit, FluentAssertions
3. Mocking: Moq
4. Other utilities: Automapper

## Notes
1. Improved the solution considering: clean code, TDD, and followed SOLID principles.
2. Implemented clean architecture.

## Build and Run
To build and run the sample, type the following command:

`dotnet run`

To run the configured tests:

`dotnet test`

## Further Improvements
- Create integration and functional tests
- HttpClient retry policies
- Add caching
- Implement I18N
- Implement REST Api depending on business requirements to replace the console app
- Implement CQRS
- Implement Logging provider (other than the console one)