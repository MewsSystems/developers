using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infrastructure.Serializers
{
    public class JsonDeserializer<T> : IDeserializer<T>
    {
        private readonly ILogger<JsonDeserializer<T>> _logger;

        public JsonDeserializer(ILogger<JsonDeserializer<T>> logger)
        {
            _logger = logger;
        }

        public T Deserialize(string input)
        {
            _logger.LogInformation("Serializing JSon data");

            return JsonSerializer.Deserialize<T>(input);
        }
    }
}