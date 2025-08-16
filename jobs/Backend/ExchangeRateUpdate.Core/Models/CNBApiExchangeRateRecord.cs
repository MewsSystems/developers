namespace ExchangeRateUpdate.Core.Models;

public class CNBApiExchangeRateRecord
{
    public long? Amount { get; set; }
    public string Country { get; set; }
    public string Currency { get; set; }
    public string CurrencyCode { get; set; }
    public int? Order { get; set; }
    public decimal? Rate { get; set; }
    public DateTime? ValidFor { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append( "class ExRateDailyRest {\n" );
        sb.Append( "  Amount: " ).Append( Amount ).Append( "\n" );
        sb.Append( "  Country: " ).Append( Country ).Append( "\n" );
        sb.Append( "  Currency: " ).Append( Currency ).Append( "\n" );
        sb.Append( "  CurrencyCode: " ).Append( CurrencyCode ).Append( "\n" );
        sb.Append( "  Order: " ).Append( Order ).Append( "\n" );
        sb.Append( "  Rate: " ).Append( Rate ).Append( "\n" );
        sb.Append( "  ValidFor: " ).Append( ValidFor ).Append( "\n" );
        sb.Append( "}\n" );
        return sb.ToString();
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject( this, Newtonsoft.Json.Formatting.Indented );
    }
}
