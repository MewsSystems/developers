# Exchange Rate Updater Readme

## Description

This console application provides today's exchange rates for a given set of currencies. Once running, the application will persist until the user closes it.

Rates can be refreshed by pressing any key, and failed requests can be resent by pressing any key. The application will advise if the displayed rates are from the API or are cached.

## Requirements

- Ensure the .NET 8 SDK is installed

## Instructions

To run the app, either open the solution in Visual Studio and execute by pressing F5.

Alternatively, navigate to the folder containing the application and execute:

```sh
dotnet build
```

```sh
dotnet run
```

To run the unit tests:

```sh
dotnet test
```