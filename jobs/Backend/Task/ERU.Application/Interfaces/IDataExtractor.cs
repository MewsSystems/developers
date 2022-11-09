using ERU.Application.DTOs;

namespace ERU.Application.Interfaces;

public interface IDataExtractor
{
	Task<IEnumerable<CnbExchangeRateResponse>> ExtractCnbData(IReadOnlyCollection<string> currencyCodes, CancellationToken token = default);
}