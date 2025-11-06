using FluentValidation;

namespace ApplicationLayer.Queries.Users.GetCurrentUser;

public class GetCurrentUserQueryValidator : AbstractValidator<GetCurrentUserQuery>
{
    public GetCurrentUserQueryValidator()
    {
        RuleFor(x => x.CurrentUserId)
            .GreaterThan(0).WithMessage("User ID must be positive.");
    }
}
