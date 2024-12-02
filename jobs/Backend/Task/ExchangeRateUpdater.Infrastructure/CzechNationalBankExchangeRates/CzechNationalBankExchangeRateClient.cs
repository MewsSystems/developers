using ExchangeRateUpdater.Application.GetExchangeRates;
using ExchangeRateUpdater.Infrastructure.Common;
using ExchangeRateUpdater.Infrastructure.Common.Configuration;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBankExchangeRates;

public class CzechNationalBankExchangeRateClient : ICzechNationalBankExchangeRateClient
{
    private readonly IRestClient _restClient;
    private readonly InfrastructureOptions _options;

    public CzechNationalBankExchangeRateClient(IRestClient restClient, IOptions<InfrastructureOptions> options)
    {
        _restClient = restClient;
        _options = options.Value;
    }

    public async Task<string?> GetAsync(DateOnly date)
    {
        const string dateFormat = "dd.MM.yyyy";
        var formatedDate = date.ToString(dateFormat);
        var url = $"{_options.CzechNationalBankExchangeRateService.Url}?date={formatedDate}";
        var response = await _restClient.GetAsync<string?>(url);
        return response;
    }
}