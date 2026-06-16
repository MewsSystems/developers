using ApiClients.CzechNationalBank.Models;
using Core.Services.CzechNationalBank;
using Core.Services.CzechNationalBank.Interfaces;
using Data;
using Data.Database;
using Data.Models;
using Data.Repositories;
using Data.Repositories.Interfaces;
using Infrastructure.CzechNationalBank;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Services.Handlers.CzechNationalBank;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// configure logger
builder.Host.UseSerilog((hostingContext, services, loggerConfiguration) => 
loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration).Enrich.FromLogContext());

builder.Services.AddSingleton(typeof(ILog<>), typeof(Log<>));

var serviceProvider = builder.Services.BuildServiceProvider();
var configuration = serviceProvider.GetService<IConfiguration>();

builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
{
    options.UseSqlite(configuration.GetConnectionString("ConnectionDatabase"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

//Add options
builder.Services.AddOptions<CzechNationalBankApiOptions>().Configure(configuration.GetSection("CzechNationalBankApi").Bind).ValidateDataAnnotations();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetRateRequestHandler).GetTypeInfo().Assembly));

builder.Services.AddHttpClient("CzechNationalBankApi", c => { });
builder.Services.AddSingleton<ICzechNationalBankHttpApiClient, CzechNationalBankHttpApiClient>();

builder.Services.AddScoped<IGenericRepository<CurrencyCzechRate>, GenericRepository<CurrencyCzechRate>>();
builder.Services.AddScoped<ICheckBankRateService, CheckBankRateService>();
builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>(); 

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
