using ExchangeRatesService.Configuration;
using ExchangeRatesService.Providers;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
/*builder.Services.AddSwaggerGen(c => {
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.IgnoreObsoleteActions();
    c.IgnoreObsoleteProperties();
    c.CustomSchemaIds(type => type.FullName);
});*/

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddOptionsWithValidateOnStart<CnbExchangeRateApiConfig>()
    .Bind(builder.Configuration.GetSection("CnbApi"))
    .ValidateDataAnnotations();

builder.Services.AddHttpClient<ExchangeRateProvider>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptionsMonitor<CnbExchangeRateApiConfig>>().CurrentValue;
    client.BaseAddress = new Uri(options.ApiUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();