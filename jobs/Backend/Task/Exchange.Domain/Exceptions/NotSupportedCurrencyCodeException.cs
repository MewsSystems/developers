using Exchange.Domain.Abstractions.Exceptions;

namespace Exchange.Domain.Exceptions;

public class NotSupportedCurrencyCodeException(string code)
    : BadRequestException($"Not supported currency code [{code}].");