using ExchangeRateProvider.BusinessLogic.IBusinessLogic;
using ExchangeRateProvider.BusinessLogic;
using ExchangeRateProvider.Persistence;
using ExchangeRateProvider.Persistence.IRepo;
using ExchangeRateProvider.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddScoped<ICurrencyExchangeRepo, CurrencyExchangeRepo>();
builder.Services.AddScoped<ICurrencyPairRates, CurrencyPairRates>()
    .AddProblemDetails()
    .AddExceptionHandler<GlobleExceptionHandler>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<ICurrencyExchangeRepo, CurrencyExchangeRepo>();

var app = builder.Build();

app.UseStatusCodePages();
app.UseExceptionHandler();

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
