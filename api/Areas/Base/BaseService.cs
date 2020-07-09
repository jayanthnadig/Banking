using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ASNRTech.CoreService.Core;
using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.Utilities;
using ASNRTech.CoreService.Dashboard;

namespace ASNRTech.CoreService.Services
{
    public class BaseService
    {
        internal static ResponseBase GetDuplicateErrorResponse(TeamHttpContext httpContext, string modelError)
        {
            return GetModelErrorResponse(httpContext, modelError, HttpStatusCode.PreconditionFailed);
        }

        internal static ResponseBase GetModelErrorResponse(TeamHttpContext httpContext, string modelError, HttpStatusCode code = HttpStatusCode.BadRequest)
        {
            return GetResponse(httpContext, new List<ModelError> {
        new ModelError(modelError)
      }, code);
        }

        internal static PagedResponse<T> GetNotFoundPagingResponse<T>(TeamHttpContext httpContext, string errorMessage = "")
        {
            return new PagedResponse<T>
            {
                RowCount = 0,
                RequestRef = httpContext.RequestId,
                Code = HttpStatusCode.NotFound,
                Error = errorMessage
            };
        }

        internal static ResponseBase GetNotFoundResponse(TeamHttpContext httpContext, string errorMessage = "")
        {
            return GetResponse(httpContext, HttpStatusCode.NotFound, errorMessage);
        }

        internal static ResponseBase<T> GetNotFoundResponse<T>(TeamHttpContext httpContext, string errorMessage = "")
        {
            return GetTypedResponse<T>(httpContext, HttpStatusCode.NotFound, errorMessage);
        }

        internal static PagedResponse<T> GetPagingResponse<T>(TeamHttpContext teamContext, DataRequest pagingRequest, List<T> entities)
        {
            if (pagingRequest.SortOrder.EqualsIgnoreCase("desc"))
            {
                entities.Reverse();
            }

            // entities= entities.OrderBy(sortBy).ToList();
            int dataCount = entities.Count;
            entities = entities.Skip(pagingRequest.Offset).Take(pagingRequest.Limit).ToList();

            return new PagedResponse<T>
            {
                RowCount = dataCount,
                DataEntries = entities,
                RequestRef = teamContext.RequestId,
                Code = HttpStatusCode.OK
            };
        }

        internal static PagedResponse<T> GetPagingResponse<T>(TeamHttpContext teamContext)
        {
            return new PagedResponse<T>
            {
                RowCount = 0,
                DataEntries = null,
                RequestRef = teamContext.RequestId,
                Code = HttpStatusCode.OK
            };
        }

        internal static PagedResponse<T> GetPagingResponse<T>(TeamHttpContext teamContext, HttpStatusCode statusCode, string errorMessage = "")
        {
            return new PagedResponse<T>
            {
                RowCount = 0,
                DataEntries = null,
                RequestRef = teamContext.RequestId,
                Code = statusCode,
                Error = errorMessage
            };
        }

        internal static PagedResponse<T> GetPagingResponse<T>(TeamHttpContext httpContext, List<T> entities, int dataCount)
        {
            return new PagedResponse<T>
            {
                RowCount = dataCount,
                DataEntries = entities,
                RequestRef = httpContext.RequestId,
                Code = HttpStatusCode.OK
            };
        }

        internal static PagedResponse<T> GetPagingResponse<T>(TeamHttpContext httpContext, List<T> entities)
        {
            return GetPagingResponse<T>(httpContext, entities, entities.Count);
        }

        internal static PagedResponse<T> GetPagingResponse<T>(TeamHttpContext httpContext, IEnumerable<T> entities)
        {
            return GetPagingResponse<T>(httpContext, entities.ToList());
        }

        internal static PagedResponse<T> GetPagingResponse<T>(TeamHttpContext httpContext, DataWithCount<T> entities)
        {
            return new PagedResponse<T>
            {
                RowCount = entities.DataCount,
                DataEntries = entities.Data,
                RequestRef = httpContext.RequestId,
                Code = HttpStatusCode.OK
            };
        }

