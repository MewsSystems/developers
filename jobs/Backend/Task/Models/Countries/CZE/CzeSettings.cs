namespace ExchangeRateUpdater.Models.Countries.CZE;

public class CzeSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public int TtlInSeconds { get; set; } = 0;
    public string UpdateHourInLocalTime { get; set; } = "00:00:00";
}
