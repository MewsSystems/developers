using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories;

public class CurrencyRepository : Repository<Currency>, ICurrencyRepository
{
    public CurrencyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Currency?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Code == code, cancellationToken);
    }

    public async Task<IEnumerable<Currency>> GetAllCurrenciesOrderedAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .OrderBy(c => c.Code)
            .ToListAsync(cancellationToken);
    }
}
