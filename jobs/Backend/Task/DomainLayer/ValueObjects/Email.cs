namespace DomainLayer.ValueObjects;

/// <summary>
/// Represents an email address as an immutable value object with validation.
/// </summary>
public record Email
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates an Email from a string value.
    /// </summary>
    /// <param name="email">The email address string</param>
    /// <returns>An Email value object</returns>
    /// <exception cref="ArgumentException">Thrown when the email is invalid</exception>
    public static Email From(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));

        var normalizedEmail = email.Trim().ToLowerInvariant();

        if (!IsValidEmail(normalizedEmail))
            throw new ArgumentException($"Invalid email format: '{email}'", nameof(email));

        return new Email(normalizedEmail);
    }

    /// <summary>
    /// Attempts to create an Email from a string without throwing exceptions.
    /// </summary>
    public static bool TryFrom(string email, out Email? result)
    {
        try
        {
            result = From(email);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    /// <summary>
    /// Validates an email address format.
    /// </summary>
    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;
}
