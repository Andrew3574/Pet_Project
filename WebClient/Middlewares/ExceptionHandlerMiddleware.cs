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
            context.Response.ContentType = "application/json";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                ErrorMessage = "MSG:"+exception.Message + "DATA: "+exception.Data + "SUORCE: "+exception.Source + "HELPLINK: "+exception.HelpLink + "TARGETMETHOD: "+exception.TargetSite
            });
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
