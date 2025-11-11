namespace ConsoleTestApp.Models;

public class CurrencyData
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Symbol { get; set; }
    public int DecimalPlaces { get; set; }
    public bool IsActive { get; set; }
}

public class CurrenciesListData
{
    public List<CurrencyData> Currencies { get; set; } = new();
    public int TotalCount { get; set; }
}
