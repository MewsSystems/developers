using ExchangeRateUpdater.Common.Exceptions;
using ExchangeRateUpdater.Common.PipelineBehaviours;
using ExchangeRateUpdater.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http.Headers;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionPipelineBehaviour<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));
});
builder.Services.AddControllers();
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

builder.Services.AddMemoryCache();
builder.Services.AddFusionCache();

// Retry policy (exponential backoff)
var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .WaitAndRetryAsync(3, retryAttempt =>
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

// Circuit breaker policy
var circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30));

builder.Services.AddHttpClient<IAPIClient, APIClient>( c => {
    c.BaseAddress = new Uri(builder.Configuration["CnbExchangeRatesApi"] ?? throw new InvalidOperationException("Missing Configuration Value : CnbApi"));
    c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddPolicyHandler(retryPolicy)
.AddPolicyHandler(circuitBreakerPolicy);

builder.Services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();

builder.Services.AddHealthChecks();

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHealthChecks("/hc");
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.Run();