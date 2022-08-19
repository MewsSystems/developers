# Mews - Backend Coding Task

## Useful Commands

### Build the application
To build the application run the following commands
```
dotnet restore
dotnet build
```

### Run the tests
This solution contains unit and functional tests.
The unit tests are available in the folder `./test/unit` and have been written using NUnit, NSubstitute and FluentAssertions.
The functional tests are available in the folder `./test/functional` and have been written using SpecFlow and WireMock-Net.

To run the tests use the following command:
```
dotnet test
```

### Run the app
To run the app use the following command:
```
dotnet run --project .\src\ExchangeRateUpdater\ExchangeRateUpdater.csp
roj
```

### Author 
[Daniele De Francesco](https://github.com/danieledefrancesco)