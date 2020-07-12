using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using ASNRTech.CoreService.Data;
using ASNRTech.CoreService.Enums;
using ASNRTech.CoreService.Logging;
using ASNRTech.CoreService.Services;
using ASNRTech.CoreService.Utilities;

namespace ASNRTech.CoreService.Security
{
    public class TeamAuthorizeAttribute : TypeFilterAttribute
    {
        public TeamAuthorizeAttribute(AccessType accessType = AccessType.Client, bool clientScoped = true) : base(typeof(TeamAuthorizeFilter))
        {
            this.Arguments = (new object[] { accessType, clientScoped });
        }

        public TeamAuthorizeAttribute(AccessType accessType = AccessType.Client, SecurityCheckType securityCheckType = SecurityCheckType.Client) : base(typeof(TeamAuthorizeFilter))
        {
            this.Arguments = (new object[] { accessType, securityCheckType });
        }
    }

    public class TeamAuthorizeFilter : IAuthorizationFilter
    {
        private static readonly string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();

        private readonly AccessType accessType;
        private readonly bool clientScoped;
        private readonly SecurityCheckType securityCheckType = SecurityCheckType.Client;
        private string requestId = "";

        public TeamAuthorizeFilter()
        {
        }

        public TeamAuthorizeFilter(AccessType accessType, bool clientScoped)
        {
            this.accessType = accessType;
            this.clientScoped = clientScoped;
        }

        public TeamAuthorizeFilter(AccessType accessType, SecurityCheckType securityCheckType)
        {
            this.accessType = accessType;
            this.securityCheckType = securityCheckType;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            requestId = Utility.GetRequestId(context.HttpContext);

            if (accessType == AccessType.Open)
            {
                return;
            }

            IHeaderDictionary headers = context.HttpContext.Request.Headers;
            string clientId = string.Empty;
            string userId = GetUserIdFromRoute(context);

            KeyValuePair<JwtTokenValidationStatus, LoginResponseModel> validationStatus = ValidateJwt(context.HttpContext);

            if (validationStatus.Key != JwtTokenValidationStatus.Valid)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            User user = ValidateSession(context, validationStatus.Value);

            if (user == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (user.UserType == UserType.Client)
            {
                // check clients have access to this route
                if (clientScoped)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }

            context.HttpContext.SetValue(Constants.CONTEXT_USER, user);

            GenericPrincipal genericPrincipal = new GenericPrincipal(new GenericIdentity(user.UserId), Array.Empty<string>());
            //GenericPrincipal genericPrincipal = new GenericPrincipal(new GenericIdentity(userId), Array.Empty<string>());
            context.HttpContext.User = genericPrincipal;
            Thread.CurrentPrincipal = genericPrincipal;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                context.HttpContext.SetValue(Constants.CONTEXT_USER_ID, userId);
            }
        }

        internal static KeyValuePair<JwtTokenValidationStatus, LoginResponseModel> ValidateJwt(HttpContext httpContext)
        {
            string jwt = string.Empty;
            IHeaderDictionary headers = httpContext.Request.Headers;
            if (headers.ContainsKey(Constants.HEADER_JWT_CLIENT_TOKEN))
            {
                jwt = headers[Constants.HEADER_JWT_CLIENT_TOKEN];
            }

            if (string.IsNullOrWhiteSpace(jwt))
            {
                jwt = HttpUtility.GetQueryStringValue(httpContext, Constants.QUERY_PARAM_JWT);
            }

            return Jwt.Validate(jwt);
        }

        private string GetClientIdFromRoute(AuthorizationFilterContext context)
        {
            return GetParamFromRoute(context, Constants.ROUTE_PARAM_CLIENT_ID, true);
        }

        private string GetUserIdFromRoute(AuthorizationFilterContext context)
        {
            return GetParamFromRoute(context, Constants.ROUTE_PARAM_USER_ID, true);
        }

        private string GetParamFromRoute(AuthorizationFilterContext context, string param, bool logMissingParam)
        {
            const string METHOD_NAME = "GetParamFromRoute";

            RouteValueDictionary keyValuePairs = context.RouteData.Values;

            object value = keyValuePairs.FirstOrDefault(x => string.Equals(x.Key, param, StringComparison.OrdinalIgnoreCase)).Value;

            if (value == null && logMissingParam)
            {
                LoggerService.LogInfo(requestId, className, METHOD_NAME, $"Can't determine {param} from route. {context.HttpContext.Request.Path}");
                return null;
            }

            return (string) value;
        }

        private User ValidateSession(AuthorizationFilterContext context, LoginResponseModel loginResponseModel)
        {
            User user = null;

            string sessionIdFromCache = CacheService.GetSessionId(context.HttpContext, loginResponseModel.SessionId);

            // TODO: Handle sessions between server restarts and app-updates
            if (!string.IsNullOrWhiteSpace(sessionIdFromCache))
            {
                user = CacheService.GetUserAsync(context.HttpContext, loginResponseModel.UserId).Result;
            }

            if (user == null)
            {
                using (TeamDbContext dbContext = new TeamDbContext())
                {
                    UserSession userSession = dbContext.UserSessions.FirstOrDefault(e => e.SessionId == loginResponseModel.SessionId && e.Active);

                    if (userSession == null)
                    {
                        context.Result = new UnauthorizedResult();
                        return null;
                    }
                    user = dbContext.Users.FirstOrDefault(e => e.UserId == loginResponseModel.UserId.ToUpper(CultureInfo.InvariantCulture));

                    if (user == null)
                    {
                        context.Result = new UnauthorizedResult();
                        return null;
                    }

                    //AuthService.PopulateUserClientsAsync(dbContext, user);

                    // save user to session
                    CacheService.StoreUserAsync(context.HttpContext, user);
                }
            }

            return user;
        }
    }
}
