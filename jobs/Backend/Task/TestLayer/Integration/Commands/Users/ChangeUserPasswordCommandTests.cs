using ApplicationLayer.Commands.Users.ChangeUserPassword;
using ApplicationLayer.Commands.Users.CreateUser;
using DomainLayer.Enums;
using FluentAssertions;
using Integration.Infrastructure;

namespace Integration.Commands.Users;

public class ChangeUserPasswordCommandTests : IntegrationTestBase
{
    private const string DefaultPassword = "Test123!@#";

    private async Task<int> CreateTestUser()
    {
        var command = new CreateUserCommand(
            Email: $"user_{Guid.NewGuid():N}@example.com",
            Password: DefaultPassword,
            FirstName: "John",
            LastName: "Doe",
            Role: UserRole.Consumer
        );

        var result = await Mediator.Send(command);
        return result.Value;
    }

    [Fact]
    public async Task ChangeUserPassword_WithCorrectCurrentPassword_ShouldSucceed()
    {
        // Arrange
        var userId = await CreateTestUser();
        var newPassword = "NewPassword456!@#";

        var command = new ChangeUserPasswordCommand(
            UserId: userId,
            CurrentPassword: DefaultPassword,
            NewPassword: newPassword
        );

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify we can change password again using the new password
        var verifyCommand = new ChangeUserPasswordCommand(
            UserId: userId,
            CurrentPassword: newPassword,
            NewPassword: "AnotherPassword789!@#"
        );

        var verifyResult = await Mediator.Send(verifyCommand);
        verifyResult.IsSuccess.Should().BeTrue("the new password should work as current password");
    }

    [Fact]
    public async Task ChangeUserPassword_WithIncorrectCurrentPassword_ShouldFail()
    {
        // Arrange
        var userId = await CreateTestUser();

        var command = new ChangeUserPasswordCommand(
            UserId: userId,
            CurrentPassword: "WrongPassword123!@#",
            NewPassword: "NewPassword456!@#"
        );

        // Act
        var result = await Mediator.Send(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("incorrect");
    }

    [Fact]
    public async Task ChangeUserPassword_WithNonExistentUser_ShouldFail()
    {
        // Arrange
        var command = new ChangeUserPasswordCommand(
            UserId: 999,
            CurrentPassword: DefaultPassword,
            NewPassword: "NewPassword456!@#"
        );

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.NotFoundException>(
            async () => await Mediator.Send(command)
        );
    }

    [Fact]
    public async Task ChangeUserPassword_WithWeakNewPassword_ShouldFail()
    {
        // Arrange
        var userId = await CreateTestUser();

        var command = new ChangeUserPasswordCommand(
            UserId: userId,
            CurrentPassword: DefaultPassword,
            NewPassword: "weak"
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.ValidationException>(
            async () => await Mediator.Send(command)
        );

        exception.Errors.Should().ContainKey("NewPassword");
    }

    [Fact]
    public async Task ChangeUserPassword_WithSamePassword_ShouldFail()
    {
        // Arrange
        var userId = await CreateTestUser();

        var command = new ChangeUserPasswordCommand(
            UserId: userId,
            CurrentPassword: DefaultPassword,
            NewPassword: DefaultPassword
        );

        // Act & Assert - Should fail because new password must be different
        var exception = await Assert.ThrowsAsync<ApplicationLayer.Common.Exceptions.ValidationException>(
            async () => await Mediator.Send(command)
        );

        exception.Errors.Should().ContainKey("NewPassword");
    }
}
