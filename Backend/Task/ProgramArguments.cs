using CommandLine;
using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    class ProgramArguments
    {
        [Option('d', "date", Required = false, HelpText = "Get exchange rates for specified date.")]
        public DateTime? Date { get; set; }

        [Option('c', "currencies", Required = false, HelpText = "Which currencies to extract (multiple argument).")]
        public IEnumerable<string> Currencies { get; set; }
    }
}
