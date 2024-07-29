using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Domain.Entities
{
    public class CurrencySource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Currency Code is required.")]
        [StringLength(3)]
        public string? CurrencyCode { get; set; }

        [Required(ErrorMessage = "Source URL is required.")]
        [StringLength(250)]
        public string? SourceUrl { get; set; }
    }
}
