using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationLayer.Commands.Authentication.Login;
using REST.Response.Models.Common;
using REST.Response.Models.Areas.Authentication;
using REST.Response.Converters;
using REST.Request.Models.Areas.Authentication;

namespace REST.Areas.Authentication;

/// <summary>
/// API endpoints for authentication.
/// </summary>
[ApiController]
[Area("Authentication")]
[Route("api/auth")]
[AllowAnonymous]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Authenticate a user and generate JWT tokens.
    /// </summary>
    /// <param name="request">Login credentials</param>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthenticationResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<AuthenticationResponse>), 400)]
    [ProducesResponseType(typeof(ApiResponse<AuthenticationResponse>), 404)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var result = await _mediator.Send(command);

        var apiResponse = result.ToApiResponse(
            dto => dto.ToResponse(),
            "Login successful"
        );

        if (!apiResponse.Success)
        {
            return StatusCode(apiResponse.StatusCode, apiResponse);
        }

        return Ok(apiResponse);
    }
}
