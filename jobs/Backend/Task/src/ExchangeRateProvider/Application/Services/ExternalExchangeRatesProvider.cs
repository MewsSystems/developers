using AutoMapper;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Infrastructure.Clients.CzechNationalBank;
using ExchangeRateUpdater.Infrastructure.Services;

namespace ExchangeRateUpdater.Application.Services;

public interface IExternalExchangeRatesProvider
{
    Task<IEnumerable<ExchangeRate>> ProvideAsync();
}

public class ExternalExchangeRatesProvider(ICzechNationalBankService czechNationalBankService, IMapper mapper) : IExternalExchangeRatesProvider
{
    private readonly ICzechNationalBankService _czechNationalBankService = czechNationalBankService;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<ExchangeRate>> ProvideAsync()
    {
        var exchangeRates = await _czechNationalBankService.ProvideExchangeRatesAsync();        
        return GetDomainExchangeRates(exchangeRates);
    }

    private IEnumerable<ExchangeRate> GetDomainExchangeRates(IEnumerable<CzechNationalBankExchangeRate> exRates) => _mapper.Map<IEnumerable<ExchangeRate>>(exRates);
}
