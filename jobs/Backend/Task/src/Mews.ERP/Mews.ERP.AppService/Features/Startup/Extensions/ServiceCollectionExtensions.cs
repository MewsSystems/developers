using System.Diagnostics.CodeAnalysis;
using Mews.ERP.AppService.Features.Fetch.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Mews.ERP.AppService.Features.Startup.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureStartup(this IServiceCollection serviceCollection) 
        => serviceCollection.AddFetchFeature();
}