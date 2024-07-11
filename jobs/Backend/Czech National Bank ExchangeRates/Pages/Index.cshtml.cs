using Czech_National_Bank_ExchangeRates.Models;
using ExchangeRateUpdater;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Czech_National_Bank_ExchangeRates.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IExchangeRateProvider _exchangeRateProvider;

        public IndexModel(ILogger<IndexModel> logger, IExchangeRateProvider exchangeRateProvider)
        {
            _logger = logger;
            _exchangeRateProvider = exchangeRateProvider;
        }

        [BindProperty]
        public DateTime DateString { get; set; } = DateTime.Now;

        public ExchangeRates CNBRates { get; set; }

        public async Task OnGetAsync()
        {
            CNBRates = await _exchangeRateProvider.GetExchangeRatesByDate(DateString.ToString("yyyy-MM-dd"));
        }
    }
}