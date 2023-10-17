using Mews.ExchangeRateProvider.Extensions;
using Mews.ExchangeRateProvider.Mappers;
using Microsoft.Extensions.Options;

namespace Mews.ExchangeRateProvider;

public sealed class CzechNationalBankExchangeRateProvider : IExchangeRateProvider
{
    private readonly HttpMessageInvoker _httpMessageInvoker;
    private readonly CzechNationalBankExchangeRateMapper _mapper;
    private readonly ExchangeRateProviderOptions _options;

    public CzechNationalBankExchangeRateProvider(HttpMessageInvoker httpMessageInvoker, CzechNationalBankExchangeRateMapper mapper, IOptions<ExchangeRateProviderOptions> options)
    {
        _httpMessageInvoker = httpMessageInvoker;
        _mapper = mapper;
        _options = options.Value;
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken)
    {
        List<Task<HttpResponseMessage>> replies = new();
        List<ExchangeRate> result = new();

        replies.AddRange(_options.ExchangeRateProviders.Select(uri => _httpMessageInvoker.SendAsync(new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = uri.Uri
        }, cancellationToken)));

        await Task.WhenAll(replies);

        foreach (var reply in replies)
        {
            result.AddRange(_mapper.Read(await (await reply).Content.ReadAsStringAsync(cancellationToken)));
        }

        return result;
    }
}