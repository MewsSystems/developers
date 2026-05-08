# ExchangeRateUpdater

This service retrieves daily exchange rates from the Czech National Bank (CNB), parses the XML feed, and maps the data into a strongly-typed model.

## 🔧 Technologies Used

- .NET 7 (or compatible)
- xUnit for unit testing
- Moq for mocking dependencies
- Microsoft.Extensions.DependencyInjection for DI
- Microsoft.Extensions.Logging for structured logging

## 🚀 Project Structure

```
Task/
├── ExchangeRateUpdater.csproj      # Main service
├── Interfaces/                     # Service contracts
├── Models/                         # Data models (Currency, ExchangeRate, etc.)
├── Services/                       # Business logic (CNB parser, provider)
└── Startup.cs                      # Dependency injection setup

Tests/
└── ExchangeRateUpdater.Tests/
    └── Services/
        ├── CnbXmlParserTests.cs
        └── ExchangeRateProviderTests.cs
```

## 📥 Data Source

The exchange rates are retrieved from CNB's official XML feed.

Example:
```xml
<radek kod="USD" mnozstvi="1" kurz="24,50" />
```

Parsing is done using `CultureInfo("cs-CZ")` to correctly interpret Czech decimal formats (comma as separator).

## ✅ Key Features

- Parses CNB daily XML using `XDocument`
- Maps currency rates to `ExchangeRate` objects
- Fixed source currency: CZK
- Fully tested using xUnit
- Structured logging via `ILogger`
- Dependency Injection setup via `Startup.cs`

## 🧪 How to Run Tests

```bash
cd ../Tests/ExchangeRateUpdater.Tests
dotnet test
```

## 📝 Notes

- `TryParse` is not used: parsing failures are intentional signals of feed format issues.
- Only explicitly provided currencies are returned.
- Consider adding a caching layer in production scenarios (e.g., in-memory TTL).

---

## 📦 Possible Improvements

- Caching layer with expiration
- Retry policy for HTTP via Polly
- Add CI pipeline to run tests

---

## 🔗 License
