using ExchangeRateUpdater.Api.Metrics;
using ExchangeRateUpdater.Api.Models;
using ExchangeRateUpdater.Application.Queries.GetExchangeRates;
using ExchangeRateUpdater.Core.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Api.Handlers;

public static class ExchangeRatesHandler
{
    public static async Task<IResult> GetExchangeRates(
        [FromQuery] string[] currencyCodes,
        [FromQuery] DateTime? date,
        IMediator mediator,
        IValidator<GetExchangeRatesRequest> validator)
    {
        var request = new GetExchangeRatesRequest
        {
            CurrencyCodes = currencyCodes,
            Date = date ?? DateTime.UtcNow
        };

        var validationResult = await validator.ValidateAsync(request);
        
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(new
            {
                Success = false,
                Errors = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Message = e.ErrorMessage
                })
            });
        }

        try
        {
            var rates = await GetExchangeRates(request, mediator);
            
            SharedMetrics.ExchangeRatesCounter.Add(rates.Count);
            
            return Results.Ok(new
            {
                Success = true,
                rates.Count,
                Rates = rates
            });
        }
        catch (Exception e)
        {
            return Results.Problem(
                title: "Error fetching exchange rates",
                detail: e.Message,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

    private static async Task<IList<ExchangeRate>> GetExchangeRates(
        GetExchangeRatesRequest request,
        IMediator mediator)
    {
        var query = new GetExchangeRatesQuery(request.ToCurrencies(), request.Date);
        return await mediator.Send(query);
    }
}
