using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Application.Validators
{
    public static class DateValidator
    {
        /// <summary>
        /// Validates a string to ensure it is a valid date in the format (yyyy-MM-dd) and is not a future date
        /// </summary>
        /// <param name="input">The date string to validate.</param>
        /// <returns>
        /// True if the input is a valid date, false otherwise
        /// </returns>
        public static bool ValidateDate(string input)
        {
            DateTime date;

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("No date entered.");
                return false;
            }

            if (!DateTime.TryParseExact(input, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
            {
                Console.WriteLine("Invalid date format. Please enter date as yyyy-MM-dd.");
                return false;
            }

            if (date > DateTime.Today)
            {
                Console.WriteLine("Date cannot be in the future. Please enter a valid past or current date.");
                return false;
            }

            return true;
        }
    }
}
