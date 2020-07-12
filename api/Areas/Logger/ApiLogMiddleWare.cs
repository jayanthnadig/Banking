using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ASNRTech.CoreService.Logging;
using ASNRTech.CoreService.Utilities;

namespace ASNRTech.CoreService.Core
{
    public class ApiLogMiddleWare
    {
        private readonly RequestDelegate _next;
        private static readonly HashSet<string> apiRequestBlackList = new HashSet<string>();
        private static readonly HashSet<string> apiResponseBlackList = new HashSet<string>();
        private static readonly HashSet<string> apiBlackList = new HashSet<string>();

        public ApiLogMiddleWare(RequestDelegate next)
        {
            if (apiBlackList.Count == 0)
            {
                apiBlackList.Add("/swagger/v1/swagger.json");
                apiBlackList.Add("/v1/auth/login");

                foreach (string item in apiBlackList)
                {
                    apiRequestBlackList.Add(item);
                    apiResponseBlackList.Add(item);
                }
            }

            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            bool logRequestBody = !apiRequestBlackList.Contains(context.Request.Path);
            bool logResponseBody = !apiResponseBlackList.Contains(context.Request.Path);

            //First, get the incoming request
            ApiLogEntry apiLogEntry = await FormatRequest(context, logRequestBody);

            //Copy a pointer to the original response body stream
            Stream originalBodyStream = context.Response.Body;

            //Create a new memory stream...
            using (MemoryStream responseBody = new MemoryStream())
            {
                //...and use that for the temporary response body
                context.Response.Body = responseBody;

                //Continue down the Middleware pipeline, eventually returning to this class
                await _next(context);

                //Format the response from the server
                apiLogEntry = await FormatResponse(context, apiLogEntry, logResponseBody);

                //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                await responseBody.CopyToAsync(originalBodyStream);
            }

            LoggerService.SaveApiLogEntry(apiLogEntry);
        }

        private async Task<ApiLogEntry> FormatRequest(HttpContext context, bool logBody)
        {
            HttpRequest request = context.Request;

            string requestUri = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
            ApiLogEntry returnValue = new ApiLogEntry
            {
                RequestId = (string) context.Items[Constants.CONTEXT_CORRELATION_ID],
                Application = "core-service",
                User = context.User.Identity.Name,
                //User = "Admin",
                RequestContentType = request.ContentType,
                RequestIpAddress = context.Connection.RemoteIpAddress.ToString(),
                RequestMethod = request.Method,
                RequestHeaders = LoggerService.SerializeHeaders(request.Headers),
                RequestTimestamp = DateTime.Now,
                RequestUri = requestUri
            };

            if (logBody)
            {
                var body = request.Body;

                //This line allows us to set the reader for the request back at the beginning of its stream.
                request.EnableRewind();

                returnValue.RequestContentBody = await new StreamReader(request.Body).ReadToEndAsync();

                request.Body.Position = 0;
            }
            return returnValue;
        }

        private async Task<ApiLogEntry> FormatResponse(HttpContext httpContext, ApiLogEntry apiLogEntry, bool logBody)
        {
            HttpResponse response = httpContext.Response;
            if (logBody)
            {
                //We need to read the response stream from the beginning...
                response.Body.Seek(0, SeekOrigin.Begin);

                //...and copy it into a string
                apiLogEntry.ResponseContentBody = await new StreamReader(response.Body).ReadToEndAsync();
            }
            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            apiLogEntry.ResponseStatusCode = response.StatusCode;
            apiLogEntry.ResponseHeaders = LoggerService.SerializeHeaders(response.Headers);
            apiLogEntry.User = httpContext.User.Identity.Name;

            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return apiLogEntry;
        }

        internal static ApiLogEntry GetApiLogEntry(string requestUrl, HttpMethod method)
        {
            return ApiLogMiddleWare.GetApiLogEntry(requestUrl, method, string.Empty);
        }

        internal static ApiLogEntry GetApiLogEntry(string requestUrl, HttpMethod method, List<string> headers)
        {
            return ApiLogMiddleWare.GetApiLogEntry(requestUrl, method, LoggerService.SerializeHeaders(headers));
        }

        internal static ApiLogEntry GetApiLogEntry(string requestUrl, HttpMethod method, WebHeaderCollection headers)
        {
            return ApiLogMiddleWare.GetApiLogEntry(requestUrl, method, LoggerService.SerializeHeaders(headers));
        }

        internal static ApiLogEntry GetApiLogEntry(string requestUrl, HttpMethod method, List<KeyValuePair<string, string>> headers)
        {
            return ApiLogMiddleWare.GetApiLogEntry(requestUrl, method, LoggerService.SerializeHeaders(headers));
        }

        internal static ApiLogEntry GetApiLogEntry(string requestUrl, HttpMethod method, string headers)
        {
            return new ApiLogEntry
            {
                Application = "app-net",
                Machine = Environment.MachineName,
                RequestMethod = method.ToString(),
                RequestTimestamp = DateTime.Now,
                RequestUri = requestUrl,
                RequestContentBody = string.Empty,
                RequestHeaders = headers
            };
        }
    }
}
