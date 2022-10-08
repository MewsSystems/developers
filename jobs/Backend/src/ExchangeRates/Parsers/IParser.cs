using ExchangeRates.Contracts;

namespace ExchangeRates.Parsers
{
	public interface IParser<TInputData>
	{
		ExchangeRate[] ParserData(TInputData data);
	}
}
