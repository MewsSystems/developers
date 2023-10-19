using Mews.ExchangeRateProvider.Exceptions;
using Mews.ExchangeRateProvider.Extensions;
using Mews.ExchangeRateProvider.Mappers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mews.ExchangeRateProvider;

/// <summary>
/// Obtains exchange rate data from the Czech National Bank referenced against a base currency of Czech Koruna 
/// </summary>
public class CzechNationalBankExchangeRateProvider : IExchangeRateProvider
{
    private readonly HttpMessageInvoker _httpMessageInvoker;
    private readonly CzechNationalBankExchangeRateMapper _mapper;
    private readonly CzechNationalBankExchangeRateProviderOptions? _options;
    private readonly ILogger<CzechNationalBankExchangeRateProvider> _logger;

    /// <summary>
    /// Constructs a new instance of <see cref="CzechNationalBankExchangeRateProvider"/>
    /// </summary>
    /// <param name="httpMessageInvoker">A HTTP Client or message invoker that will be used to fetch the remote exchange rate data</param>
    /// <param name="mapper">A class that maps the remote exchange rate content into the Exchange Rate models</param>
    /// <param name="options">Configuration options that includes the remote URIs to obtain the data from</param>
    /// <param name="logger">An implementation of <see cref="ILogger{CzechNationalBankExchangeRateProvider}"/></param>
    public CzechNationalBankExchangeRateProvider(HttpMessageInvoker httpMessageInvoker,
        CzechNationalBankExchangeRateMapper mapper,
        IOptions<CzechNationalBankExchangeRateProviderOptions> options,
        ILogger<CzechNationalBankExchangeRateProvider> logger)
    {
        _httpMessageInvoker = httpMessageInvoker;
        _mapper = mapper;
        _options = options?.Value;
        _logger = logger;
    }

    /// <summary>
    /// Obtains the most recent available exchange rate data for a given list of currencies
    /// </summary>
    /// <param name="currencies">A list of currency pairs to obtain exchange rate data for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A Task that will return a collection of <see cref="ExchangeRate"/> objects for each supplied currency, if known to the remote provider</returns>
    /// <exception cref="ArgumentNullException">Thrown if input parameters are null</exception>
    /// <exception cref="ArgumentException">Thrown if list of currencies is empty</exception>
    /// <exception cref="InvalidOperationException">Thrown if no endpoint URIs are defined in the _options field</exception>
    /// <exception cref="ObtainExchangeRateException">Thrown if there is a issue obtaining exchange rate data</exception>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken)
    {
        if (currencies == null) throw new ArgumentNullException(nameof(currencies), "You must supply a list of currencies you wish to see exchange rate data for");
        if (!currencies.Any()) throw new ArgumentException("The supplied list of currencies was empty", nameof(currencies));
        if (_options?.ExchangeRateProviders?.Any(erp => erp.Uri != null) != true) throw new InvalidOperationException("There are no exchange rate provider URIs defined in _options");

        List<Task<HttpResponseMessage>> replies = new();
        List<ExchangeRate> result = new();
        try
        {
            replies.AddRange(_options.ExchangeRateProviders!.Select(uri =>
            {
                _logger.LogInformation("Making call to obtain exchange rate data from Czech National Bank to {Uri}", uri.Uri!.AbsoluteUri);

                return _httpMessageInvoker.SendAsync(new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = uri.Uri
                }, cancellationToken);
            }));

            await Task.WhenAll(replies);

            _logger.LogInformation("All calls to Czech National Bank completed");
        }
        catch (Exception e)
        {
            var wrappedException = new ObtainExchangeRateException("Exception obtaining remote data, see inner exception for further details", e);
            _logger.LogError(wrappedException, "Exception thrown making call to obtain remote data");

            throw wrappedException;
        }

        foreach (var reply in replies)
        {
            var unwrappedReply = await reply;
            if (!unwrappedReply.IsSuccessStatusCode)
            {
                var wrappedException = new ObtainExchangeRateException($"Exception obtaining remote data, remote server replied with HTTP status code: {unwrappedReply.StatusCode}");
                _logger.LogError(wrappedException, "Exception thrown as remote source did not reply with successful HTTP status");

                throw wrappedException;
            }

            try
            {
                result.AddRange(_mapper.Read(await unwrappedReply.Content.ReadAsStringAsync(cancellationToken)));
            }
            catch (Exception e)
            {
                var wrappedException = new ObtainExchangeRateException("Exception reading/mapping remote data, see inner exception for further details", e);
                _logger.LogError(wrappedException, "Exception thrown whilst reading from or mapping remote data");

                throw wrappedException;
            }
        }

        var returnValue = result.Where(er => currencies.Select(c => c.Code).Contains(er.SourceCurrency.Code));

        _logger.LogInformation("Obtained total of {TotalCount} exchange rate values and selected {SelectedCount} from these", result.Count, returnValue.Count());

        return returnValue;
    }
}