using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Users;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.Users.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDetailDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;

    public GetUserByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetUserByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<UserDetailDto?> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Getting user by ID: {UserId}", request.UserId);

        var user = await _unitOfWork.Users
            .GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            _logger.LogDebug("User {UserId} not found", request.UserId);
            return null;
        }

        return new UserDetailDto
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
