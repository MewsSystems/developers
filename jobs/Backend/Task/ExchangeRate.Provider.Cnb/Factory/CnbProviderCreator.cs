using ExchangeRate.Models;
using ExchangeRate.Provider.Base.Interfaces;
using ExchangeRate.Provider.Base.Models;
using ExchangeRate.Provider.Base.Service;
using ExchangeRate.Provider.Cnb.Interfaces;
using ExchangeRate.Provider.Cnb.Models.Configuration;
using Microsoft.Extensions.Options;

namespace ExchangeRate.Provider.Cnb.Factory;

public class CnbProviderCreator : ProviderCreator
{
    #region Fields

    private readonly ICnbHttpClient _cnbHttpClient;
    private readonly IOptions<CnbProviderConfiguration> _configuration;
    private readonly Currency _currency;

    #endregion

    #region Constructors

    public CnbProviderCreator(ICnbHttpClient cnbHttpClient, IOptions<CnbProviderConfiguration> configuration)
    {
        _cnbHttpClient = cnbHttpClient;
        _configuration = configuration;
        _currency = new Currency(Currencies.Czk);
    }

    #endregion

    public override IExchangeRateProvider CreateProvider()
    {
        _configuration.Value.Validate();

        return new ConcreteCnbProvider(_cnbHttpClient, _currency);
    }
}