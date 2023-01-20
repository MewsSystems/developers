namespace ExchangeRateUpdater.Providers.CzechNationalBank
{
	internal interface IDataParser
	{
		IEnumerable<DataRow> Parse(string data);
	}
}
