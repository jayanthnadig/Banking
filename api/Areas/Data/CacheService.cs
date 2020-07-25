using ASNRTech.CoreService.Core;
using ASNRTech.CoreService.Security;
using ASNRTech.CoreService.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace ASNRTech.CoreService.Services
{
    public class CacheService
    {
        internal static async Task<T> GetAsync<T>(TeamHttpContext httpContext, string key)
        {
            if (httpContext.CacheExists)
            {
                return await httpContext.Cache.GetAsync<T>(key).ConfigureAwait(false);
            }
            return default(T);
        }

        internal static IDistributedCache GetCache(HttpContext httpContext)
        {
            return (IDistributedCache) httpContext.RequestServices.GetService(typeof(IDistributedCache));
        }

        internal static string GetSessionId(HttpContext httpContext, string sessionId)
        {
            IDistributedCache cache = GetCache(httpContext);

            if (cache != null)
            {
                return cache.GetString($"{Constants.CACHE_SESSION_ID}{sessionId}");
            }
            return string.Empty;
        }

        internal static async Task RemoveSessionAsync(TeamHttpContext httpContext, string sessionId)
        {
            if (httpContext.CacheExists)
            {
                await httpContext.Cache.RemoveAsync($"{Constants.CACHE_SESSION_ID}{sessionId}").ConfigureAwait(false);
            }
        }

        internal static async Task RemoveUserAsync(TeamHttpContext httpContext, User user)
        {
            if (httpContext.CacheExists)
            {
                await RemoveUserAsync(httpContext, user.UserId).ConfigureAwait(false);
            }
        }

        internal static async Task RemoveUserAsync(TeamHttpContext httpContext, string userId)
        {
            if (httpContext.CacheExists)
            {
                await httpContext.Cache.RemoveAsync($"{Constants.CACHE_USER}{userId}").ConfigureAwait(false);
            }
        }

        internal static async Task SetObjectAsync(TeamHttpContext httpContext, string key, object value)
        {
            if (httpContext.CacheExists)
            {
                await httpContext.Cache.SetAsync(key, Utility.ObjectToByteArray(value)).ConfigureAwait(false);
            }
        }

        internal static async Task StoreSessionAsync(TeamHttpContext httpContext, string sessionId)
        {
            if (httpContext.CacheExists)
            {
                await httpContext.Cache.SetStringAsync($"{Constants.CACHE_SESSION_ID}{sessionId}", sessionId).ConfigureAwait(false);
            }
        }

        internal static async Task StoreUserAsync(TeamHttpContext httpContext, User user)
        {
            if (httpContext.CacheExists)
            {
                await StoreUserAsync(httpContext.Cache, user).ConfigureAwait(false);
            }
        }

        internal static async Task StoreUserAsync(HttpContext httpContext, User user)
        {
            await StoreUserAsync(GetCache(httpContext), user).ConfigureAwait(false);
        }

        internal static async Task<User> GetUserAsync(HttpContext httpContext, string userId)
        {
            IDistributedCache cache = GetCache(httpContext);

            if (cache == null)
            {
                return null;
            }
            return cache.Get<User>($"{Constants.CACHE_USER}{userId}");
        }

        private static async Task StoreUserAsync(IDistributedCache cache, User user)
        {
            if (cache != null)
            {
                await cache.SetObjectAsync($"{Constants.CACHE_USER}{user.UserId}", user).ConfigureAwait(false);
            }
        }
    }
}
