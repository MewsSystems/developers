using DomainLayer.Common;
using DomainLayer.Enums;
using DomainLayer.Events.UserAggregate;

namespace DomainLayer.Aggregates.UserAggregate;

/// <summary>
/// Aggregate root representing a system user.
/// Encapsulates user authentication and authorization logic.
/// </summary>
public class User : AggregateRoot<int>
{
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset Created { get; private set; }
    public DateTimeOffset? Modified { get; private set; }
    public DateTimeOffset? LastLogin { get; private set; }

    /// <summary>
    /// Gets the user's full name.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

    // EF Core constructor
    private User()
    {
        Email = string.Empty;
        PasswordHash = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
    }

    private User(
        string email,
        string passwordHash,
        string firstName,
        string lastName,
        UserRole role)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));

        if (!IsValidEmail(email))
            throw new ArgumentException("Email format is invalid.", nameof(email));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash cannot be null or empty.", nameof(passwordHash));

        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));

        Email = email.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Role = role;
        IsActive = true;
        Created = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    public static User Create(
        string email,
        string passwordHash,
        string firstName,
        string lastName,
        UserRole role)
    {
        var user = new User(email, passwordHash, firstName, lastName, role);

        user.AddDomainEvent(new UserCreatedEvent(
            user.Id,
            user.Email,
            user.FullName,
            user.Role,
            user.Created));

        return user;
    }

    /// <summary>
    /// Reconstructs a User aggregate from persistence without validation or domain events.
    /// For use by infrastructure layer only when loading from database.
    /// </summary>
    public static User Reconstruct(
        int id,
        string email,
        string passwordHash,
        string firstName,
        string lastName,
        UserRole role,
        bool isActive,
        DateTimeOffset created,
        DateTimeOffset? modified,
        DateTimeOffset? lastLogin)
    {
        return new User
        {
            Id = id,
            Email = email,
            PasswordHash = passwordHash,
            FirstName = firstName,
            LastName = lastName,
            Role = role,
            IsActive = isActive,
            Created = created,
            Modified = modified,
            LastLogin = lastLogin
        };
    }

    /// <summary>
    /// Updates the user's basic information.
    /// </summary>
    public void UpdateInfo(string firstName, string lastName, string email)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));

        if (!IsValidEmail(email))
            throw new ArgumentException("Email format is invalid.", nameof(email));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Email = email.Trim().ToLowerInvariant();
        Modified = DateTimeOffset.UtcNow;

        AddDomainEvent(new UserInfoUpdatedEvent(Id, Email, FullName, DateTimeOffset.UtcNow));
    }

    /// <summary>
    /// Changes the user's password.
    /// </summary>
    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("Password hash cannot be null or empty.", nameof(newPasswordHash));

        PasswordHash = newPasswordHash;
        Modified = DateTimeOffset.UtcNow;

        AddDomainEvent(new UserPasswordChangedEvent(Id, Email, DateTimeOffset.UtcNow));
    }

    /// <summary>
    /// Changes the user's role.
    /// </summary>
    public void ChangeRole(UserRole newRole)
    {
        if (Role == newRole)
            return;

        var oldRole = Role;
        Role = newRole;
        Modified = DateTimeOffset.UtcNow;

        AddDomainEvent(new UserRoleChangedEvent(Id, Email, oldRole, newRole, DateTimeOffset.UtcNow));
    }

    /// <summary>
    /// Activates the user account.
    /// </summary>
    public void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;
        Modified = DateTimeOffset.UtcNow;

        AddDomainEvent(new UserActivatedEvent(Id, Email, DateTimeOffset.UtcNow));
    }

    /// <summary>
    /// Deactivates the user account.
    /// </summary>
    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;
        Modified = DateTimeOffset.UtcNow;

        AddDomainEvent(new UserDeactivatedEvent(Id, Email, DateTimeOffset.UtcNow));
    }

    /// <summary>
    /// Records a successful login.
    /// </summary>
    public void RecordLogin()
    {
        LastLogin = DateTimeOffset.UtcNow;
        Modified = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Checks if the user has a specific role.
    /// </summary>
    public bool HasRole(UserRole role) => Role == role;

    /// <summary>
    /// Checks if the user is an administrator.
    /// </summary>
    public bool IsAdministrator => Role == UserRole.Admin;

    /// <summary>
    /// Basic email validation.
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
}
