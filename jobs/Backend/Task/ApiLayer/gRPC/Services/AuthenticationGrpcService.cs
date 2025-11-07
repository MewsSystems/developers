using ApplicationLayer.Commands.Authentication.Login;
using ApplicationLayer.Commands.Authentication.Register;
using gRPC.Mappers;
using gRPC.Protos.Authentication;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace gRPC.Services;

/// <summary>
/// gRPC service for user authentication operations.
/// Wraps existing MediatR command handlers with gRPC interface.
/// </summary>
[AllowAnonymous] // Authentication endpoints should be accessible without auth
public class AuthenticationGrpcService : AuthenticationService.AuthenticationServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthenticationGrpcService> _logger;

    public AuthenticationGrpcService(
        IMediator mediator,
        ILogger<AuthenticationGrpcService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Authenticates a user and generates JWT token.
    /// </summary>
    public override async Task<LoginResponse> Login(
        LoginRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("Login request received for email: {Email}", request.Email);

        try
        {
            // Create command from proto request
            var command = new LoginCommand(request.Email, request.Password);

            // Execute via MediatR (reuses existing business logic!)
            var result = await _mediator.Send(command, context.CancellationToken);

            // Map result to proto response
            var response = AuthenticationMappers.ToProtoLoginResponse(result);

            if (response.Success)
            {
                _logger.LogInformation("Login successful for email: {Email}", request.Email);
            }
            else
            {
                _logger.LogWarning("Login failed for email: {Email}", request.Email);
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", request.Email);
            throw new RpcException(new Status(StatusCode.Internal, "An error occurred during login"));
        }
    }

    /// <summary>
    /// Registers a new user account.
    /// </summary>
    public override async Task<RegisterResponse> Register(
        RegisterRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("Registration request received for email: {Email}", request.Email);

        try
        {
            // Create command from proto request
            var command = new RegisterCommand(
                request.Email,
                request.Password,
                request.FullName,
                request.Role);

            // Execute via MediatR (reuses existing business logic!)
            var result = await _mediator.Send(command, context.CancellationToken);

            // Map result to proto response
            var response = AuthenticationMappers.ToProtoRegisterResponse(result);

            if (response.Success)
            {
                _logger.LogInformation("Registration successful for email: {Email}", request.Email);
            }
            else
            {
                _logger.LogWarning("Registration failed for email: {Email}", request.Email);
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for email: {Email}", request.Email);
            throw new RpcException(new Status(StatusCode.Internal, "An error occurred during registration"));
        }
    }
}
