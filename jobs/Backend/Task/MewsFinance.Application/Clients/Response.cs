namespace MewsFinance.Application.Clients
{
    public class Response<T>
    {
        public Response(
            T data,
            bool isSuccess,
            string message)
        {
            Data = data;
            IsSuccess = isSuccess;
            Message = message;
        }

        public T Data { get; }
        public bool IsSuccess { get; }
        public string Message { get; } = string.Empty;
    }
}
