using FluentValidation;

namespace ApplicationLayer.Queries.Users.GetUsersByRole;

public class GetUsersByRoleQueryValidator : AbstractValidator<GetUsersByRoleQuery>
{
    public GetUsersByRoleQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0.")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size cannot exceed 100.");

        RuleFor(x => x.Role)
            .IsInEnum()
            .WithMessage("Invalid user role.");
    }
}
