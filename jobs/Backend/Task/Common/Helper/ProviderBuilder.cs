using Common.Exceptions;
using Common.Interface;
using Common.Logs;
using Common.Providers;

namespace Common.Helper;

/// <summary>
/// Builder class which can build several exchange rate providers.
/// </summary>
public class ProviderBuilder
{
    #region Public methods.

    public IExchangeRateProvider GetProvider<T>() where T : IExchangeRateProvider
    {
	    Type type = typeof(T);
	    if (typeof(CnbProvider) == type)
	    {
		    Log.Instance.Error($"Creating {typeof(T)} instance.");
			return new CnbProvider();
	    }

		Log.Instance.Error($"No exchange rate provider found for given type {typeof(T)}.");
	    throw new ProviderNotBuiltException<T>();
    }

    #endregion

}