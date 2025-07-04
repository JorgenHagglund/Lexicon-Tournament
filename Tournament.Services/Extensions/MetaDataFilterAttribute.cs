using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text;
using Tournament.Core.Contracts;

namespace Tournament.Services.Extensions;

public class MetaDataFilterAttribute(IMetaData metaData) : ResultFilterAttribute
{
    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        // Custom logic after the result is executed
        if (context.Result is ObjectResult objectResult)
        {
            // Add meta-data to the response
            objectResult.Value ??= new { Message = "No data found" }; // Ensure there's a value to avoid null
            var newResult = new
            {
                Data = objectResult.Value,
                Metadata = metaData
            };
            byte[] data = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(newResult));

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            context.HttpContext.Response.ContentType = "application/json";

            await context.HttpContext.Response.Body.WriteAsync(data.AsMemory(0, data.Length));
        }
        else
            await next();
    }
}
