using ApplicationLayer.Commands.Users.CreateUser;
using ApplicationLayer.Queries.Users.CheckEmailExists;
using DomainLayer.Enums;
using FluentAssertions;
using Integration.Infrastructure;

namespace Integration.Queries.Users;

public class CheckEmailExistsQueryTests : IntegrationTestBase
{
    private async Task<string> CreateTestUser(UserRole role = UserRole.Consumer)
    {
        var email = $"user_{Guid.NewGuid():N}@example.com";
        var command = new CreateUserCommand(
            Email: email,
            Password: "Test123!@#",
            FirstName: "John",
            LastName: "Doe",
            Role: role
        );

        await Mediator.Send(command);
        return email;
    }

    [Fact]
    public async Task CheckEmailExists_WithExistingEmail_ShouldReturnTrue()
    {
        // Arrange
        var email = await CreateTestUser();

        var query = new CheckEmailExistsQuery(email);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task CheckEmailExists_WithNonExistentEmail_ShouldReturnFalse()
    {
        // Arrange
        var query = new CheckEmailExistsQuery($"nonexistent_{Guid.NewGuid():N}@example.com");

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task CheckEmailExists_IsCaseInsensitive_ShouldReturnTrue()
    {
        // Arrange
        var email = await CreateTestUser();
        var upperCaseEmail = email.ToUpper();

        var query = new CheckEmailExistsQuery(upperCaseEmail);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().BeTrue("email check should be case-insensitive");
    }

    [Fact]
    public async Task CheckEmailExists_WithAdminEmail_ShouldReturnTrue()
    {
        // Arrange
        var email = await CreateTestUser(UserRole.Admin);

        var query = new CheckEmailExistsQuery(email);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task CheckEmailExists_WithInvalidEmail_ShouldFail()
    {
        // Arrange
        var query = new CheckEmailExistsQuery("invalid-email");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.ValidationException>(
            async () => await Mediator.Send(query)
        );

        exception.Errors.Should().ContainKey("Email");
    }

    [Fact]
    public async Task CheckEmailExists_WithMultipleUsers_ShouldCheckCorrectEmail()
    {
        // Arrange
        var email1 = await CreateTestUser();
        var email2 = await CreateTestUser();
        var email3 = await CreateTestUser();

        var query = new CheckEmailExistsQuery(email2);

        // Act
        var result = await Mediator.Send(query);

        // Assert
        result.Should().BeTrue();
    }
}
