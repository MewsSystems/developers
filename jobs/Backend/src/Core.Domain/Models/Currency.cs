using CSharpFunctionalExtensions;

namespace Core.Domain.Models
{
    public record Currency(string Code)
    {
        public static Result<Currency> Create(string code)
        {
            if (string.IsNullOrWhiteSpace(code) || code.Length != 3)
            {
                return Result.Failure<Currency>("Currency code is not valid.");
            }

            return Result.Success(new Currency(code));
        }

        public override string ToString()
        {
            return Code;
        }
    }
}
