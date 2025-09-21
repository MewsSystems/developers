using Exchange.Domain.Abstractions.Exceptions;

namespace Exchange.Domain.Exceptions;

public class InvalidCurrencyCodeException(string code) : BadRequestException($"Invalid currency code [{code}].");