using ExchangeRateUpdater.Application.ExchangeRates.GetExchangeRates;
using ExchangeRateUpdater.Infrastructure;
using MediatR;
using ExchangeRateUpdater.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetExchangeRatesQuery).Assembly));

builder.Services.AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/exchange-rates", async (GetExchangeRatesRequest request, IMediator mediator) =>
{
    try
    {
        var query = new GetExchangeRatesQuery(request.ToCurrencies(), request.Date);
        var rates = await mediator.Send(query);
        
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
})
.WithName("GetExchangeRates")
.WithOpenApi();

app.Run();