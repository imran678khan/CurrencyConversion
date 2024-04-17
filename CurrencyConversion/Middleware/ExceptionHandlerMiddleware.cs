using CurrencyConversion.Services.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace CurrencyConversion.Middlewares
{
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static void UseExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }

    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(httpContext, ex).ConfigureAwait(false);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var error = "Unhandled exception.";

            switch (exception)
            {
                case NotFoundException:
                    code = HttpStatusCode.NotFound;
                    error = string.IsNullOrEmpty(exception.Message) ? "Not found." : exception.Message;
                    break;
                case BadRequestException:
                    code = HttpStatusCode.BadRequest;
                    error = exception.Message;
                    break;
                case UnauthorizedAccessException:
                    code = HttpStatusCode.Unauthorized;
                    error = exception.Message;
                    break;
                default:
                    _logger.LogError(exception, "Exception caught in ExceptionHandlingMiddleware.");
                    break;
            }

            var result = JsonConvert.SerializeObject(new { code = code, message = error });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
