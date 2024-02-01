using System.ComponentModel.DataAnnotations;

namespace ApiClients.CzechNationalBank.Models;

public class CzechNationalBankApiOptions
{
    [Required]
    public string BaseUrl { get; set; }

    [Required]
    public string GetRates { get; set; }
}
