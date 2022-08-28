namespace ExchangeRateUpdater;

public static class SerializationHelpers
{
    public static T DeserializeToObject<T>(this string input) where T : class
    {
        var ser = new System.Xml.Serialization.XmlSerializer(typeof(T));
        using (var reader = new StringReader(input))
            return (T)ser.Deserialize(reader);
    }
}