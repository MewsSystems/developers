using FluentValidation;

namespace ApplicationLayer.Commands.Users.UpdateUserInfo;

public class UpdateUserInfoCommandValidator : AbstractValidator<UpdateUserInfoCommand>
{
    public UpdateUserInfoCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User ID must be positive.");

        RuleFor(x => x.Email)
            .MaximumLength(256).WithMessage("Email must not exceed 256 characters.")
            .EmailAddress().WithMessage("Email must be a valid email address.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");
    }
}
