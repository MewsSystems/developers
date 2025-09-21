namespace Exchange.Domain.Abstractions.Exceptions;

public abstract class BadRequestException(string? message) : Exception(message);