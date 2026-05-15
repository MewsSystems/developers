namespace CNB.Client.Exceptions
{
    public class JsonDeserializationException : Exception
    {
        public JsonDeserializationException(string message) : base(message)
        {
        }

        public JsonDeserializationException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
