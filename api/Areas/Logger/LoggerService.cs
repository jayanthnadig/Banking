using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ASNRTech.CoreService.Core;
using ASNRTech.CoreService.Data;
using ASNRTech.CoreService.Enums;
using ASNRTech.CoreService.Utilities;

namespace ASNRTech.CoreService.Logging {
    public static class LoggerService {
        private static EnLogLevel? logLevel;

        private static bool IsDebugEnabled {
            get {
                return LogLevel <= EnLogLevel.DEBUG;
            }
        }

        private static bool IsInfoEnabled {
            get {
                return LogLevel <= EnLogLevel.INFO;
            }
        }

        private static EnLogLevel LogLevel {
            get {
                if (logLevel == null) {
                    string stringLogLevel = Utility.GetConfigValue("Logging:LogLevel");
                    if (stringLogLevel?.Length == 0) {
                        logLevel = EnLogLevel.INFO;
                    }
                    else {
                        logLevel = Utility.ParseEnum<EnLogLevel>(stringLogLevel);
                    }
                }
                return logLevel.Value;
            }
        }

        internal static void LogDebug(string requestId, string className, string methodName, string message, params object[] args) {
            if (IsDebugEnabled) {
                Log(requestId, EnLogLevel.DEBUG, className, methodName, message, args);
            }
        }

        internal static void LogDebug(string requestId, string className, string methodName, string message) {
            if (IsDebugEnabled) {
                Log(requestId, EnLogLevel.DEBUG, className, methodName, message);
            }
        }

        internal static void LogException(string requestId, string className, string methodName, Exception ex, bool skipNotification = false) {
            Log(requestId, EnLogLevel.ERROR, className, methodName, ex, skipNotification);
        }

        internal static void LogInfo(string requestId, string className, string methodName, string message) {
            if (IsInfoEnabled) {
                Log(requestId, EnLogLevel.INFO, className, methodName, message);
            }
        }

        internal static void LogInfo(string requestId, string className, string methodName, string message, params object[] args) {
            if (IsInfoEnabled) {
                Log(requestId, EnLogLevel.INFO, className, methodName, message, args);
            }
        }

        internal static void LogMethodEnd(string requestId, string className, string methodName) {
            LogDebug(requestId, className, methodName, "End");
        }

        internal static void LogMethodStart(string requestId, string className, string methodName) {
            LogDebug(requestId, className, methodName, "Start");
        }

        internal static ApiLogEntry SaveApiLogEntry(ApiLogEntry apiLogEntry) {
            using (TeamDbContext dbContext = new TeamDbContext()) {
                dbContext.ApiLogEntries.Add(apiLogEntry);
                dbContext.SaveChanges();

                return apiLogEntry;
            }
        }

        internal static ApiLogEntry SaveApiLogEntry(TeamHttpContext teamContext, string fullUrl, RestRequest restRequest) {
            ApiLogEntry apiLogEntry = new ApiLogEntry {
                Application = "core-service",
                Machine = Environment.MachineName,
                RequestId = teamContext.RequestId,
                RequestMethod = restRequest.Method.ToString(),
                RequestContentBody = GetParameter(restRequest, ParameterType.RequestBody),
                RequestContentType = GetHeader(restRequest, Constants.HEADER_CONTENT_TYPE),
                RequestHeaders = LoggerService.SerializeHeaders(restRequest.Parameters),
                RequestUri = fullUrl
            };

            return SaveApiLogEntry(apiLogEntry);
        }

        internal static string SerializeHeaders(WebHeaderCollection headers) {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (string item in headers.Keys) {
                dict.Add(item, headers[item]);
            }

            return SerialiseDictionary(dict);
        }

        internal static string SerializeHeaders(IHeaderDictionary headers) {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (string item in headers.Keys) {
                dict.Add(item, headers[item]);
            }

            return SerialiseDictionary(dict);
        }

        internal static string SerializeHeaders(List<KeyValuePair<string, string>> headers) {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> item in headers) {
                dict.Add(item.Key, item.Value);
            }

            return SerialiseDictionary(dict);
        }

        internal static string SerializeHeaders(List<string> headers) {
            string header = string.Empty;
            foreach (string s in headers) {
                header += s + " ";
            }

            return header;
        }

        internal static string SerializeHeaders(IList<Parameter> headers) {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (Parameter item in headers) {
                dict.Add(item.Name, item.Value.ToString());
            }

            return SerialiseDictionary(dict);
        }

