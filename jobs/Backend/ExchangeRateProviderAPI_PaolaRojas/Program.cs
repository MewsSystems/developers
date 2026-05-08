using ExchangeRateProviderAPI_PaolaRojas.Models.Handlers;
using ExchangeRateProviderAPI_PaolaRojas.Models.Options;
using ExchangeRateProviderAPI_PaolaRojas.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Bind Configuration Options
builder.Services.Configure<CnbOptions>(
    builder.Configuration.GetSection(CnbOptions.SectionName));
builder.Services.Configure<ApiKeyOptions>(
    builder.Configuration.GetSection(ApiKeyOptions.SectionName));

// Add services
builder.Services.AddHttpClient<IExchangeRateService, ExchangeRateService>();
builder.Services.AddMemoryCache();
builder.Services.AddControllers();

//Add startup service tester
builder.Services.AddHostedService<StartupExchangeRateTester>();

// Add API Key Authentication
builder.Services.AddAuthentication(ApiKeyAuthenticationOptions.DefaultScheme)
    .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthHandler>(ApiKeyAuthenticationOptions.DefaultScheme, options => { });

// Swagger with API Key
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key needed to access endpoints.",
        Type = SecuritySchemeType.ApiKey,
        Name = "X-API-KEY",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                In = ParameterLocation.Header
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();