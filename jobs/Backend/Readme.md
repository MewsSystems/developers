# Exchange rate app

## Running the app
In order to run the app all You need is a .NET 6 SDK and run the application in any supported IDE (Visual Studio 2022, Rider, VS Code etc.)

You can also run the app using the .NET CLI in `/src/WebApi`:

```
dotnet run 
```

## Data source

CNB does not expose a public API

For the purpose of this project I was able to find an [XML file on CNB website](https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.xml)


## Solution structure

The solution follows a tree folder structure that corresponds with the namespaces of specific project files.

This means that every `.csproj` has a specified `RootNamespace` in order to create a default namespace.

_Example:_

```cs
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    ...
    <RootNamespace>ExchangeRate.Infrastructure.$(AssemblyName)</RootNamespace>
  </PropertyGroup>

</Project>

```

The project is ready to be scaled for other clients, not just CNB.

## Solution Projects

The project consists of these projects:

### WebApi

A .NET Web API project exposing an API to fetch the data from the provided clients.

### Logging

An assembly setting up a logging configuration in order to use Serilog within the application logging.

The entire point of this project is to separate the logging logic from the app itself, therefore the configuration is easier and the projects is potentially reusable in other solutions (or for more APIs within the same solution),

### Domain

Holds domain objects provided by Mews. The app could definitely be changed completely and still fullfill the expected results, but it felt easier to keep at least these two objects in order to demonstrate the understanding of the domain.

### Infrastracture

Infrastracture is separated into multiple projects. Their prefix is designed to hold the logic and data specific to a client that provides the exchange rate. This app uses CNB as the only data source at the moment, but this way it will be easier to manage multiple providers, like for example an API from kurzy.cz or something like that.

### Unit tests

Pretty much self-explanatory. Tests different part of the systems


## Testing strategy

Currently there is no clear testing strategy. For demonstration purposes I decided to just the entire domain (even tho its logic is super simple) and one repository test just to show the understanding of the Mocking library.

## Error handling

The Web API utilizes a middleware that catches possible exceptions within the entire web API project.