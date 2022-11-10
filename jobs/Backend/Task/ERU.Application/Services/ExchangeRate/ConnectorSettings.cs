namespace ERU.Application.Services.ExchangeRate;

public record ConnectorSettings
{
	public string[] FileUri { get; set; } = Array.Empty<string>();
	public string SourceCurrency { get; set; } = "CZK";
	public string CultureInfo { get; set; } = "";
	public short DataSkipLines { get; set; }
	public short AmountIndex { get; set; }
	public short CodeIndex { get; set; }
	public short RateIndex { get; set; }
}