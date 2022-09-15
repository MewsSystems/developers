using System.Globalization;
using ExchangeRateUpdater.Contract.Helpers;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.Contract.Services;

/// <summary>
/// Abstract class describing methods that should be provided for acquiring the exchange rates from the providers.
/// </summary>
public abstract class ExchangeRateProviderServiceBase
{
  private readonly MemoryCacheHelper? _memoryCacheHelper;
  
  private readonly IEnumerable<Currency>? _defaultCurrencies = new[] 
  { 
    new Currency("USD"), 
    new Currency("EUR"), 
    new Currency("CZK"), 
    new Currency("JPY"), 
    new Currency("KES"), 
    new Currency("RUB"), 
    new Currency("THB"), 
    new Currency("TRY"), 
    new Currency("XYZ") 
  };

  protected ExchangeRateProviderServiceBase(MemoryCacheHelper memoryCacheHelper)
  {
    _memoryCacheHelper = memoryCacheHelper;
  }

  /// <summary>
  /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
  /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
  /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
  /// some of the currencies, ignore them.
  /// </summary>
  public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency>? currencies = null)
  {
    currencies ??= _defaultCurrencies;
    
    List<Currency> currenciesLst = currencies!.ToList();
    string currencyCodes = string.Join(",", currenciesLst.Select(x => x.Code.ToLowerInvariant()).OrderBy(x => x));
    string cacheKey = $"{DateTime.UtcNow.Date.ToString(CultureInfo.InvariantCulture)}-{currencyCodes}";
      
    IEnumerable<ExchangeRate>? result = _memoryCacheHelper?.GetFromCache<IEnumerable<ExchangeRate>>(cacheKey);
    if (result != null)
    {
      return result;
    }
    else
    {
      result = await GetExchangeRatesFromExternalSource(currenciesLst);
      if (result != null)
      {
        _memoryCacheHelper?.InsertToCache(cacheKey, result);

        return result;
      }
    }
      
    return Enumerable.Empty<ExchangeRate>();
  }

  /// <summary>
  /// <see cref="GetExchangeRates(System.Collections.Generic.IEnumerable{ExchangeRateUpdater.Domain.Currency})" />
  /// The result does not exists in the cache so get the data from the external source and process it.
  /// </summary>
  /// <param name="currencies"></param>
  /// <returns></returns>
  protected abstract Task<IEnumerable<ExchangeRate>>? GetExchangeRatesFromExternalSource(IEnumerable<Currency> currencies);

  /// <summary>
  /// It should fetch the data about the rates from the selected provider (usually from the internet).
  /// </summary>
  /// <returns></returns>
  protected abstract Task<ExchangeRateHeaderBase?> FetchExchangeRatesFromProvider();
}