# Hello reviewers

## Solution introduction
In the solution there are folders [host](host), [service](service) and [test](test).
In [host](host) folder you can find [ExchangeRateUpdater](host/ExchangeRateUpdater.csproj) project which contains [Program.cs](host/Program.cs) whose functionality is part of the task.
In [service](service) folder, there are folders containing two projects. Project [ExchangeRateProviderCzechNationalBank](service/ExchangeRateProvider/ExchangeRateProviderCzechNationalBank.csproj) is responsibly for obtaining, storing and returning exchange rates which Czech National Bank provides. Project [ExchangeRateProvider.Contracts](service/ExchangeRateProvider.Contracts/ExchangeRateProvider.Constracts.csproj) holds input and output classes so they can be easily referenced.
In [test](test) folder, you can find project [ExchangeRateProviderCzechNationalBank.Tests](test/ExchangeRateProviderCzechNationalBank.Tests/ExchangeRateProviderCzechNationalBank.Tests.csproj). This project contains unit tests of services in [ExchangeRateProviderCzechNationalBank](service/ExchangeRateProvider/ExchangeRateProviderCzechNationalBank.csproj)

## Main functionality
The task was to create a ExchangeRateProvider for Czech National Bank. The project ExchangeRateProviderCzechNationalBank does exactly that. 