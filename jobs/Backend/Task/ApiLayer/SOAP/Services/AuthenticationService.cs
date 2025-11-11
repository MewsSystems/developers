using ApplicationLayer.Commands.Authentication.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace SOAP.Services;

/// <summary>
/// SOAP service implementation for authentication operations.
/// Reuses ApplicationLayer commands via MediatR.
/// </summary>
[AllowAnonymous] // Login doesn't require authentication
public class AuthenticationService : IAuthenticationService
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(
        IMediator mediator,
        ILogger<AuthenticationService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: Login called for email: {Email}", request.Email);

            // Reuse existing ApplicationLayer command
            var command = new LoginCommand(request.Email, request.Password);
            var result = await _mediator.Send(command);

            if (result.IsSuccess && result.Value != null)
            {
                return new LoginResponse
                {
                    Success = true,
                    Message = "Login successful",
                    Data = new AuthenticationDataSoap
                    {
                        UserId = result.Value.UserId,
                        Email = result.Value.Email,
                        FirstName = result.Value.FirstName,
                        LastName = result.Value.LastName,
                        Role = result.Value.Role,
                        AccessToken = result.Value.AccessToken,
                        RefreshToken = result.Value.RefreshToken,
                        ExpiresAt = result.Value.ExpiresAt
                    }
                };
            }

            return new LoginResponse
            {
                Success = false,
                Message = result.Error ?? "Login failed",
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "Authentication failed",
                    Detail = result.Error
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Login SOAP operation");

            return new LoginResponse
            {
                Success = false,
                Message = "An error occurred during login",
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }
}
