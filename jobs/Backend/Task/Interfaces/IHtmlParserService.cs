using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Interfaces;

public interface IHtmlParserService
{
    public Task<List<List<string>>> GetDataFromSource();
}