using Newtonsoft.Json;
using System.Net;

namespace EventsAPI.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionMessageAsync(context, ex);
            }
        }

        private static Task HandleExceptionMessageAsync(HttpContext context,Exception exception)
        {
            context.Response.ContentType = "text/html";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            var result = $@"
                <html>
                <head>
                    <title>Error</title>
                </head>
                <body>
                    <h1 class=""text-danger"">Error Occurred</h1>
                    <p><strong>Status Code:</strong> {statusCode}</p>
                    <p><strong>Error Message:</strong> MSG: {exception.Message}</p>
                </body>
                </html>";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
