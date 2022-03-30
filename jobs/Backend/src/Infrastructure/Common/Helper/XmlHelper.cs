using System.Xml.Serialization;

namespace ExchangeRate.Infrastructure.Common.Helper;

public static class XmlHelper
{
    /// <summary>
    /// Parses string xml input for XML object deserialization
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T FromXml<T>(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return default;

        using TextReader reader = new StringReader(value);

        return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
    }
}
