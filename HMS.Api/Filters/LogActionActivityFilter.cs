using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HMS.Api.Filters;

public class LogActionActivityFilter(ILogger<LogActionActivityFilter> logger) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var controllerName = context.Controller.GetType().Name;
        var actionName = context.ActionDescriptor.DisplayName;

        logger.LogInformation(" [Audit] Starting: {Controller} -> {Action} with {Count} argument(s).",
            controllerName, actionName, context.ActionArguments.Count);

        var stopwatch = Stopwatch.StartNew();

        var resultContext = await next();

        stopwatch.Stop();

        var statusCode = resultContext.Result switch
        {
            ObjectResult obj => obj.StatusCode ?? 200,
            StatusCodeResult status => status.StatusCode,
            _ => 200
        };

        logger.LogInformation("⬅️ [Audit] Completed: {Controller} in {Elapsed} ms | Status: {StatusCode}",
            controllerName, stopwatch.ElapsedMilliseconds, statusCode);
    }
}
