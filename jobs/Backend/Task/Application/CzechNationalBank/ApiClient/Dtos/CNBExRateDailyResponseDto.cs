using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CzechNationalBank.ApiClient.Dtos
{
    public record CNBExRateDailyResponseDto
    {
        public CNBExRateDailyRestDto[] Rates { get; init; }
    }

    public record CNBExRateDailyRestDto
    {
        public int Amount { get; init; }
        public string Country { get; init; }
        public string Currency { get; init; }
        public string CurrencyCode { get; init; }
        public int Order { get; init; }
        public decimal Rate { get; init; }
        public DateTime ValidFor { get; init; }
    }
}
