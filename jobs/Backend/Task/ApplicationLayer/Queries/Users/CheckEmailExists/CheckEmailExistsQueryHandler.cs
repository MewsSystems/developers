using ApplicationLayer.Common.Abstractions;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.Users.CheckEmailExists;

public class CheckEmailExistsQueryHandler : IQueryHandler<CheckEmailExistsQuery, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CheckEmailExistsQueryHandler> _logger;

    public CheckEmailExistsQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<CheckEmailExistsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(
        CheckEmailExistsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Checking if email exists: {Email}", request.Email);

        var exists = await _unitOfWork.Users
            .ExistsByEmailAsync(request.Email, cancellationToken);

        _logger.LogDebug("Email {Email} exists: {Exists}", request.Email, exists);

        return exists;
    }
}
