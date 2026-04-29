using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AciPlatform.Api.Filters;

public sealed class FleetExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<FleetExceptionFilter> _logger;

    public FleetExceptionFilter(ILogger<FleetExceptionFilter> logger)
    {
        _logger = logger;
    }

    public Task OnExceptionAsync(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case KeyNotFoundException ex:
                context.Result = new NotFoundObjectResult(new
                {
                    code = StatusCodes.Status404NotFound,
                    message = ex.Message
                });
                context.ExceptionHandled = true;
                return Task.CompletedTask;

            case ArgumentException ex:
                context.Result = new BadRequestObjectResult(new
                {
                    code = StatusCodes.Status400BadRequest,
                    message = ex.Message
                });
                context.ExceptionHandled = true;
                return Task.CompletedTask;

            case InvalidOperationException ex:
                context.Result = new BadRequestObjectResult(new
                {
                    code = StatusCodes.Status400BadRequest,
                    message = ex.Message
                });
                context.ExceptionHandled = true;
                return Task.CompletedTask;

            default:
                _logger.LogError(context.Exception, "Unhandled FleetTransportation exception");
                context.Result = new ObjectResult(new
                {
                    code = StatusCodes.Status500InternalServerError,
                    message = context.Exception.Message + (context.Exception.InnerException != null ? " - " + context.Exception.InnerException.Message : ""),
                    stack = context.Exception.StackTrace
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
                context.ExceptionHandled = true;
                return Task.CompletedTask;
        }
    }
}
