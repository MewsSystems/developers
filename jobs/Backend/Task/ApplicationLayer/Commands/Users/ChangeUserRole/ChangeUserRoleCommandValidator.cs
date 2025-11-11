using FluentValidation;

namespace ApplicationLayer.Commands.Users.ChangeUserRole;

public class ChangeUserRoleCommandValidator : AbstractValidator<ChangeUserRoleCommand>
{
    public ChangeUserRoleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User ID must be positive.");

        RuleFor(x => x.NewRole)
            .IsInEnum().WithMessage("Invalid role specified.");
    }
}
