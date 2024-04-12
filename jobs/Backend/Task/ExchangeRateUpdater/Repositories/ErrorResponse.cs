using System;

namespace ExchangeRateUpdater.Repositories;

public record ErrorResponse(
    string Description, 
    string EndPoint, 
    string ErrorCode, // [ INTERNAL_SERVER_ERROR, VALIDATION_ERROR ]
    DateTime HappenedAt, 
    string MessageId);