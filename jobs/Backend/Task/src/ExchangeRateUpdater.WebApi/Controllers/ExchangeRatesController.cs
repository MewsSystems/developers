using Carter;
using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdater.Application.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.WebApi.Controllers
{
    public class ExchangeRatesController : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {

            app.MapPost("/exchangeRates", async ([FromBody] IEnumerable<string> currencies,
                GetExchangeRatesQuery query) =>
            {
                if (currencies == null || !currencies.Any())
                    return Results.Problem(
                        type: "validation_error",
                        title: "Invalid request",
                        detail: $"Currencies must be provided.",
                        statusCode: StatusCodes.Status400BadRequest);

                var exchangeRates = await query.ExecuteAsync(currencies.Select(c => new Currency(c)));

                if (exchangeRates == null || !exchangeRates.Any())
                    return Results.NotFound();

                return Results.Ok(exchangeRates);
            })
            .Produces<IEnumerable<string>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)            
            .WithTags("ExchangeRates")
            .WithName("GetByCurrencies");           
        }
    }
}
