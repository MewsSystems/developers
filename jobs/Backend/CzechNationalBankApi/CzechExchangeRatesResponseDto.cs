namespace CzechNationalBankApi
{
    public class CzechExchangeRatesResponseDto
    {
        public List<CzechExchangeItemDto> Rates { get; set; } = new List<CzechExchangeItemDto>();
    }
}
