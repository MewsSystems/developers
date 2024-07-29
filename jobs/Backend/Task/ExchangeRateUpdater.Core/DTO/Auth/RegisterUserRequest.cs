using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.DTO.Auth
{
    public class RegisterUserRequest
    {

        [Required(ErrorMessage = "{0} cannot be blank")]
        [EmailAddress(ErrorMessage = "Email must be in a proper email format")]
        [Remote(action: "IsEmailAvailable", controller: "Auth", ErrorMessage = "{0} is already in use")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} cannot be blank")]
        [Display(Name = "First Name")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "{0} should be between {2} and {1} characters long")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} cannot be blank")]
        [Display(Name = "Last Name")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "{0} should be between {2} and {1} characters long")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} cannot be blank")]
        [Display(Name = "Phone Number")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "{0} should contain digits only")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} cannot be blank")]
        [Display(Name = "Password Confirmation")]
        [Compare(nameof(Password), ErrorMessage = "Password and Password Confirmation do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
