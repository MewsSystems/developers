namespace ExchangeRateUpdater.Contracts.Responses;

public class Response
{
    public Response()
    {
        Result = true;
        Errors = new();
    }

    public bool Result { get; set; }
    public List<ResponseError> Errors { get; set; }
}
