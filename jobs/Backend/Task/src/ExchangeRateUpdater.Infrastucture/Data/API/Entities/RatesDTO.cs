namespace ExchangeRateUpdater.Infrastucture.Data.API.Entities
{
    public class RatesDTO
    {
        public RatesDTO()
        {
            Rates = new List<ExchangeRateDTO>();
        }
        public IList<ExchangeRateDTO> Rates { get; set; }

    }
}
