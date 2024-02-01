using ApiClients.CzechNationalBank.Models;
using Core.Services.CzechNationalBank.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Data;
using Data.Models;
using Data.Repositories.Interfaces;
using Infrastructure.CzechNationalBank;
using Infrastructure.CzechNationalBank.Requests;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace Core.Services.CzechNationalBank;

public class ExchangeRateService : IExchangeRateService
{
    private readonly ILog<ExchangeRateService> _logger;
    private readonly ICheckBankRateService _checkBankRateService;
    private readonly IGenericRepository<CurrencyCzechRate> _repository;
    private readonly ICzechNationalBankHttpApiClient _apiClient;
    private readonly IOptions<CzechNationalBankApiOptions> _czechNationalBankApiOptions;
    private const string TargetCurrency = "ZCH";

    public ExchangeRateService(ICzechNationalBankHttpApiClient apiClient, IOptions<CzechNationalBankApiOptions> czechNationalBankApiOptions,
        IGenericRepository<CurrencyCzechRate> repository, ICheckBankRateService checkBankRateService, ILog<ExchangeRateService> logger)
    {
        _checkBankRateService = checkBankRateService;
        _repository = repository;
        _apiClient = apiClient;
        _czechNationalBankApiOptions = czechNationalBankApiOptions;
        _logger = logger;
    }

    public async Task<List<ExchangeRate>> GetExchangeRates(List<Currency> currencies)
    {
        if (_checkBankRateService.IsNeededCallCzechNationalBankRates())
        {
            var source = await _apiClient.ExecuteAsync(new GetExchangeRatesRequest(_czechNationalBankApiOptions));
            _logger.Info("GetExchangeRate from service", source);
            await ReadSourceAndSaveEntity(source);           
        }
        var exchangeRates = new List<ExchangeRate>();
        foreach (var currency in currencies)
        {
            var lastElement = _repository.LastElement();
            var entity = await _repository.SingleOrDefaultAsync(x => x.Code == currency.Code.ToUpper() && x.CreatedDate.Date == lastElement.CreatedDate.Date);
            if (entity != null)
                exchangeRates.Add(new ExchangeRate(new Currency(currency.Code.ToUpper()), new Currency(TargetCurrency), entity.Rate));
        }
        return exchangeRates;
    }

    private async Task ReadSourceAndSaveEntity(string source)
    {
        if (!string.IsNullOrEmpty(source))
        {
            source = source[(source.IndexOf('\n') + 1)..];
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "|"
            };
            var reader = new StringReader(source);
            using var csv = new CsvReader(reader, config);
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                var record = csv.GetRecord<CurrencyCzechRateRecord>();
                if(record != null)
                {
                    await _repository.AddAsync(
                        new CurrencyCzechRate(record.Country, record.Currency, record.Amount, record.Code.ToUpper(), record.Rate, DateTime.Now));
                }
            }
        }
    }
}

public class CurrencyCzechRateRecord
{
    [Index(0)]
    public string Country { get; set; }
    [Index(1)]
    public string Currency { get; set; }
    [Index(2)]
    public decimal Amount { get; set; }
    [Index(3)]
    public string Code { get; set; }
    [Index(4)]
    public decimal Rate { get; set; }
}
