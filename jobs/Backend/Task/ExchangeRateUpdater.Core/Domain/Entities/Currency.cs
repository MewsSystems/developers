using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Domain.Entities
{
    public class Currency
    {
        private string _code;

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        
        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be a 3 letter code.")]
        public string Code 
        {
            get => _code;
            set => _code = value?.ToUpperInvariant();
        }

        public override string ToString()
        {
            return Code;
        }
    }
}
