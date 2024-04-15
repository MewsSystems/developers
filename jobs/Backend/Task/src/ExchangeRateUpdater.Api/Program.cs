using ExchangeRateUpdater.Api.Extensions;
using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();
app.UseHealthChecks("/health");
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors();
app.MapControllers();

app.Run();
