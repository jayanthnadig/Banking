using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ASNRTech.CoreService.Core;
using ASNRTech.CoreService.Logging;

namespace ASNRTech.CoreService.Utilities {
  internal static class HttpUtility {

    internal static string CreateExpiredToken() {
      return CreateJwtToken(DateTime.UtcNow.AddHours(-1));
    }

    internal static string CreateJwtToken() {
      return CreateJwtToken(DateTime.UtcNow.AddHours(1));
    }

    internal static async Task<KeyValuePair<HttpStatusCode, string>> GetAsync(TeamHttpContext teamHttpContext, string url) {
      IRestResponse restResponse = await SendAsync(teamHttpContext, url, Method.GET).ConfigureAwait(false);

      return new KeyValuePair<HttpStatusCode, string>(restResponse.StatusCode, restResponse.Content);
    }

    internal static async Task<IRestResponse<T>> GetAsync<T>(TeamHttpContext teamHttpContext, string url) where T : new() {
      return await SendAsync<T>(teamHttpContext, url, Method.GET).ConfigureAwait(false);
    }

    internal static DateTime GetQueryStringDate(HttpContext httpContext, string key, DateTime defaultValue) {
      string qsValue = string.Empty;
      if (httpContext.Request.Query.ContainsKey(key)) {
        qsValue = httpContext.Request.Query[key];
      }
      if (string.IsNullOrWhiteSpace(qsValue)) {
        return defaultValue;
      }
      return Convert.ToDateTime(qsValue);
    }

    internal static string GetQueryStringValue(HttpContext httpContext, string key) {
      if (httpContext.Request.Query.ContainsKey(key)) {
        return httpContext.Request.Query[key];
      }
      return string.Empty;
    }

    internal static async Task<IRestResponse<T>> PostAsync<T>(TeamHttpContext teamHttpContext, string url, dynamic data) where T : new() {
      return await SendAsync<T>(teamHttpContext, url, Method.POST, data).ConfigureAwait(false);
    }

    internal static IRestResponse<T> Send<T>(string url, RestSharp.Method method, dynamic data) where T : new() {
      RestClient restClient = new RestClient(url);

      RestRequest restRequest = new RestRequest(string.Empty, method);

      if (data != null) {
        restRequest.AddJsonBody(data);
      }

      return restClient.Execute<T>(restRequest);
    }

    internal static async Task<IRestResponse<T>> SendAsync<T>(TeamHttpContext teamHttpContext, string url, Method method, dynamic data = null) where T : new() {
      RestClient restClient = new RestClient(url);
      RestRequest restRequest = new RestRequest(string.Empty, method);
      restRequest.AddHeader(Constants.HEADER_CONTENT_TYPE, "application/json");
      ApiLogEntry logEntry = LoggerService.SaveApiLogEntry(teamHttpContext, url, restRequest);

      if (data != null) {
        restRequest.AddJsonBody(data);
      }

      IRestResponse<T> restResponse = await restClient.ExecuteTaskAsync<T>(restRequest).ConfigureAwait(false);
      LoggerService.UpdateApiLogEntry(logEntry, restResponse);

      return restResponse;
    }

    internal static async Task<IRestResponse> SendAsync(TeamHttpContext teamHttpContext, string url, Method method, dynamic data = null) {
      RestClient restClient = new RestClient(url);
      RestRequest restRequest = new RestRequest(string.Empty, method);

      restRequest.AddHeader(Constants.HEADER_CONTENT_TYPE, "application/json");

      string fullUrl = restClient.BuildUri(restRequest).ToString();
      ApiLogEntry logEntry = LoggerService.SaveApiLogEntry(teamHttpContext, fullUrl, restRequest);

      if (data != null) {
        restRequest.AddJsonBody(data);
      }

      IRestResponse restResponse = await restClient.ExecuteTaskAsync(restRequest).ConfigureAwait(false);

      LoggerService.UpdateApiLogEntry(logEntry, restResponse);
      return restResponse;
    }

    private static string CreateJwtToken(DateTime expiry) {
      string preSharedKey = Utility.GetConfigValue("services:jwtSymmetricKey");

      SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(preSharedKey));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

      var header = new JwtHeader(credentials);

      List<Claim> claims = new List<Claim> {
        new Claim("t_value", DateTime.UtcNow.UnixTimeStamp().ToString()),
        new Claim("exp", expiry.UnixTimeStamp().ToString()),
      };
      var payload = new JwtPayload(claims);

      JwtSecurityToken secToken = new JwtSecurityToken(header, payload);

      JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

      return handler.WriteToken(secToken);
    }
  }
}
