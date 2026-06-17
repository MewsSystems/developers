namespace ExchangeRateUpdate.Core.Models;

public class CNBApiDailyExchangeRateResponse
{
    public List<CNBApiExchangeRateRecord> Rates { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append( "class ExRateDailyResponse {\n" );
        sb.Append( "  Rates: " ).Append( Rates ).Append( "\n" );
        sb.Append( "}\n" );
        return sb.ToString();
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject( this, Newtonsoft.Json.Formatting.Indented );
    }

}
