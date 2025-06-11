using ExchangeRate.Api.Mappers;
using ExchangeRate.Application.Parsers;
using ExchangeRate.Application.Parsers.Interfaces;
using ExchangeRate.Application.Services;
using ExchangeRate.Application.Services.Interfaces;
using ExchangeRate.CrossCutting.Configurations;
using ExchangeRate.Infrastructure.ExternalServices.CzechNationalBank;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using System.Reflection;

namespace ExchangeRate.Api.Extensions
{
    public static class ConfigureService
    {
        public static void ConfigureApiServices(this WebApplicationBuilder builder) 
        {
            ConfigureCors(builder.Services);
            builder.Services.Configure<JsonOptions>(o => o.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));
            builder.Services.AddControllers().AddNewtonsoftJson( options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAutoMapper(typeof(MappingModelProfile));

            ConfigureSwagger(builder.Services);

            Load(builder.Services, builder.Configuration);

        }

        private static void Load(IServiceCollection services, ConfigurationManager configuration)
        {
            IInfraConfigs infra = new InfraConfigs();
            configuration.GetSection("InfraConfigs").Bind(infra);

            services.AddSingleton<IInfraConfigs>(infra);

            services.AddScoped<IParserService, ParserService>();
            services.AddScoped<ICurrencyParser, CurrencyParser>();
            services.AddScoped<IExchangeRateService, ExchangeRateService>();
            services.AddScoped<IExchangeRateParserTxt, ExchangeRateParserTxt>();
            services.AddScoped<IExchangeRateParserXml, ExchangeRateParserXml>();
            services.AddScoped<ICzechNationalBankService, CzechNationalBankService>();
            services.AddScoped<IExchangeRateProviderService, ExchangeRateProviderService>();
            
        }

        private static void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(x => x.AddPolicy("MyAllowSpecificOrigins", builder =>
            {
                builder.WithOrigins("*")
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .Build();
            }));
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddMemoryCache();

            services.AddSwaggerGen(swgOptions =>
            {
                swgOptions.CustomSchemaIds( type => type.Name);
                swgOptions.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Exchange Rate Api Czech National Bank",
                    Version = "v1",
                    Description = "Provider based on real world public data source of Czech National Bank",
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swgOptions.IncludeXmlComments(xmlPath);
            });


        }
    }

}
