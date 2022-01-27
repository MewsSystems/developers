namespace ExchangeRateUpdater.DataTransferObjects
{
    public class CurrencyRateDto
    {
        public uint Ammount { get; set; }

        public string Code { get; set; }

        public string CountryName { get; set; }

        public string Name { get; set; }            

        public decimal Rate { get; set; }        
    }
}
