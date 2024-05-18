using ExchangeRateUpdater.API.Models.RequestModels;
using ExchangeRateUpdater.Application.ExchangeRates.Queries.GetExchangeRates;
using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdater.DependencyResolution;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using static ExchangeRateUpdater.API.Controllers.ExchangeRatesController;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// register services
services.AddMvc();
services.AddConfigurationSections(configuration);
services.AddServiceRegistrations();
services.AddExternalApiRegistrations();
// validator services.AddScoped<IValidator<Currency>, CurrencyValidator>();
services.AddScoped<IValidator<GetExchangeRatesRequest>, CurrencyValidator>();
services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetExchangeRatesQuery).Assembly));
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// configure middleware
var app = builder.Build();

app.UseHsts();

app.UseHttpsRedirection()
.UseRouting()
.UseAuthorization()
.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
})
.UseSwagger()
.UseSwaggerUI();

app.Run();