namespace System.Net.Http;

public static class HttpResponseMessageExtensions
{
    public static Task<string> GetContentAsStringAsync(this HttpResponseMessage httpResponseMessage)
    {
        return httpResponseMessage.Content.ReadAsStringAsync();
    }
}
