using System.ComponentModel.DataAnnotations;

namespace Mews.ExchangeRate.API.Dtos;

public record class Currency(string Code) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        if (string.IsNullOrWhiteSpace(Code))
        {
            results.Add(new ValidationResult($"Parameter {nameof(Code)} cannot be null or white space"));
        }
        else if (Code.Length != 3 || !Code.All(char.IsLetter))
        {
            results.Add(new ValidationResult($"Parameter {nameof(Code)} with value {Code} does not have a valid ISO 4217 format"));
        }

        return results;
    }
}
