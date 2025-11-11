using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Users;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.Users.GetUserByEmail;

public class GetUserByEmailQueryHandler : IQueryHandler<GetUserByEmailQuery, UserDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetUserByEmailQueryHandler> _logger;

    public GetUserByEmailQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetUserByEmailQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<UserDto?> Handle(
        GetUserByEmailQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Getting user by email: {Email}", request.Email);

        var user = await _unitOfWork.Users
            .GetByEmailAsync(request.Email, cancellationToken);

        if (user == null)
        {
            _logger.LogDebug("User with email {Email} not found", request.Email);
            return null;
        }

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = user.FullName,
            Role = user.Role.ToString()
        };
    }
}
