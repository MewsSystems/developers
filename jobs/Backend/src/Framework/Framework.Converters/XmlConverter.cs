using System.Xml.Serialization;
using Framework.Converters.Abstract;
using Framework.Exceptions;
using Microsoft.Extensions.Logging;

namespace Framework.Converters
{
	public class XmlConverter : IXmlConverter
	{
		private readonly ILogger<XmlConverter> _logger;

		public XmlConverter(ILogger<XmlConverter> logger)
		{
			_logger = logger;
		}

		public T ConvertFromXml<T>(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new EmptyResultSetException("Cannot convert empty value.");
			}

			try
			{
				using TextReader reader = new StringReader(value);
				var deserializedObject = new XmlSerializer(typeof(T)).Deserialize(reader);
				if (deserializedObject != null)
				{
					return (T)deserializedObject;
				}
			}
			catch (Exception e)
			{
				const string errorMessage = "Content from xml request has invalid format";
				_logger.LogCritical(e, errorMessage);
				throw new ParsingException(errorMessage);
			}

			throw new EmptyResultSetException("Deserialized object is null.");
		}
	}
}
