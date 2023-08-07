using Ardalis.GuardClauses;
using AutoMapper;
using Mews.ExchangeRate.Domain;
using Mews.ExchangeRate.Domain.Exceptions;
using Mews.ExchangeRate.Provider.CNB.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

namespace Mews.ExchangeRate.Provider.CNB;
internal sealed class CnbExchangeRateProvider :
    IRetrieveExchangeRatesFromSource,
    IHealthCheck
{
    private readonly ILogger<CnbExchangeRateProvider> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMapper _mapper;
    private readonly CnbSettings _settings;

    public CnbExchangeRateProvider(ILogger<CnbExchangeRateProvider> logger,
        IOptions<CnbSettings> options,
        IHttpClientFactory httpClientFactory, 
        IMapper mapper)
    {
        Guard.Against.Null(logger);
        Guard.Against.Null(httpClientFactory);
        Guard.Against.Null(options);
        Guard.Against.Null(options.Value);
        Guard.Against.Null(mapper);

        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _mapper = mapper;
        _settings = options.Value;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy("OK"));
    }

    public async Task<IEnumerable<Domain.ExchangeRate>> GetAllExchangeRatesAsync()
    {
        _logger.LogInformation("Retrieving list of all exchange rates from Czech National Bank");
        
        try
        {
            using var httpClient = _httpClientFactory.CreateClient(_settings.ExratesEndpoint);
            var rates = await httpClient.GetFromJsonAsync<Dtos.CnbExRatesApiResponse>(_settings.ExratesEndpoint);

            if (rates is null)
            {
                _logger.LogError("Czech National Bank API returned no values, returning empty collection to the caller.");
                return Enumerable.Empty<Domain.ExchangeRate>();
            }

            return _mapper.Map<IEnumerable<Domain.ExchangeRate>>(rates.Rates);
        }
        catch (Exception e)
        {
            _logger.LogError(e, 
                "An error has occurred when trying to retrieve Exchange Rates from Czech National Bank with settings: {settings}",
                _settings);
            throw new ExchangeRateException("An error has occurred when trying to retrieve Exchange Rates from Czech National Bank API");
        }
    }
}