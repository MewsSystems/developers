namespace ExchangeRateUpdater.Providers.CzechNationalBank
{
	internal record DataRow(string Code, decimal Value)
	{
		internal static DataRow Parse(string[] parts)
		{
			var code = parts[3];
			var amount = int.Parse(parts[2], CultureInfo.InvariantCulture);
			var rate = decimal.Parse(parts[4], CultureInfo.InvariantCulture);

			return new DataRow(code, rate / amount);
		}
	}
}
