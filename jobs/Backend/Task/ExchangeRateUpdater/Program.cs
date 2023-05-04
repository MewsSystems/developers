var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

builder.Services.AddTransient<IExchangeRateProviderService, ExchangeRateProviderService>();
builder.Services.AddTransient<IValidator<ExchangeRateRequest>, ExchangeRateRequestValidator>();
builder.Services.AddTransient<IValidator<Currency>, CurrencyCodeValidator>();
builder.Services.AddTransient<IHttpClientWrapper, HttpClientWrapper>();
builder.Services.AddTransient<IExchangeRatesParser, ExchangeRatesParser>();
builder.Services.AddTransient<ICalculatorService, CalculatorService>();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();