using DomainLayer.Interfaces.Services;

namespace InfrastructureLayer.Services;

/// <summary>
/// Password hasher using BCrypt algorithm.
/// BCrypt is a secure, adaptive hashing function designed for password hashing.
/// </summary>
public class BCryptPasswordHasher : IPasswordHasher
{
    /// <summary>
    /// Hashes a plain text password using BCrypt with default work factor (11).
    /// </summary>
    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));

        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// Verifies a password against a BCrypt hash.
    /// </summary>
    public bool VerifyPassword(string password, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        if (string.IsNullOrWhiteSpace(passwordHash))
            return false;

        try
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
        catch
        {
            // Invalid hash format or other BCrypt errors
            return false;
        }
    }
}
