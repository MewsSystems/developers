using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication()
    .AddInfrastructure();


var configuration = builder.Configuration;

builder.WebHost.UseUrls("https://localhost:5064");

var cnbApiBaseUrl = configuration.GetValue<string>("ApiUrls:CzechNationalBankApi");
 
if (cnbApiBaseUrl is null)
{
    throw new ArgumentNullException("Czech national bank url is not configured");
}

builder.Services.AddHttpClient("CzechNationalBankApi", c =>
{
    c.BaseAddress = new Uri(cnbApiBaseUrl);
});

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

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
