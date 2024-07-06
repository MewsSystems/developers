using ExchangeRates.Api.Infrastructure.Clients.Common.DelegatingHandlers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class DelegatingHandlersServiceCollectionExtensions
{
    public static IHttpClientBuilder AddRequestTimingDelegatingHandler(this IHttpClientBuilder builder)
    {
        builder.Services
            .TryAddTransient<RequestTimingDelegatingHandler>();

        builder.AddHttpMessageHandler<RequestTimingDelegatingHandler>();

        return builder;
    }
}
