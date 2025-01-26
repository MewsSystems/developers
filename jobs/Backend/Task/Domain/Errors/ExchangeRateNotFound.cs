using Microsoft.Extensions.Logging;

namespace Domain.Errors;

public sealed class ExchangeRateNotFound : Base.NotFoundError
{
    private const string Error = "No data could be found for ExchangeRate : {0} => {1}";
    public ExchangeRateNotFound(ILogger logger, string source, string target) : base(string.Format(Error, source, target))
    {
        logger.LogError(string.Format(Error, source, target));
    }
}