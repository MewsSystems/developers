using FluentValidation;

namespace ApplicationLayer.Queries.Users.CheckEmailExists;

public class CheckEmailExistsQueryValidator : AbstractValidator<CheckEmailExistsQuery>
{
    public CheckEmailExistsQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");
    }
}
