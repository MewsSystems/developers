namespace CNB.ApiClient;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCNBClient(this IServiceCollection services)
    {
        services.Configure<CNBApiSettings>(options =>
        {
            options.BaseUrl = new Uri("https://api.cnb.cz");
        });

        services
            .AddHttpClient<ICNBApiClient, CNBApiClient>()
            .ConfigureHttpClient(
                (provider, client) =>
                {
                    var options = provider.GetRequiredService<IOptions<CNBApiSettings>>();
                    client.BaseAddress = options.Value.BaseUrl;
                    client.DefaultRequestHeaders.Add("Accept", MediaTypeNames.Application.Json);
                }
            );

        return services;
    }
}
