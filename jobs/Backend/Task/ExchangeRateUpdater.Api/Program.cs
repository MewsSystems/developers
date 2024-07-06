using ExchangeRateUpdater.Api.Auth;
using ExchangeRateUpdater.Api.Handlers;
using ExchangeRateUpdater.Application.Features.ExchangeRates.GetByCurrency;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

ApplicationBootstrapper.AddServices(builder.Services, builder.Configuration);
builder.Services.Configure<AuthSettings>(
    builder.Configuration.GetSection(AuthSettings.ConfigSectionId));

builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    var apiKeyScheme = new OpenApiSecurityScheme
    {
        Description = "Api Key needed to access endpoints",
        In = ParameterLocation.Header,
        Name = "X-Api-Key",
        Type = SecuritySchemeType.ApiKey,
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKeyAuth"
        }
    };
    x.AddSecurityDefinition("ApiKeyAuth", apiKeyScheme);
    var securityRequirement = new OpenApiSecurityRequirement
    {
        [apiKeyScheme] = new List<string>()
    };
    x.AddSecurityRequirement(securityRequirement);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

var api = app
    .MapGroup("/")
    .AddEndpointFilter<AuthEndpointFilter>();

api.MapPost("exchangeRates",
    (IMediator mediator, [FromBody] GetExchangeRatesByCurrencyQuery query) => mediator.Send(query))
    .ProducesValidationProblem()
    .Produces<GetExchangeRatesByCurrencyQueryResponse>((int)HttpStatusCode.OK)
    .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError)
    .Produces((int)HttpStatusCode.Unauthorized)
    .WithOpenApi();

app.Run();

public partial class Program { }