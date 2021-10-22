using Microsoft.Extensions.Logging;
using System.IO;
using System.Xml.Serialization;

namespace Infrastructure.Serializers
{
    public class XmlDeserializer<T> : IDeserializer<T>
    {
        private readonly ILogger<XmlDeserializer<T>> _logger;

        public XmlDeserializer(ILogger<XmlDeserializer<T>> logger)
        {
            _logger = logger;
        }

        public T Deserialize(string input)
        {
            _logger.LogInformation("Serializing Xml data");

            var serializer = new XmlSerializer(typeof(T));
            using var stream = new StringReader(input);

            return (T)serializer.Deserialize(stream);
        }
    }
}
