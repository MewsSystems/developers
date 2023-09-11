using Mews.ERP.AppService.Data.Models;
using Mews.ERP.AppService.Data.Repositories;
using Mews.ERP.AppService.Data.Repositories.Interfaces;
using Mews.ERP.AppService.Features.Fetch.Models;

namespace Mews.ERP.AppService.Features.Fetch.Repositories.Interfaces;

public interface ICurrenciesRepository : IReadOnlyRepository<Currency>
{
    
}