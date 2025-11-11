using ApplicationLayer.Common.Abstractions;
using DomainLayer.Interfaces.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Behaviors;

/// <summary>
/// Pipeline behavior that wraps commands in a database transaction.
/// Only applies to commands, not queries.
/// </summary>
public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    public TransactionBehavior(
        IUnitOfWork unitOfWork,
        ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Only apply transaction to commands, not queries
        if (request is not ICommand<TResponse>)
        {
            return await next();
        }

        var requestName = typeof(TRequest).Name;

        try
        {
            _logger.LogInformation(
                "Beginning transaction for {RequestName}",
                requestName);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var response = await next();

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            _logger.LogInformation(
                "Committed transaction for {RequestName}",
                requestName);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Rolling back transaction for {RequestName}",
                requestName);

            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
