global using ExchangeRateUpdater.WebApi.Models;
using System.Reflection;
using ExchangeRateUpdater.WebApi.Filters;
using ExchangeRateUpdater.WebApi.Services.ExchangeRateDownloader;
using ExchangeRateUpdater.WebApi.Services.ExchangeRateParser;
using ExchangeRateUpdater.WebApi.Services.ExchangeRateProvider;
using ExchangeRateUpdater.WebApi.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

builder.Services.AddValidatorsFromAssemblyContaining<CurrencyListValidator>(); 
builder.Services.AddFluentValidationAutoValidation(); 
builder.Services.AddFluentValidationClientsideAdapters(); 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Swagger API",
        Description = "Exchange rate provider"
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddScoped<IExchangeRateDownloader, ExchangeRateDownloader>();
builder.Services.AddScoped<IExchangeRateParser, ExchangeRateParser>();
builder.Services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();

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
