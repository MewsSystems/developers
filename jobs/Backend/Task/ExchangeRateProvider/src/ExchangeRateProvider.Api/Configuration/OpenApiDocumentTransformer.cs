using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using System.Diagnostics.CodeAnalysis;

namespace ExchangeRateProvider.Api.Configuration
{
    [ExcludeFromCodeCoverage(Justification = "OpenAPI configuration with only metadata assignment - no testable logic")]
    public class OpenApiDocumentTransformer : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            document.Info = new OpenApiInfo
            {
                Title = "Exchange Rate Provider API",
                Version = "v1",
                Description = "API for retrieving exchange rates for the given currency." +
                              "Provides current exchange rates and supports filtering by quote currencies.",
                Contact = new OpenApiContact
                {
                    Name = "API Support",
                    Email = "ashleighadams.contact@gmail.com"
                }
            };

            return Task.CompletedTask;
        }
    }
}
