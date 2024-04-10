namespace ExchangeRates.Api.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddControllers();
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();

        return services;
    }
}
