using AutoMapper;
using MewsFinance.Application.Clients;
using MewsFinance.Application.Interfaces;

namespace MewsFinance.Application.UseCases.ExchangeRates.Queries
{
    public class GetExchangeRatesUseCase : IGetExchangeRatesUseCase
    {
        private readonly IFinancialClient _financialClient;
        private readonly IMapper _mapper;

        public GetExchangeRatesUseCase(IFinancialClient financialClient, IMapper mapper)
        {
            _financialClient = financialClient;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ExchangeRateResponse>> GetExchangeRates(ExchangeRateRequest exchangeRateRequest)
        {
            var currencyCodes = exchangeRateRequest.CurrencyCodes;

            var exchangeRates = await _financialClient.GetExchangeRates(DateTime.UtcNow);

            var exchangeRateResponse = _mapper.Map<IEnumerable<ExchangeRateResponse>>(exchangeRates);

            return exchangeRateResponse;
        }
    }
}