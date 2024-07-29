using Asp.Versioning;
using ExchangeRateUpdater.Core.Domain.Identity;
using ExchangeRateUpdater.Core.DTO.Auth;
using ExchangeRateUpdater.Core.ServiceContracts.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExchangeRateUpdater.WebAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AuthController : CustomBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IJwtService _jwtService;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Registers new users to the application
        /// </summary>
        /// <param name="registerUserRequest">Request object that holds the registration information about the new user.</param>
        /// <returns>Returns the Authentication Response inlcuding Access and Refresh Tokens.</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUser>> PostRegister(RegisterUserRequest registerUserRequest)
        {
            //Validation
            if (ModelState.IsValid == false)
            {
                string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return Problem(errorMessage);
            }

            //Create new user
            ApplicationUser user = new ApplicationUser()
            {
                Email = registerUserRequest.Email,
                PhoneNumber = registerUserRequest.PhoneNumber,
                UserName = registerUserRequest.Email,
                FirstName = registerUserRequest.FirstName,
                LastName = registerUserRequest.LastName,
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerUserRequest.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                AuthenticationResponse response = _jwtService.CreateJwtToken(user);

                user.RefreshToken = response.RefreshToken;
                user.RefreshTokenExpiration = response.RefreshTokenExpirationDateTime;
                await _userManager.UpdateAsync(user);

                return Ok(response);
            }
            else
            {
                string errorMessage = string.Join(" | ", result.Errors.Select(x => x.Description));
                return Problem(errorMessage);

            }
        }

        /// <summary>
        /// Checks to see if emails are unique within the identity database.  Used as part of data attribute validation on registration request
        /// </summary>
        /// <param name="email">Email to check</param>
        /// <returns>Returns true if available, false otherwise.</returns>
        [HttpGet]
        public async Task<IActionResult> IsEmailAvailable(string email)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }

        /// <summary>
        /// Logs a registered user into the application
        /// </summary>
        /// <param name="loginRequest">Request object containing username and password for the user.</param>
        /// <returns>Returns the Authentication Response inlcuding Access and Refresh Tokens.</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> PostLogin(LoginUserRequest loginRequest)
        {
            //Validation
            if (ModelState.IsValid == false)
            {
                string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return Problem(errorMessage);
            }

            var result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(loginRequest.Email);
                if (user == null)
                {
                    return NoContent();
                }
                AuthenticationResponse response = _jwtService.CreateJwtToken(user);

                user.RefreshToken = response.RefreshToken;
                user.RefreshTokenExpiration = response.RefreshTokenExpirationDateTime;
                await _userManager.UpdateAsync(user);

                return Ok(response);
            }
            else
            {
                return Problem("Invalid email and password");
            }
        }

        /// <summary>
        /// Logs a user out of the application
        /// </summary>
        /// <returns>No Content</returns>
        [HttpGet("logout")]
        public async Task<IActionResult> GetLogout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NoContent();

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            await _signInManager.SignOutAsync();


            return NoContent();
        }

        /// <summary>
        /// Generates a new access token based off of a refresh token, provided everything is valid
        /// </summary>
        /// <param name="request">Request object containing access token and refresh token.</param>
        /// <returns>Returns the Authentication Response inlcuding Access and Refresh Tokens.</returns>
        [HttpPost("generate-new-jwt-token")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateNewAccessToken(RefreshTokenRequest request)
        {
            if (request == null) return BadRequest("Invalid request");

            ClaimsPrincipal? principal = _jwtService.GetPrincipalFromJwtToken(request.Token);
            if (principal == null)
            {
                return BadRequest("Invalid JWT access token");
            }

            string? email = principal.FindFirstValue(ClaimTypes.Email);

            ApplicationUser? user = await _userManager.FindByEmailAsync(email);

            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiration <= DateTime.UtcNow)
            {
                return BadRequest("Invalid refresh token");
            }

            AuthenticationResponse response = _jwtService.CreateJwtToken(user);

            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpiration = response.RefreshTokenExpirationDateTime;
            await _userManager.UpdateAsync(user);

            return Ok(response);

        }
    }
}
