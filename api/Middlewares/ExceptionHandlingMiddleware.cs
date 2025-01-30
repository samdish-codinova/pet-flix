using System.Net;
using System.Text.Json;
using BusinessLogicLayer;

namespace Middlewares
{
  public class ExceptionHandlingMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
      catch (ErrorResponseException ex)
      {
        _logger.LogWarning(ex, "Handled exception occurred.");

        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)ex.StatusCode;

        var errorResponse = new { status = response.StatusCode, message = ex.Message };
        var result = JsonSerializer.Serialize(errorResponse);

        await response.WriteAsync(result);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An unhandled exception occurred.");

        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var errorResponse = new
        {
          status = response.StatusCode,
          message = ex.Message,
          details = ex.StackTrace
        };

        var result = JsonSerializer.Serialize(errorResponse);
        await response.WriteAsync(result);
      }

    }

  }
}