namespace Domain.Configurations;

public class CNBConfig
{
    public string BaseURL { get; set; }
    public string ExchangeRateURL { get; set; }
    public int RefreshTimeHour { get; set; }
    public int RefreshTimeMinute { get; set; }
}
