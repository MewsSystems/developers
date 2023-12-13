namespace ExchangeRateUpdater.CnbApi
{
    public class CnbApiOptions
    {
        /// <summary>
        /// URL of daily exchange rates endpoint.
        /// </summary>
        public string ApiUrl { get; set; } = "https://api.cnb.cz/cnbapi/exrates/daily";
        
        /// <summary>
        /// UTC time, when Exchangerates are released. 
        /// </summary>
        public TimeSpan DailyReleaseTime { get; set; } = new TimeSpan(13, 30, 0);

    }
}