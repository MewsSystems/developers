using System.Text.Json;

namespace ExchangeRateProvider.Exceptions;

public class UnexpectedException : Exception
{
    public UnexpectedException(string msg) : base(msg)
    {
    }

    public string DataToString()
    {
        return JsonSerializer.Serialize(Data, new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        });
    }
}