# Description
This library implements an `ExchangeRateProvider` that can load currency rates from various sources. Currently, it only supports [Czech National Bank] (https://www.cnb.cz/) .

# Usage
Simply include the following code into the `Program.cs` of your application:
```csharp
builder.Services.AddExchangeRateProvider();
```

To include Czech National Bank source, add:
```csharp
builder.Services.WithCzechNationalBankRateSource();
```

Then, inject the `ExchangeRateProvider` service into your application.

# Enchancements
The library provides an interface `IExchangeRateCache`. If you want to add caching to rate sources, you may implement it and register an implementation. By default, no caching is done.

Additionaly, you can implement your own `IRateSource` and register it like that:
```csharp
builder.Services.AddSingleton<IRateSource, MyRateSource>();
```

After that, `ExchangeRateProvider` will pick it up automatically.
