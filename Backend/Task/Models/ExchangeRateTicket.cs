using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Models
{
    public class ExchangeRateTicket
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<ExchangeRateTicketItem> Items { get; set; }
    }
}