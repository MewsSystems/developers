namespace ExchangeRateProviderAPI_PaolaRojas.Models.Options
{
    public class ApiKeyOptions
    {
        public const string SectionName = "Authentication:ApiKey";
        public string Key { get; set; } = string.Empty;
    }
}