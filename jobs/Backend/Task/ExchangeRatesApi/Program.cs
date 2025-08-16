using ExchangeRatesService.Configuration;
using ExchangeRatesService.Providers;
using ExchangeRatesService.Providers.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
[assembly: ApiController]
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddOptionsWithValidateOnStart<CnbExchangeRateApiConfig>()
    .Bind(builder.Configuration.GetSection("CnbApi"))
    .ValidateDataAnnotations();

builder.Services.AddHttpClient<IRatesProvider, ExchangeRateProvider>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptionsMonitor<CnbExchangeRateApiConfig>>().CurrentValue;
    client.BaseAddress = new Uri(options.ApiUrl);
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();
    
app.Run();