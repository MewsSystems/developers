using Microsoft.OpenApi.Models;
using ExchangeRateUpdater.BusinessLogic;
using ExchangeRateUpdater.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddLogging(l => l.AddConsole());
builder.Services.AddHttpClient();
builder.Services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
builder.Services.AddScoped<IExchangeRateManager>(sp =>
    new ExchangeRateManager(
        currencies => sp.GetRequiredService<IExchangeRateProvider>().GetExchangeRates(currencies)
    )
);

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Exchange Rate API",
        Version = "v1",
        Description = "API for retrieving Czech National Bank (CNB) exchange rates",
        Contact = new OpenApiContact
        {
            Name = "Mews Systems"
        }
    });

    // Include XML comments if available
    var xmlFile = Path.Combine(AppContext.BaseDirectory, "ExchangeRateUpdater.Api.xml");
    if (File.Exists(xmlFile))
    {
        c.IncludeXmlComments(xmlFile);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Exchange Rate API V1");
});

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
