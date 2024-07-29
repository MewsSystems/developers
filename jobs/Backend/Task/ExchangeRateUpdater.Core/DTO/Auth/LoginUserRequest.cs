using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.DTO.Auth
{
    public class LoginUserRequest
    {
        [Required(ErrorMessage = "Email cannot be blank")]
        [EmailAddress(ErrorMessage = "Email should be in proper format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password cannot be blank")]
        public string Password { get; set; } = string.Empty;
    }
}
