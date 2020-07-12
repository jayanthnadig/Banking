using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System;
using ASNRTech.CoreService.Security;
using ASNRTech.CoreService.Utilities;

namespace ASNRTech.CoreService.Core {
  public class TeamHttpContext {
    private HttpContext httpContext;

    public TeamHttpContext(HttpContext httpContext) {
      this.httpContext = httpContext;
    }

    internal IDistributedCache Cache {
      get {
        IDistributedCache cache = (IDistributedCache) httpContext.RequestServices.GetService(typeof(IDistributedCache));

        if (cache == null) {
          throw new ApplicationException("Cache not found in services");
        }

        return cache;
      }
    }

    internal bool CacheExists {
      get {
        try {
          IDistributedCache cache = (IDistributedCache) httpContext.RequestServices.GetService(typeof(IDistributedCache));

          return cache == null ? false : true;
        }
        catch {
          return false;
        }
      }
    }

        internal string ContextUserId
        {
            get
            {
                return (string) httpContext.GetValue(Constants.CONTEXT_USER_ID);
            }
        }

    internal string RequestId {
      get {
        return Utility.GetRequestId(httpContext);
      }
    }

    internal void SetValue(string key, object value) {
      this.httpContext.SetValue(key, value);
    }
  }
}
