using Serilog;
using ExchangeRates.Infrastructure.External.CNB;
using ExchangeRates.Application.Providers;

namespace ExchangeRates.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((ctx, lc) =>
                lc.ReadFrom.Configuration(ctx.Configuration));

            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();
            Configure(app, app.Environment);
            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var baseUrl = configuration.GetValue<string>("CNBApi:BaseUrl");

            services.AddHttpClient<CNBHttpClient>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddPolicyHandler(CNBHttpClientPolicies.TimeoutPolicy())
            .AddPolicyHandler(CNBHttpClientPolicies.RetryPolicy())
            .AddPolicyHandler(CNBHttpClientPolicies.CircuitBreakerPolicy());

            services.AddScoped<ExchangeRatesProvider>();
        }

        private static void Configure(WebApplication app, IHostEnvironment env)
        {
            app.UseExceptionHandler("/error");
            app.UseCors("AllowAll");
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();
            app.MapControllers();
            app.Map("/error", (HttpContext httpContext) =>
            {
                return Results.Problem("An unexpected error occurred. Please try again later.");
            });
        }
    }
}