        internal static void UpdateApiLogEntry(ApiLogEntry logEntry, IRestResponse<dynamic> restResponse) {
            using (TeamDbContext dbContext = new TeamDbContext()) {
                ApiLogEntry dbEntry = dbContext.ApiLogEntries.Find(logEntry.ApiLogEntryId);
                dbEntry.ResponseHeaders = LoggerService.SerializeHeaders(restResponse.Headers);
                dbEntry.ResponseContentType = restResponse.ContentType;
                dbEntry.ResponseContentBody = restResponse.Content;
                dbEntry.ResponseStatusCode = (int) restResponse.StatusCode;
                dbEntry.ResponseTimestamp = DateTime.Now;

                dbContext.SaveChanges();
            }
        }

        internal static void UpdateApiLogEntry(ApiLogEntry logEntry, IRestResponse restResponse) {
            using (TeamDbContext dbContext = new TeamDbContext()) {
                ApiLogEntry dbEntry = dbContext.ApiLogEntries.Find(logEntry.ApiLogEntryId);
                dbEntry.ResponseHeaders = LoggerService.SerializeHeaders(restResponse.Headers);
                dbEntry.ResponseContentType = restResponse.ContentType;
                dbEntry.ResponseContentBody = restResponse.Content;
                dbEntry.ResponseStatusCode = (int) restResponse.StatusCode;
                dbEntry.ResponseTimestamp = DateTime.Now;

                dbContext.SaveChanges();
            }
        }

        private static string GetHeader(RestRequest restRequest, string headerName) {
            Parameter item = restRequest.Parameters.Find(p => p.Type == ParameterType.HttpHeader && p.Name == headerName);

            return item == null ? string.Empty : item.Value.ToString();
        }

        private static string GetParameter(RestRequest restRequest, ParameterType parameterType) {
            Parameter item = restRequest.Parameters.Find(p => p.Type == parameterType);

            return item == null ? string.Empty : item.Value.ToString();
        }

        private static void Log(string requestId, EnLogLevel logLevel, string className, string methodName, string message, params object[] args) {
            Log(requestId, logLevel, className, methodName, string.Format(message, args));
        }

        private static void Log(string requestId, EnLogLevel logLevel, string className, string methodName, Exception ex, bool skipNotification = false) {
            string exceptionMessage = ex.Message;

            while (ex.InnerException != null) {
                ex = ex.InnerException;
                exceptionMessage += "\n" + ex.Message;
            }

            Log(requestId, logLevel, className, methodName, string.Empty, exceptionMessage, skipNotification);
        }

        private static void Log(string requestId, EnLogLevel logLevel, string className, string methodName, string message, string exception = "", bool skipNotification = false) {
            Task.Run(() => {
                const string LOG_FORMAT = "{0}:{1}:{2}";
                const int MAX_LEN = 3850;
                message = string.Format(LOG_FORMAT, className, methodName, message);

                if (message.Length > MAX_LEN) {
                    message = message.Substring(0, MAX_LEN);
                }

                TeamDbContext dbContext = new TeamDbContext();
                dbContext.AppLogEntries.Add(new AppLogEntry {
                    RequestId = requestId,
                    Date = DateTime.Now,
                    User = Utility.CurrentUserId,
                    Level = logLevel.ToString(),
                    Message = message,
                    ExceptionMessage = exception,
                    AdminNotified = skipNotification
                });

                dbContext.SaveChanges();
            });
        }

        private static string SerialiseDictionary(Dictionary<string, string> dict) {
            return JsonConvert.SerializeObject(dict, Formatting.Indented, new JsonSerializerSettings() {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        //private static string SerializeHeaders(HttpHeaders headers) {
        //  var dict = new Dictionary<string, string>();

        //  foreach (var item in headers.ToList()) {
        //    if (item.Value != null) {
        //      string header = String.Empty;
        //      foreach (string value in item.Value) {
        //        header += value + " ";
        //      }

        //      // Trim the trailing space and add item to the dictionary
        //      header = header.TrimEnd(" ".ToCharArray());
        //      dict.Add(item.Key, header);
        //    }
        //  }

        //  return JsonConvert.SerializeObject(dict, Formatting.Indented, new JsonSerializerSettings() {
        //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //  });
        //}
    }
}
