using AutoMapper;
using MewsFinance.Application.Clients;
using MewsFinance.Application.Extensions;
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
            var sourceCurrencyCodes = exchangeRateRequest.CurrencyCodes;

            var exchangeRateClientResponse = await _financialClient.GetExchangeRates(DateTime.UtcNow);

            if (!exchangeRateClientResponse.IsSuccess)
            {
                return Enumerable.Empty<ExchangeRateResponse>();
            }

            var exchangeRates = exchangeRateClientResponse.Data;

            var filteredExchangeRates = exchangeRates.FilterBySourceCurrency(sourceCurrencyCodes);

            var exchangeRateResponse = _mapper.Map<IEnumerable<ExchangeRateResponse>>(filteredExchangeRates);

            return exchangeRateResponse;
        }
    }
}