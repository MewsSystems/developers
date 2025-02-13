using System.Text;

namespace ExchangeRate.Api.Extensions
{
    public static class ApplyHostConfiguration
    {
        public static void ApplyConfiguration(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            var envAppSettings = Environment.GetEnvironmentVariable("CZECH_NATIONAL_BANK_PROVIDER");

            if (string.IsNullOrWhiteSpace(envAppSettings))
            {
                Console.WriteLine("Reading local configurations");
                configuration
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables();

                return;
            }

            Console.WriteLine("Reading configurations from the host server");

            try
            {
                // based on an applicantion hosted by AWS.
                using var appSettingsStream = new MemoryStream(Encoding.UTF8.GetBytes(envAppSettings));
                appSettingsStream.Position = 0;
                configuration
                    .AddJsonStream(appSettingsStream)
                    .AddEnvironmentVariables();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong reading variables: " + ex.Message);
                throw;
            }
        }
    }
}
