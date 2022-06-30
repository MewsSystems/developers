using System.Text.RegularExpressions;
using Common.Exceptions;
using ExchangeRate.Models;
using ExchangeRate.Models.Utils;
using ExchangeRate.Provider.Base.Interfaces;
using ExchangeRate.Provider.Base.Models;

namespace ExchangeRate.Provider.Cnb.Models.Configuration;

public class CnbProviderConfiguration : IValidatable
{
    #region Properties

    public CacheConfiguration? Cache { get; set; }
    public ConnectionConfiguration? Connection { get; set; }

    public List<string>? Currencies { get; set; }
    public CnbDataSourceFieldsConfiguration? DataSourceFields { get; set; }

    #endregion

    public void Validate()
    {
        if (Connection is null || string.IsNullOrWhiteSpace(Connection.Url))
            throw new ConfigurationException($"{nameof(CnbProviderConfiguration)} - {nameof(Connection)} - {nameof(Connection.Url)} is not set");

        if (DataSourceFields is null)
            throw new ConfigurationException($"{nameof(CnbProviderConfiguration)} - {nameof(DataSourceFields)} is not set");

        if (string.IsNullOrWhiteSpace(DataSourceFields.Amount) ||
            string.IsNullOrWhiteSpace(DataSourceFields.Currency) ||
            string.IsNullOrWhiteSpace(DataSourceFields.Rate) ||
            string.IsNullOrWhiteSpace(DataSourceFields.Code) ||
            string.IsNullOrWhiteSpace(DataSourceFields.Country))
            throw new ConfigurationException($"{nameof(CnbProviderConfiguration)} - one of " +
                                             $"{nameof(DataSourceFields.Amount)}, " +
                                             $"{nameof(DataSourceFields.Currency)}, " +
                                             $"{nameof(DataSourceFields.Country)}, " +
                                             $"{nameof(DataSourceFields.Code)}, " +
                                             $"{nameof(DataSourceFields.Rate)} is not set");

        if (string.IsNullOrWhiteSpace(DataSourceFields.Separator))
            throw new ConfigurationException($"{nameof(CnbProviderConfiguration)} - {nameof(DataSourceFields.Separator)} is not set");

        if (Cache?.IsEnabled is null)
            throw new ConfigurationException($"{nameof(CnbProviderConfiguration)} - {nameof(Cache)} - {nameof(Cache.IsEnabled)} is not set");

        if (Cache.IsEnabled is not null && Cache.TimeZoneId is null)
            throw new ConfigurationException($"{nameof(CnbProviderConfiguration)} - {nameof(Cache)} - {nameof(Cache.TimeZoneId)} is not set");

        if (Currencies is null || Currencies?.Count == 0)
            throw new ConfigurationException($"{nameof(CnbProviderConfiguration)} - {nameof(Currencies)} is not set or have zero records");

        if (Currencies?.All(x => Regex.Match(x, CurrencyUtils.CurrencyRegexString).Success) == false)
            throw new ConfigurationException(
                $"{nameof(CnbProviderConfiguration)} - {nameof(Currencies)} - All currencies must be exactly 3 characters without digits");
    }

    public IEnumerable<Currency> GetCurrencies()
    {
        return from currency in Currencies
               select new Currency(currency);
    }
}