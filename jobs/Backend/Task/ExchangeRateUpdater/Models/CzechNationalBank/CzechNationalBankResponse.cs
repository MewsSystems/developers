using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Models.CzechNationalBankResponse;

namespace ExchangeRateUpdater.Models.CzechNationalBank;

public record Response(IEnumerable<RecordedRate> Rates);

public record EmptyResponse(): Response (Enumerable.Empty<RecordedRate>());