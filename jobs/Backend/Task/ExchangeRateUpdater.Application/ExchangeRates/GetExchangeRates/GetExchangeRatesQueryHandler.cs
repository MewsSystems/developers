using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Application.Common;
using MediatR;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Application.ExchangeRates.GetExchangeRates;

public class GetExchangeRatesQueryHandler(IExchangeRateProvider exchangeRateProvider)
    : IRequestHandler<GetExchangeRatesQuery, IList<ExchangeRate>>
{
    public async Task<IList<ExchangeRate>> Handle(GetExchangeRatesQuery request, CancellationToken cancellationToken)
    {
        return await exchangeRateProvider.GetExchangeRates(request.Currencies, request.Date);
    }
} 