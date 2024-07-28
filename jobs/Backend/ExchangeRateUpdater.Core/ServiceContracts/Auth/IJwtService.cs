using ExchangeRateUpdater.Core.Domain.Identity;
using ExchangeRateUpdater.Core.DTO.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.ServiceContracts.Auth
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtToken(ApplicationUser user);

        ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
    }
}
