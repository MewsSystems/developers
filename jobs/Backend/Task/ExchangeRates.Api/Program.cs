using ExchangeRates.Api.DependencyInjection;
using ExchangeRates.Application.DependencyInjection;
using ExchangeRates.Infrastructure.DependencyInjection;

// Configuration Root.
var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
    .AddApi()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);
}

// Resolution Root.
var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // Error middleware.
    app.UseExceptionHandler("/error");

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}

// Make the implicit Program class public so test projects can access it.
public partial class Program { }
