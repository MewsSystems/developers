namespace ExchangeRateUpdater.Contracts.Extensions;

public static class ResponseExtensions
{
    public static Response ToResponse(this FluentValidation.Results.ValidationResult result) 
    {
        return new Response
        {
            Result = result.IsValid,
            Errors = result.Errors
                            .Select(e => new ResponseError { Name = $"code: {e.AttemptedValue}", Error = e.ErrorMessage })
                            .ToList()
        };
    }
}
