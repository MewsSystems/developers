using Core.Services.CzechNationalBank.Interfaces;
using Data.Models;
using Data.Repositories.Interfaces;
using DateTimeExtensions;
using DateTimeExtensions.WorkingDays;

namespace Core.Services.CzechNationalBank;

public class CheckBankRateService : ICheckBankRateService
{
    private readonly IGenericRepository<CurrencyCzechRate> _repository;
    public CheckBankRateService(IGenericRepository<CurrencyCzechRate> repository)
    {
        _repository = repository;
    }
    public bool IsNeededCallCzechNationalBankRates()
    {
        if(HasDataDatabase())
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday || DateTime.Now.IsHoliday(new WorkingDayCultureInfo("cs-CZ")))
            {
                return false;
            }
            return DateTimeOffset.Now.Hour >= 14 && !IsDatabaseUpdated();
        }
        return true;       
    }

    private bool HasDataDatabase()
    {
        return _repository.LastElement() != null;
    }

    private bool IsDatabaseUpdated()
    {
        var entity = _repository.LastElement();
        if (entity == null)
            return false;
        if (entity.CreatedDate.Date != DateTime.Today)
            return false;
        return true;
    }
}
