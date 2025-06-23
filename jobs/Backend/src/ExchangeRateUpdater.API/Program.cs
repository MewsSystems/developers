using ExchangeRateUpdater.API.Models.RequestModels;
using ExchangeRateUpdater.API.Validators;
using ExchangeRateUpdater.Application.ExchangeRates.Queries.GetExchangeRates;
using ExchangeRateUpdater.DependencyResolution;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// register services
services.AddMvc();
services.AddConfigurationSections(configuration);
services.AddServiceRegistrations();
services.AddExternalApiRegistrations();

services.AddScoped<IValidator<GetExchangeRatesRequest>, GetExchangeRatesRequestValidator>();
services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetExchangeRatesQuery).Assembly));
services.AddControllers();

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