using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ASNRTech.CoreService.Utilities
{
    internal static class Extensions
    {
        internal static string EmptyIfNull(this string s)
        {
            return s ?? string.Empty;
        }

        internal static bool EqualsIgnoreCase(this string s, string compare)
        {
            if (s == null || compare == null)
            {
                return false;
            }

            return s.Equals(compare, StringComparison.OrdinalIgnoreCase);
        }

        internal static string FormatDateForDisplay(this DateTime dt)
        {
            return dt.ToString(Constants.INDIA_DATE_FORMAT, CultureInfo.InvariantCulture);
        }

        internal static string FormatDateTimeForDisplay(this DateTime dt)
        {
            return dt.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        }

        internal static string FormatTimeForDisplay(this DateTime dt)
        {
            return dt.ToString("HH:mm", CultureInfo.InvariantCulture);
        }

        internal static string FormatTimeForDisplayNonMilitary(this DateTime dt)
        {
            return dt.ToString("h:mm tt", CultureInfo.InvariantCulture);
        }

        internal static T Get<T>(this IDistributedCache cache, string key)
        {
            byte[] data = cache.Get(key);

            if (data != null && data.Length != 0)
            {
                return (T) Utility.ByteArrayToObject(data);
            }
            return default(T);
        }

        internal static async Task<T> GetAsync<T>(this IDistributedCache cache, string key)
        {
            byte[] data = await cache.GetAsync(key).ConfigureAwait(false);

            if (data != null && data.Length != 0)
            {
                return (T) Utility.ByteArrayToObject(data);
            }
            return default(T);
        }

        internal static DateTime GetMonthEnd(this DateTime dt)
        {
            dt = new DateTime(dt.Year, dt.Month, 1);
            dt = dt.AddMonths(1);
            return dt.AddDays(-1);
        }

        internal static DateTime GetMonthStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        internal static long GetUnixTimeStamp(this DateTime dt)
        {
            return Convert.ToInt64(Math.Round((dt - new DateTime(1970, 1, 1)).TotalSeconds, 0));
        }

        internal static object GetValue(this HttpContext httpContext, string key)
        {
            if (httpContext.Items.ContainsKey(key))
            {
                return httpContext.Items[key];
            }
            return null;
        }

        internal static DateTime GetWeekEnd(this DateTime dt)
        {
            return dt.AddDays(6).GetWeekStart();
        }

        internal static DateTime GetWeekEnd(this DateTime dt, DayOfWeek firstDay)
        {
            return dt.GetWeekStart(firstDay).AddDays(6);
        }

        internal static DateTime GetWeekStart(this DateTime dt)
        {
            return dt.GetWeekStart(DayOfWeek.Monday);
        }

        internal static DateTime GetWeekStart(this DateTime dt, DayOfWeek firstDay)
        {
            CultureInfo info = Thread.CurrentThread.CurrentCulture;
            DayOfWeek todayDow = info.Calendar.GetDayOfWeek(dt);

            int diff = todayDow - firstDay;

            if (diff == -1)
            {
                diff = 6;
            }
            return dt.AddDays(-diff);
        }

        internal static DateTime GetYearEnd(this DateTime dt)
        {
            dt = new DateTime(dt.Year, 1, 1);
            dt = dt.AddYears(1);
            return dt.AddDays(-1);
        }

        internal static DateTime GetYearStart(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1);
        }

        internal static bool HasProperty(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }

        internal static bool HasValue(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }

        internal static int LineNumber(this Exception e)
        {
            int linenum = 0;
            try
            {
                linenum = Convert.ToInt32(e.StackTrace.Substring(e.StackTrace.LastIndexOf(":line") + 5));
            }
            catch { }
            return linenum;
        }

        internal static async Task RemoveAsync(this IDistributedCache cache, string key)
        {
            await cache.RemoveAsync(key).ConfigureAwait(false);
        }

        internal static async Task SetObjectAsync(this IDistributedCache cache, string key, object value)
        {
            await cache.SetAsync(key, Utility.ObjectToByteArray(value)).ConfigureAwait(false);
        }

        internal static void SetObject(this IDistributedCache cache, string key, object value)
        {
            cache.Set(key, Utility.ObjectToByteArray(value));
        }

        internal static void SetValue(this HttpContext httpContext, string key, object value)
        {
            if (httpContext.Items.ContainsKey(key))
            {
                httpContext.Items[key] = value;
            }
            else
            {
                httpContext.Items.Add(key, value);
            }
        }

        internal static byte[] ToByteArray(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];

            for (int totalBytesCopied = 0; totalBytesCopied < stream.Length;)
            {
                totalBytesCopied += stream.Read(buffer, totalBytesCopied, Convert.ToInt32(stream.Length) - totalBytesCopied);
            }
            return buffer;
        }

        internal static long UnixTimeStamp(this DateTime dt)
        {
            return Convert.ToInt64(Math.Round((dt - new DateTime(1970, 1, 1)).TotalSeconds, 0));
        }

        internal static long UnixTimeStampInMs(this DateTime dt)
        {
            return Convert.ToInt64(Math.Round((dt - new DateTime(1970, 1, 1)).TotalMilliseconds, 0));
        }

        internal static string WithMaxLength(this string value, int maxLength)
        {
            return value?.Substring(0, Math.Min(value.Length, maxLength));
        }

        internal static List<T> Join<T>(this List<T> listA, List<T> listB)
        {
            if (listA == null)
                return listB;
            if (listB == null)
                return listA;

            return listA.Concat(listB).ToList();
        }
    }
}
