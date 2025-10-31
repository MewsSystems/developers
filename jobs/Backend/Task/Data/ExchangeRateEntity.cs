using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchangeRateUpdater.Data;

[Table("exchange_rates")]
public class ExchangeRateEntity
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Column("source_currency")]
    [MaxLength(3)]
    public string SourceCurrency { get; set; }

    [Column("target_currency")]
    [MaxLength(3)]
    public string TargetCurrency { get; set; }

    [Column("rate")] public decimal Rate { get; set; }

    [Column("date")] public DateTime Date { get; set; }
}