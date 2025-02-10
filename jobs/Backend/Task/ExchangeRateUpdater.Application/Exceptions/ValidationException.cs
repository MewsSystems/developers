using System.Collections.Generic;

namespace ExchangeRateUpdater.Application.Exceptions;

/// <summary>
/// Represents an exception that occurs when validation fails.
/// </summary>
/// <param name="errorsDictionary">A dictionary containing validation errors, where the key is the field name, and the value is an array of error messages.</param>
public sealed class ValidationException(IReadOnlyDictionary<string, string[]> errorsDictionary)
    : ApplicationException("Validation Failure", "One or more validation errors occurred")
{
    /// <summary>
    /// Gets a dictionary containing validation errors.
    /// </summary>
    /// <value>
    /// A dictionary where the key represents the field name, and the value is an array of error messages.
    /// </value>
    public IReadOnlyDictionary<string, string[]> ErrorsDictionary { get; } = errorsDictionary;
}
