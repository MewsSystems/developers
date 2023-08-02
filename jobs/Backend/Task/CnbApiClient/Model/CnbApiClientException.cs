namespace CnbApiClient.Model;

public class CnbApiClientException : Exception
{
    public CnbApiClientException(string message, Exception inner)
        : base(message, inner)
    {
    }
}