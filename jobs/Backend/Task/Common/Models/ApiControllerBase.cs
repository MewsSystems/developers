using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Common.Models;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>()!;
}