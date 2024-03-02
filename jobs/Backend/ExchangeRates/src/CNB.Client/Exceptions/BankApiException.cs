namespace CNB.Client.Exceptions
{
    public class BankApiException : Exception
    {
        public BankApiException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}