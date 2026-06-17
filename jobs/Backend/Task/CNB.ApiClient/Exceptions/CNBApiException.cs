namespace CNB.ApiClient.Exceptions;

public class CNBApiException(string message, Exception inner) : Exception(message, inner)
{
}
