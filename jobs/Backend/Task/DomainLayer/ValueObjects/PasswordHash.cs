namespace DomainLayer.ValueObjects;

/// <summary>
/// Represents a hashed password as an immutable value object.
/// Ensures that only hashed passwords are stored, never plain text.
/// </summary>
public record PasswordHash
{
    public string Value { get; }

    private PasswordHash(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a PasswordHash from a hashed string value.
    /// Note: This method assumes the password is already hashed.
    /// </summary>
    /// <param name="hashedPassword">The hashed password string</param>
    /// <returns>A PasswordHash value object</returns>
    /// <exception cref="ArgumentException">Thrown when the hash is invalid</exception>
    public static PasswordHash FromHash(string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword))
            throw new ArgumentException("Password hash cannot be null or empty.", nameof(hashedPassword));

        // Basic validation - most password hashes are at least 32 characters
        if (hashedPassword.Length < 32)
            throw new ArgumentException("Invalid password hash format.", nameof(hashedPassword));

        return new PasswordHash(hashedPassword);
    }

    /// <summary>
    /// Verifies if a plain text password matches this hash.
    /// Note: Actual implementation would depend on the hashing algorithm used (e.g., BCrypt, Argon2).
    /// </summary>
    public bool Verify(string plainTextPassword, Func<string, string, bool> verifyFunction)
    {
        if (string.IsNullOrWhiteSpace(plainTextPassword))
            return false;

        return verifyFunction(plainTextPassword, Value);
    }

    public override string ToString() => "***HASHED***"; // Security: Never expose the actual hash

    public static implicit operator string(PasswordHash hash) => hash.Value;
}
