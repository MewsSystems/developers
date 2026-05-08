using Microsoft.Extensions.Logging;

namespace Domain.Errors;

public class CurrencyNotFound : Base.NotFoundError
{
    private const string Error = "No data could be found for Currency : {0}";
    public CurrencyNotFound(ILogger logger, string id) : base(string.Format(Error, id))
    {
        logger.LogError(string.Format(Error, id));
    }
}