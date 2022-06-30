using System.Net.Http;

namespace ExchangeRate.Service.UnitTests.Extensions;

public static class HttpResponseMessageExtensions
{
    public static HttpResponseMessage SetContent(this HttpResponseMessage message, string content)
    {
        message.Content = new StringContent(content);
        return message;
    }
}