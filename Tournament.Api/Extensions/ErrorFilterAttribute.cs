using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Tournament.Api.Extensions;

public class ErrorFilterAttribute(ProblemDetailsFactory factory) : IAsyncExceptionFilter
{
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        var exception = context.Exception;
        var httpContext = context.HttpContext;  

        var problemDetails = factory.CreateProblemDetails(
            httpContext,
            statusCode: httpContext.Response.StatusCode,
            title: "An error occurred while processing your request.",
            detail: exception.Message,
            instance: httpContext.Request.Path);
        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status,
            ContentTypes = { "application/json" }
        };

        context.ExceptionHandled = true;
    }
}
