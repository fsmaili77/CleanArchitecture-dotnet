using Clean.Architecture.Web.Dtos;
using Microsoft.IdentityModel.Tokens;

namespace Clean.Architecture.Web.Middlewares;

public class CustomExceptionHandlerMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

  public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task Invoke(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      await HandleExceptionAsync(context, ex);
    }
  }

  private Task HandleExceptionAsync(HttpContext context, Exception exception)
  {
    // Log the exception
    _logger.LogError(exception, "An error occurred while processing the request.");

    // Return a friendly error message to the client
    //context.Response.StatusCode = 500;
    //context.Response.ContentType = "text/html";
    return context.Response.WriteAsync("<html><body><h1>An error occurred</h1></body></html>");
  }
}
