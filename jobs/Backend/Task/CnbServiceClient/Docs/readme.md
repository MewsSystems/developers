# Cnb Service Client

This is the Cnb Service Client library. Created to connect to the CNB api.

## Configuration

In order to use this library, you have to register the library:

```csharp
private static ServiceProvider CreateServiceProvider()
{
    ...
    serviceCollection.AddCnbServiceClient(configuration);
    ...
}
```

This settings should be added to the configuration of the project:

```json
"ServiceClient": {
    "Url": "https://api.cnb.cz/"
}
```

- `ServiceClient:Url`: The URL of the Cnb API.

## Usage

In order to use the library, you just have to inject the desired service and call the methods.

Ex:

```csharp

public class MainController
{
    private readonly IExratesService _exrateService;

    public MainController(IExratesService exrateService)
    {
        _exrateService = exrateService;
    }

    public async Task GetExratesDailyAsync()
    {
        await _exrateService.GetExratesDailyAsync();
    }
}

```