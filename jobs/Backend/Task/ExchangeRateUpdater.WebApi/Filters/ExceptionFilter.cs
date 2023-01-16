using System.Net;
using ExchangeRateUpdater.WebApi.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExchangeRateUpdater.WebApi.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        if (exception is ServiceUnavailableException)
        {
            context.Result = new JsonResult(exception.Message)
            {
                Value = new ServiceResponse<IEnumerable<ExchangeRate>>
                {
                    Data = new ExchangeRate[]{},
                    Success = false,
                    Message = exception.Message
                },
                StatusCode = (int)HttpStatusCode.ServiceUnavailable
            };
        }
        if (exception is InvalidConfigurationException)
        {
            context.Result = new JsonResult(exception.Message)
            {
                Value = new ServiceResponse<IEnumerable<ExchangeRate>>
                {
                    Data = new ExchangeRate[] { },
                    Success = false,
                    Message = exception.Message
                },
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }
}