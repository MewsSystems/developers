using AutoMapper;
using ExchangeRateUpdater.Grpc.Interceptors;
using ExchangeRateUpdater.Grpc.Mapping;
using ExchangeRateUpdater.Implementation.Queries;
using ExchangeRateUpdater.Implementation.Services;
using ExchangeRateUpdater.Interface.Configuration;
using ExchangeRateUpdater.Interface.DTOs;
using ExchangeRateUpdater.Interface.Model.Validators;
using ExchangeRateUpdater.Interface.Services;
using FluentValidation;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using ExchangeRatesService = ExchangeRateUpdater.Grpc.Services.ExchangeRatesService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions();
 
builder.Services.AddOptions<CnbSettings>().BindConfiguration("CnbSettings").ValidateOnStart().ValidateDataAnnotations();

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ExceptionInterceptor>();
});

builder.Services.AddMemoryCache();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new ProtoMapping());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetExchangeRatesQuery).Assembly, 
                                                                      typeof(GetExchangeRatesQueryHandler).Assembly));

builder.Services.AddScoped<IValidator<GetExchangeRatesQuery>, GetExchangeRatesQueryValidator>();
builder.Services.AddScoped<IValidator<CurrencyDto>, CurrencyDtoValidator>();

builder.Services.AddScoped<ICnbApiService, CnbApiService>();
builder.Services.AddScoped<IExchangeRatesCacheService, ExchangeRatesCacheService>();

builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Path.GetTempPath()))
    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
    {
        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
        ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
    });

var app = builder.Build();

app.MapGrpcService<ExchangeRatesService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
