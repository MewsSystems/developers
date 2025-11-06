using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;
using DomainLayer.Interfaces.Persistence;
using DomainLayer.ValueObjects;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Commands.Currencies.CreateCurrency;

public class CreateCurrencyCommandHandler
    : ICommandHandler<CreateCurrencyCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateCurrencyCommandHandler> _logger;

    public CreateCurrencyCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateCurrencyCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(
        CreateCurrencyCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Check if currency already exists
            var existingCurrency = await _unitOfWork.Currencies
                .GetByCodeAsync(request.Code, cancellationToken);

            if (existingCurrency != null)
            {
                _logger.LogWarning("Currency {Code} already exists", request.Code);
                return Result.Failure<int>($"Currency with code '{request.Code}' already exists.");
            }

            // Create currency value object
            var currency = Currency.FromCode(request.Code);

            // Add to repository
            await _unitOfWork.Currencies.AddAsync(currency, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Query back to get the generated ID
            var savedCurrency = await _unitOfWork.Currencies.GetByCodeAsync(request.Code, cancellationToken);
            var currencyId = savedCurrency?.Id ?? 0;

            _logger.LogInformation("Created currency {Code} with ID {Id}", currency.Code, currencyId);

            return Result.Success(currencyId);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid currency code: {Code}", request.Code);
            return Result.Failure<int>(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating currency {Code}", request.Code);
            return Result.Failure<int>($"Failed to create currency: {ex.Message}");
        }
    }
}
