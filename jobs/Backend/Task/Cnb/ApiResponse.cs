namespace ExchangeRateUpdater.Cnb
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public ApiErrorResponse Error { get; set; }
    }
}
