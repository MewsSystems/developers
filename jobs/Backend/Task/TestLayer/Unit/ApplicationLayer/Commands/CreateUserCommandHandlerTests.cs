using ApplicationLayer.Commands.Users.CreateUser;
using DomainLayer.Aggregates.UserAggregate;
using DomainLayer.Enums;
using FluentAssertions;
using Moq;

namespace Unit.ApplicationLayer.Commands;

/// <summary>
/// Unit tests for CreateUserCommandHandler.
/// Tests user creation with password hashing and email uniqueness validation.
/// </summary>
public class CreateUserCommandHandlerTests : TestBase
{
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _handler = new CreateUserCommandHandler(
            MockUnitOfWork.Object,
            MockPasswordHasher.Object,
            CreateMockLogger<CreateUserCommandHandler>().Object);
    }

    [Fact]
    public async Task Handle_WithValidNewUser_ShouldCreateUserSuccessfully()
    {
        // Arrange
        var command = new CreateUserCommand(
            Email: "john.doe@example.com",
            Password: "SecureP@ssw0rd",
            FirstName: "John",
            LastName: "Doe",
            Role: UserRole.Consumer);

        // Create a user to be returned by the second GetByEmailAsync call
        var savedUser = User.Create("john.doe@example.com", "hashed_password_12345", "John", "Doe", UserRole.Consumer);
        typeof(User).GetProperty("Id")!.SetValue(savedUser, 100);

        // Mock: GetByEmailAsync called twice - first check (null), then to retrieve saved user (with ID)
        MockUserRepository
            .SetupSequence(x => x.GetByEmailAsync("john.doe@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null)  // First call: email doesn't exist
            .ReturnsAsync(savedUser);   // Second call: return saved user with ID

        // Mock: Password hasher
        MockPasswordHasher
            .Setup(x => x.HashPassword("SecureP@ssw0rd"))
            .Returns("hashed_password_12345");

        // Mock: Add and SaveChanges
        MockUserRepository
            .Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(100);

        // Verify password was hashed
        MockPasswordHasher.Verify(
            x => x.HashPassword("SecureP@ssw0rd"),
            Times.Once);

        // Verify user was added with hashed password
        MockUserRepository.Verify(
            x => x.AddAsync(It.Is<User>(u =>
                u.Email == "john.doe@example.com" &&
                u.FirstName == "John" &&
                u.LastName == "Doe" &&
                u.Role == UserRole.Consumer),
                It.IsAny<CancellationToken>()),
            Times.Once);

        VerifySaveChangesCalled();
    }

    [Fact]
    public async Task Handle_WithExistingEmail_ShouldReturnFailure()
    {
        // Arrange
        var command = new CreateUserCommand(
            Email: "existing@example.com",
            Password: "Password123",
            FirstName: "Jane",
            LastName: "Smith",
            Role: UserRole.Admin);

        var existingUser = User.Create(
            "existing@example.com",
            "some_hash",
            "Existing",
            "User",
            UserRole.Consumer);

        // Mock: Email already exists
        MockUserRepository
            .Setup(x => x.GetByEmailAsync("existing@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("already exists");

        // Verify password hasher was never called
        MockPasswordHasher.Verify(
            x => x.HashPassword(It.IsAny<string>()),
            Times.Never);

        // Verify AddAsync was never called
        MockUserRepository.Verify(
            x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
            Times.Never);

        VerifySaveChangesNotCalled();
    }

    [Theory]
    [InlineData("", "Password123", "John", "Doe")] // Empty email
    [InlineData("invalidemail", "Password123", "John", "Doe")] // Invalid email format
    public async Task Handle_WithInvalidEmail_ShouldReturnFailure(
        string email,
        string password,
        string firstName,
        string lastName)
    {
        // Arrange
        var command = new CreateUserCommand(
            Email: email,
            Password: password,
            FirstName: firstName,
            LastName: lastName,
            Role: UserRole.Consumer);

        // Mock: Email doesn't exist
        MockUserRepository
            .Setup(x => x.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        MockPasswordHasher
            .Setup(x => x.HashPassword(password))
            .Returns("hashed_password");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeEmpty();

        VerifySaveChangesNotCalled();
    }

    [Theory]
    [InlineData(UserRole.Consumer)]
    [InlineData(UserRole.Admin)]
    public async Task Handle_WithDifferentRoles_ShouldCreateUserWithCorrectRole(UserRole role)
    {
        // Arrange
        var command = new CreateUserCommand(
            Email: "user@example.com",
            Password: "Password123",
            FirstName: "Test",
            LastName: "User",
            Role: role);

        MockUserRepository
            .Setup(x => x.GetByEmailAsync("user@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        MockPasswordHasher
            .Setup(x => x.HashPassword("Password123"))
            .Returns("hashed_password");

        MockUserRepository
            .Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Callback<User, CancellationToken>((user, ct) =>
            {
                typeof(User).GetProperty("Id")!.SetValue(user, 1);
            })
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify user was created with correct role
        MockUserRepository.Verify(
            x => x.AddAsync(It.Is<User>(u => u.Role == role), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldReturnFailure()
    {
        // Arrange
        var command = new CreateUserCommand(
            Email: "test@example.com",
            Password: "Password123",
            FirstName: "Test",
            LastName: "User",
            Role: UserRole.Consumer);

        MockUserRepository
            .Setup(x => x.GetByEmailAsync("test@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        MockPasswordHasher
            .Setup(x => x.HashPassword("Password123"))
            .Returns("hashed_password");

        MockUserRepository
            .Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Failed to create user");

        VerifySaveChangesNotCalled();
    }

    [Fact]
    public async Task Handle_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        var command = new CreateUserCommand(
            Email: "test@example.com",
            Password: "Password123",
            FirstName: "Test",
            LastName: "User",
            Role: UserRole.Consumer);

        var cts = new CancellationTokenSource();
        var token = cts.Token;

        var savedUser = User.Create("test@example.com", "hashed_password", "Test", "User", UserRole.Consumer);
        typeof(User).GetProperty("Id")!.SetValue(savedUser, 1);

        MockUserRepository
            .SetupSequence(x => x.GetByEmailAsync("test@example.com", token))
            .ReturnsAsync((User?)null)  // First call: email doesn't exist
            .ReturnsAsync(savedUser);   // Second call: return saved user with ID

        MockPasswordHasher
            .Setup(x => x.HashPassword("Password123"))
            .Returns("hashed_password");

        MockUserRepository
            .Setup(x => x.AddAsync(It.IsAny<User>(), token))
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(token))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, token);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify token was passed through - GetByEmailAsync called twice (check + retrieve)
        MockUserRepository.Verify(
            x => x.GetByEmailAsync("test@example.com", token),
            Times.Exactly(2));

        MockUserRepository.Verify(
            x => x.AddAsync(It.IsAny<User>(), token),
            Times.Once);

        MockUnitOfWork.Verify(
            x => x.SaveChangesAsync(token),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldHashPasswordBeforeStoring()
    {
        // Arrange
        var plainPassword = "MyPlainPassword123!";
        var hashedPassword = "super_secure_hash_xyz";

        var command = new CreateUserCommand(
            Email: "secure@example.com",
            Password: plainPassword,
            FirstName: "Secure",
            LastName: "User",
            Role: UserRole.Consumer);

        MockUserRepository
            .Setup(x => x.GetByEmailAsync("secure@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        MockPasswordHasher
            .Setup(x => x.HashPassword(plainPassword))
            .Returns(hashedPassword);

        User? capturedUser = null;
        MockUserRepository
            .Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Callback<User, CancellationToken>((user, ct) =>
            {
                capturedUser = user;
                typeof(User).GetProperty("Id")!.SetValue(user, 1);
            })
            .Returns(Task.CompletedTask);

        MockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify the password hasher was called with the plain password
        MockPasswordHasher.Verify(
            x => x.HashPassword(plainPassword),
            Times.Once);

        // Note: We cannot directly verify the stored password hash because
        // User aggregate doesn't expose PasswordHash publicly (good design!)
        // But we can verify the hasher was called and the user was stored
        capturedUser.Should().NotBeNull();
    }

    [Theory]
    [InlineData("", "Last")]  // Empty first name
    [InlineData("First", "")]  // Empty last name
    public async Task Handle_WithInvalidNames_ShouldReturnFailure(
        string firstName,
        string lastName)
    {
        // Arrange
        var command = new CreateUserCommand(
            Email: "valid@example.com",
            Password: "Password123",
            FirstName: firstName,
            LastName: lastName,
            Role: UserRole.Consumer);

        MockUserRepository
            .Setup(x => x.GetByEmailAsync("valid@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        MockPasswordHasher
            .Setup(x => x.HashPassword("Password123"))
            .Returns("hashed_password");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeEmpty();

        VerifySaveChangesNotCalled();
    }
}
