using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.Logging;
using ASNRTech.CoreService.Utilities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ASNRTech.CoreService.Core
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
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

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            const HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError; // 500 if unexpected

            //if (exception is MyNotFoundException)
            //  code = HttpStatusCode.NotFound;
            //else if (exception is MyUnauthorizedException)
            //  code = HttpStatusCode.Unauthorized;
            //else if (exception is MyException)
            //  code = HttpStatusCode.BadRequest;

            string requestId = Utility.GetRequestId(context);
            LoggerService.LogException(requestId, ex.Source, "", ex);

            string errorMessage = ex.Message;
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                errorMessage += Environment.NewLine + ex.Message;
            }

            ErrorResponse error = new ErrorResponse
            {
                Code = httpStatusCode,
                Error = errorMessage,
                RequestRef = requestId
            };

            string result = JsonConvert.SerializeObject(error);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) httpStatusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
