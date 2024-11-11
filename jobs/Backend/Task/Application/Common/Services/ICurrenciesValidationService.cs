using Application.Common.Models;

namespace Application.Common.Services
{
    public interface ICurrenciesValidationService
    {
        void ValidateAndLogWarning(IEnumerable<Currency> currencies);
    }
}