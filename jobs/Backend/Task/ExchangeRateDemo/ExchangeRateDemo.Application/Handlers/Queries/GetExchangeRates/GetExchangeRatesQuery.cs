namespace ExchangeRateDemo.Application.Handlers.Queries.GetExchangeRates
{
    public class GetExchangeRatesQuery(List<string>? isoCodes = null, string? date = null) : IRequest<IEnumerable<GetExchangeRatesResponse>>
    {
        public string Date { get; } = ValidDateInput(date);
        public List<string>? IsoCodes { get; } = isoCodes;

        private static string ValidDateInput(string? dateInput)
        {
            if (DateTime.TryParse(dateInput, out var validDate))
            {
                return validDate.ToString("yyyy-MM-dd");
            };

            return DateTime.Now.ToString("yyyy-MM-dd");
        }
    }
}