        internal static ResponseBase GetResponse(TeamHttpContext teamContext, List<ModelError> modelErrors)
        {
            if (modelErrors.Count == 0)
            {
                return new ResponseBase
                {
                    RequestRef = teamContext.RequestId,
                    Code = HttpStatusCode.OK
                };
            }
            else
            {
                return new ErrorResponse
                {
                    RequestRef = teamContext.RequestId,
                    Code = HttpStatusCode.BadRequest,
                    Error = "Model Errors",
                    ModelErrors = modelErrors
                };
            }
        }

        internal static ResponseBase GetResponse(TeamHttpContext teamContext, string errorMessage)
        {
            return new ErrorResponse
            {
                RequestRef = teamContext.RequestId,
                Code = HttpStatusCode.BadRequest,
                Error = errorMessage
            };
        }

        internal static ResponseBase GetResponse(TeamHttpContext teamContext, HttpStatusCode httpStatusCode, string errorMessage)
        {
            return new ErrorResponse
            {
                RequestRef = teamContext.RequestId,
                Code = httpStatusCode,
                Error = errorMessage
            };
        }

        internal static ResponseBase GetResponse(TeamHttpContext teamContext)
        {
            return new ResponseBase
            {
                RequestRef = teamContext.RequestId,
                Code = HttpStatusCode.OK
            };
        }

        internal static ResponseBase GetResponse(TeamHttpContext teamContext, dynamic data)
        {
            return new ResponseBase
            {
                RequestRef = teamContext.RequestId,
                Code = HttpStatusCode.OK,
                Data = data
            };
        }

        internal static ResponseBase GetResponse(TeamHttpContext httpContext, List<ModelError> modelErrors, HttpStatusCode code = HttpStatusCode.OK)
        {
            code = (modelErrors.Count == 0) ? HttpStatusCode.OK : HttpStatusCode.BadRequest;

            if (modelErrors.Count == 0)
            {
                return new ResponseBase
                {
                    RequestRef = httpContext.RequestId,
                    Code = code
                };
            }
            else
            {
                return new ErrorResponse
                {
                    RequestRef = httpContext.RequestId,
                    Code = code,
                    Error = "Model Errors",
                    ModelErrors = modelErrors
                };
            }
        }

        internal static ResponseBase<T> GetTypedResponse<T>(TeamHttpContext teamContext, T data)
        {
            return new ResponseBase<T>
            {
                RequestRef = teamContext.RequestId,
                Code = HttpStatusCode.OK,
                Data = data
            };
        }

        internal static ResponseBase<List<T>> GetTypedResponse<T>(TeamHttpContext teamContext, List<T> data)
        {
            return new ResponseBase<List<T>>
            {
                RequestRef = teamContext.RequestId,
                Code = HttpStatusCode.OK,
                Data = data
            };
        }

        internal static ResponseBase<T> GetTypedResponse<T>(TeamHttpContext teamContext, HttpStatusCode httpStatusCode, string errorMessage = "")
        {
            return new ResponseBase<T>
            {
                RequestRef = teamContext.RequestId,
                Code = httpStatusCode,
                Error = errorMessage
            };
        }

        internal static ResponseBase GetUnauthorizedResponse(TeamHttpContext teamContext, string errorMessage = "Unauthorized")
        {
            return GetResponse(teamContext, HttpStatusCode.Unauthorized, errorMessage);
        }

        internal static ResponseBase<T> GetUnauthorizedResponse<T>(TeamHttpContext httpContext, string errorMessage = "Unauthorized")
        {
            return GetUnauthorizedResponse<T>(httpContext, default(T), errorMessage);
        }

        internal static ResponseBase<T> GetUnauthorizedResponse<T>(TeamHttpContext httpContext, T data, string errorMessage = "Unauthorized")
        {
            return new ResponseBase<T>
            {
                RequestRef = httpContext.RequestId,
                Code = HttpStatusCode.Unauthorized,
                Error = errorMessage,
                Data = data
            };
        }
    }
}
