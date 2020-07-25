using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ASNRTech.CoreService.Core
{
    public class TeamControllerBase : ControllerBase
    {
        protected ResponseBase Action<T>(Func<TeamHttpContext, T, ResponseBase> methodName, T data)
        {
            if (methodName == null)
            {
                throw new ArgumentNullException(nameof(methodName));
            }

            if (this.ModelState.IsValid)
            {
                return methodName(new TeamHttpContext(HttpContext), data);
            }
            else
            {
                return GetModelErrorsResponse();
            }
        }

        protected async Task<ResponseBase> ActionAsync<T>(Func<TeamHttpContext, T, Task<ResponseBase>> methodName, T data)
        {
            if (methodName == null)
            {
                throw new ArgumentNullException(nameof(methodName));
            }
            if (this.ModelState.IsValid)
            {
                return await methodName(new TeamHttpContext(this.HttpContext), data).ConfigureAwait(false);
            }
            else
            {
                return GetModelErrorsResponse();
            }
        }

        protected ResponseBase GetModelErrorsResponse()
        {
            List<ModelError> errors = ModelState.Values.SelectMany(v => v.Errors).ToList();
            return new ErrorResponse
            {
                Code = HttpStatusCode.BadRequest,
                ModelErrors = errors,
                RequestRef = Utility.GetRequestId(HttpContext)
            };
        }

        protected ErrorResponse<T> GetModelErrorsResponse<T>()
        {
            List<ModelError> errors = ModelState.Values.SelectMany(v => v.Errors).ToList();
            return new ErrorResponse<T>
            {
                Code = HttpStatusCode.BadRequest,
                ModelErrors = errors,
                RequestRef = Utility.GetRequestId(HttpContext)
            };
        }
    }
}
