using ExchangeRateDemo.Application;
using ExchangeRateDemo.Infrastructure.Providers.ExchangeRateProvider;
using ExchangeRateDemo.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICacheService, CacheProvider>();

builder.Services.AddExchangeRateProvider(builder.Configuration);

builder.Services.AddMediatR(opt =>
{
    opt.RegisterServicesFromAssemblies(typeof(MediatorObject).Assembly);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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
