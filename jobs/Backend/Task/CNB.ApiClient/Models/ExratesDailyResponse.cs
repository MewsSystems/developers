namespace CNB.ApiClient.Models;

public class ExratesDailyResponse
{
    public IEnumerable<ExrateApiModel> Rates { get; set; }
}

public class ExrateApiModel
{
    public DateTime ValidFor { get; set; }
    public int Order { get; set; }
    public string Country { get; set; }
    public string Currency { get; set; }
    public int Amount { get; set; }
    public string CurrencyCode { get; set; }
    public double Rate { get; set; }
}
