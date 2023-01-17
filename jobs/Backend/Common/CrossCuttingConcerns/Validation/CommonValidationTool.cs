using FluentValidation;
using ValidationException = FluentValidation.ValidationException;

namespace Common.CrossCuttingConcerns.Validation
{
    public class CommonValidationTool
    {
        public static void Validate(IValidator validator,object entity)
        {
            var context = new ValidationContext<object>(entity);
           
            var result = validator.Validate(context);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}
