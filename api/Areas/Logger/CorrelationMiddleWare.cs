using ASNRTech.CoreService.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace ASNRTech.CoreService.Core
{
    public class CorrelationMiddleWare
    {
        private readonly RequestDelegate _next;

        public CorrelationMiddleWare(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Items.Add(Constants.CONTEXT_CORRELATION_ID, Guid.NewGuid().ToString());

            //Continue down the Middleware pipeline, eventually returning to this class
            await _next(context);
        }
    }
}
