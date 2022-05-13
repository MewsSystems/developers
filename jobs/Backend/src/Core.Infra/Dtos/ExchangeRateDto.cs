namespace Core.Infra.Dtos
{
    public class ExchangeRateDto
    {
        public ExchangeRateDto(string country, string currency, string baseAmount, string code, string rate)
        {
            Country = country;
            Currency = currency;
            BaseAmount = baseAmount;
            Code = code;
            Rate = rate;
        }

        public string Country { get; }
        public string Currency { get; }
        public string BaseAmount { get; }
        public string Code { get; }
        public string Rate { get; }
    }
}
