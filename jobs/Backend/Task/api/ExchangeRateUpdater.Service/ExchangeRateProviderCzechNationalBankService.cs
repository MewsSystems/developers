using System.Net;
using System.Xml.Serialization;
using ExchangeRateUpdater.Contract.Helpers;
using ExchangeRateUpdater.Contract.Services;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Domain.CoreModels;
using ExchangeRateUpdater.Domain.CzechNationalBank;
using ExchangeRateUpdater.Localization;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Service;

public class ExchangeRateProviderCzechNationalBankService : ExchangeRateProviderServiceBase
{
  private readonly ExchangeRateProviderSettings _exchangeRateProviderSettings;
  private readonly Localizer _localizer;
  private readonly ILogger<ExchangeRateProviderCzechNationalBankService> _logger;
  
  public ExchangeRateProviderCzechNationalBankService(
    ExchangeRateProviderSettings exchangeRateProviderSettings,
    Localizer localizer,
    MemoryCacheHelper memoryCacheHelper,
    ILogger<ExchangeRateProviderCzechNationalBankService> logger) : base(memoryCacheHelper)
  {
    _exchangeRateProviderSettings = exchangeRateProviderSettings;
    _localizer = localizer;
    _logger = logger;
  }
  
  /// <summary>
  /// <inheritdoc />
  /// </summary>
  protected override async Task<IEnumerable<ExchangeRate>> GetExchangeRatesFromExternalSource(IEnumerable<Currency> currencies)
  {
    try
    {
      ExchangeRateHeaderBase? exchangeRatesBase = await FetchExchangeRatesFromProvider();
      if (exchangeRatesBase != null)
      {
        ExchangeRateHeader exchangeRates = (ExchangeRateHeader)exchangeRatesBase;

        return exchangeRates.Table.Row
          .Where(x => currencies.Select(currencyParam => currencyParam.Code.ToLowerInvariant())
            .Contains(x.Code.ToLowerInvariant()))
          .Select(x => new ExchangeRate(
            new Currency(x.Code),
            new Currency(_exchangeRateProviderSettings.DefaultCurrencyCode),
            x.Rate));
      }
      
      return Enumerable.Empty<ExchangeRate>();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, _localizer["Errors:FilterExchangeRatesError"]);
      throw;
    }
  }
  
  /// <summary>
  /// <inheritdoc />
  /// </summary>
  protected override async Task<ExchangeRateHeaderBase?> FetchExchangeRatesFromProvider()
  {
    try
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
      using (var client = new HttpClient())
      {
        Stream responseStream = await client.GetStreamAsync(_exchangeRateProviderSettings.ExchangeRatesSourceUrl);
        XmlSerializer serializer = new XmlSerializer(typeof(ExchangeRateHeader));
        
        return (ExchangeRateHeader?)serializer.Deserialize(responseStream);
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, _localizer["Errors:FetchExchangeRatesFromCnbError"]);
      throw;
    }
  }
}