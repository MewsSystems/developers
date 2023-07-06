using CnbServiceClient.DTOs;
using ExchangeEntities;

namespace ExchangeRateTool.Interfaces
{
	public interface IExrateFilterService
	{
        /// <summary>
        /// Filters the <see cref="Exrate"/> list with the <see cref="Currency"/> one based on the currency code.
        /// </summary>
        /// <param name="exrates">List of <see cref="Exrate"/> to filter.</param>
        /// <param name="currencies">List of the <see cref="Currency"/> to be filtered.</param>
        /// <returns><see cref="IEnumerable{T}">IEnumerable&lt;Exrate&gt;</see> with just the same elements present in the <paramref name="currencies"/> parameter.</returns>
        IEnumerable<Exrate> Filter(IEnumerable<Exrate> exrates, IEnumerable<Currency> currencies);
	}
}

