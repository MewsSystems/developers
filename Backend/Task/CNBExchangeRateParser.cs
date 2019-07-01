using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExchangeRateUpdater
{
    public class CNBExchangeRateParser
    {
        private const char Delimitter = '|';

        public static ExchangeRateTicket ParseContent(string content)
        {
            if (string.IsNullOrEmpty(content)) throw new ArgumentException("Argument null or empty.", nameof(content));

            var lines = content.Split('\r', '\n');

            //first line is date with id, seond line is header
            if (lines.Length < 2) return null;

            var ticket = new ExchangeRateTicket(); ;

            var dateIdMatch = Regex.Match(lines[0], @"(.*?)\s*#\s*(\d+)");

            var dateGroup = dateIdMatch.Groups[0];
            if (dateGroup.Success)
            {
                if (DateTime.TryParse(dateGroup.Value, out var dt)) ticket.Date = dt;
            }

            var idGroup = dateIdMatch.Groups[1];
            if (idGroup.Success)
            {
                if (int.TryParse(idGroup.Value, out var id)) ticket.Id = id;
            }

            //first line is date with id, seond line is header
            var itemLines = lines.Skip(2).ToList();

            ticket.Items = ParseItems(itemLines).ToList();

            return ticket;
        }

        public static IEnumerable<ExchangeRateTicketItem> ParseItems(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;

                var splittedLine = line.Split(Delimitter);
                var amount = int.Parse(splittedLine[2]);
                var rate = decimal.Parse(splittedLine[4], CultureInfo.InvariantCulture);

                var item = new ExchangeRateTicketItem
                {
                    Country = splittedLine[0],
                    Currency = splittedLine[1],
                    Amount = amount,
                    Code = splittedLine[3],
                    Rate = rate
                };
                yield return item;
            }
        }
    }
}
