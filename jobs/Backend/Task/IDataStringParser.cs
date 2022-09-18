namespace ExchangeRateUpdater
{
	internal interface IDataStringParser<T>
	{
		T Parse(string input);
	}
}
