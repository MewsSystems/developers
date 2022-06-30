namespace ExchangeRate.Provider.Base.Models;

public class CacheConfiguration
{
    #region Constructors

    public CacheConfiguration()
    {
        // For IOptions
    }

    public CacheConfiguration(bool? isEnabled, string? timeZoneId)
    {
        IsEnabled = isEnabled;
        TimeZoneId = timeZoneId;
    }

    #endregion

    #region Properties

    public bool? IsEnabled { get; set; }

    public string? TimeZoneId { get; set; }

    #endregion
}