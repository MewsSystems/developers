using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public abstract class ControllerWithMediatR : ControllerBase
{
    private readonly ISender _sender;

    protected ControllerWithMediatR(ISender sender)
    {
        _sender = sender;
    }

    protected Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken) => _sender.Send(request, cancellationToken);
}
