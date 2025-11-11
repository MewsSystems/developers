namespace DomainLayer.ValueObjects;

/// <summary>
/// Represents a reference to an API key stored in a secure vault.
/// This is not the actual key, but a reference identifier to retrieve it.
/// </summary>
public record ApiKeyReference
{
    public string Reference { get; }

    private ApiKeyReference(string reference)
    {
        Reference = reference;
    }

    /// <summary>
    /// Creates an ApiKeyReference from a reference string.
    /// </summary>
    /// <param name="reference">The vault reference identifier</param>
    /// <returns>An ApiKeyReference value object</returns>
    /// <exception cref="ArgumentException">Thrown when the reference is invalid</exception>
    public static ApiKeyReference From(string reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
            throw new ArgumentException("API key reference cannot be null or empty.", nameof(reference));

        var trimmedReference = reference.Trim();

        if (trimmedReference.Length < 3)
            throw new ArgumentException("API key reference must be at least 3 characters.", nameof(reference));

        return new ApiKeyReference(trimmedReference);
    }

    /// <summary>
    /// Attempts to create an ApiKeyReference without throwing exceptions.
    /// </summary>
    public static bool TryFrom(string reference, out ApiKeyReference? result)
    {
        try
        {
            result = From(reference);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public override string ToString() => Reference;

    public static implicit operator string(ApiKeyReference reference) => reference.Reference;
}
