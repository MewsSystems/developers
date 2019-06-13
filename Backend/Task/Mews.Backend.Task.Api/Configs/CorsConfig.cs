using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Mews.Backend.Task.Api.Configs
{
    public static class CorsConfig
    {
        public static void SetupCors(CorsPolicyBuilder options)
        {
            options
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials();
        }
    }
}
