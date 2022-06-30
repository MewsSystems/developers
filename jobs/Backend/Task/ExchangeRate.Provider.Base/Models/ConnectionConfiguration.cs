namespace ExchangeRate.Provider.Base.Models;

public class ConnectionConfiguration
{
    #region Constructors

    public ConnectionConfiguration()
    {
        // For IOptions
    }

    public ConnectionConfiguration(string? url)
    {
        Url = url;
    }

    #endregion

    #region Properties

    public string? Url { get; set; }

    #endregion
}