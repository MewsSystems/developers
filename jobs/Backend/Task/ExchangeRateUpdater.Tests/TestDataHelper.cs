namespace ExchangeRateUpdater.Tests;

public static class TestDataHelper
{
    public static string LoadTestData(string fileName)
    {
        var basePath = Path.Combine(AppContext.BaseDirectory, "TestData");
        var filePath = Path.Combine(basePath, fileName);

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Test data file not found: {filePath}");

        return File.ReadAllText(filePath);
    }
}