using ExchangeRateUpdater.Api.Application.Extensions;
using ExchangeRateUpdater.Api.Infrastructure.Extensions;
using ExchangeRateUpdater.Api.Presentation.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApiDocumentation(builder.Configuration);
builder.Services.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapGet("/", () => Results.Redirect("/scalar/v1"))
    .ExcludeFromDescription();

app.MapControllers();

app.Run();
