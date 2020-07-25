using ASNRTech.CoreService.Core.Models;
using Npgsql;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Xml;

namespace ASNRTech.CoreService.Utilities
{
    internal static class DateUtility
    {
        public readonly static DateTime DEFAULT_DATE = XmlConvert.ToDateTime("1900-01-01", "yyyy-MM-dd");
        public readonly static DateTime MAX_DATE = new DateTime(2100, 1, 1);
        public const string DB_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

        internal static DateRange AllDays {
            get {
                return new DateRange(DateTime.Today.AddDays(-2000), DateTime.Today);
            }
        }

        internal static long CurrentUnixTimeStamp {
            get {
                return DateTime.UtcNow.UnixTimeStamp();
            }
        }

        internal static string CurrentUnixTimeStampAsString {
            get {
                return DateUtility.CurrentUnixTimeStamp.ToString(CultureInfo.InvariantCulture);
            }
        }

        internal static string GetCurrentDateTimeForDisplay {
            get {
                return DateUtility.FormatDateTimeForDisplay(DateTime.Now);
            }
        }

        internal static bool IsBankWorkingTime {
            get {
                DayOfWeek day = DateTime.Today.DayOfWeek;
                if (day == DayOfWeek.Saturday || day == DayOfWeek.Sunday)
                {
                    return false;
                }
                return DateUtility.TimeBetween(10, 17);
            }
        }

        internal static DateRange Last14Days {
            get {
                return new DateRange(DateTime.Today.AddDays(-13), DateTime.Today);
            }
        }

        internal static DateRange Last30Days {
            get {
                return new DateRange(DateTime.Today.AddDays(-29), DateTime.Today);
            }
        }

        internal static DateRange Last360Days {
            get {
                return new DateRange(DateTime.Today.AddDays(-359), DateTime.Today);
            }
        }

        internal static DateRange Last7Days {
            get {
                return new DateRange(DateTime.Today.AddDays(-6), DateTime.Today);
            }
        }

        internal static DateRange LastMonth {
            get {
                DateRange lastMonth = DateUtility.ThisMonth;
                lastMonth.EndDate = lastMonth.StartDate.AddDays(-1);
                lastMonth.StartDate = lastMonth.EndDate.GetMonthStart();

                return lastMonth;
            }
        }

        internal static DateRange LastWeek {
            get {
                DateRange lastWeek = DateUtility.ThisWeek;
                lastWeek.EndDate = lastWeek.StartDate.AddDays(-1);
                lastWeek.StartDate = lastWeek.EndDate.AddDays(-6);

                return lastWeek;
            }
        }

        internal static DateRange Next7Days {
            get {
                return new DateRange(DateTime.Today, DateTime.Today.AddDays(6));
            }
        }

        internal static DateRange Previous30Days {
            get {
                return new DateRange(DateTime.Today.AddDays(-59), DateTime.Today.AddDays(-29));
            }
        }

        internal static DateRange Previous7Days {
            get {
                return new DateRange(DateTime.Today.AddDays(-13), DateTime.Today.AddDays(-7));
            }
        }

        internal static DateRange PreviousMonth {
            get {
                DateRange previousMonth = DateUtility.LastMonth;
                previousMonth.EndDate = previousMonth.StartDate.AddDays(-1);
                previousMonth.StartDate = previousMonth.EndDate.GetMonthStart();

                return previousMonth;
            }
        }

        internal static DateRange ThisFortnight {
            get {
                // WTD
                CultureInfo info = Thread.CurrentThread.CurrentCulture;
                const DayOfWeek firstday = DayOfWeek.Monday;
                DateTime today = DateTime.Today;
                DayOfWeek todayDow = info.Calendar.GetDayOfWeek(today);

                int diff = todayDow - firstday;

                if (diff == -1)
                {
                    diff = 6;
                }

                // week start
                DateTime startDate = today.AddDays(-diff);

                // fortnight start
                startDate = startDate.AddDays(-7);

                return new DateRange(startDate, today);
            }
        }

        internal static DateRange ThisMonth {
            get {
                DateTime today = DateTime.Today;
                return new DateRange(today.GetMonthStart(), today);
            }
        }

        internal static DateRange ThisWeek {
            get {
                // WTD
                CultureInfo info = Thread.CurrentThread.CurrentCulture;
                const DayOfWeek firstday = DayOfWeek.Monday;
                DateTime today = DateTime.Today;
                DayOfWeek todayDow = info.Calendar.GetDayOfWeek(today);

                int diff = todayDow - firstday;

                if (diff == -1)
                {
                    diff = 6;
                }
                DateTime startDate = today.AddDays(-diff);

                return new DateRange(startDate, today);
            }
        }

        internal static DateRange ThisYear {
            get {
                DateTime today = DateTime.Today;
                DateTime endDate = today.AddDays(1);
                return new DateRange(today.GetYearStart(), endDate);
            }
        }

        internal static DateRange Today {
            get {
                return new DateRange(DateTime.Today, DateTime.Today);
            }
        }

        internal static DateRange Yesterday {
            get {
                return new DateRange(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(-1));
            }
        }

        internal static DateTime GetDateValueFromQueryString(NameValueCollection qs, string paramName)
        {
            DateTime returnValue = Convert.ToDateTime("1900-01-01", CultureInfo.InvariantCulture);

            if (qs[paramName] != null)
            {
                try
                {
                    returnValue = XmlConvert.ToDateTime(qs[paramName], "yyyy-MM-dd HH:mm:ss");
                }
                catch
                {
                }
            }
            return returnValue;
        }

        internal static DateRange GetMonthDateRange(int year, int month)
        {
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            return new DateRange(startDate, endDate);
        }

        internal static DateTime GetXmlDateValueFromDom(XmlDocument dom, string xPath)
        {
            DateTime returnValue = Convert.ToDateTime("1900-01-01");
            XmlNode selectedNode = dom.SelectSingleNode(xPath);

            if (selectedNode != null)
            {
                try
                {
                    returnValue = XmlConvert.ToDateTime(selectedNode.InnerText, "yyyy-MM-dd HH:mm:ss");
                }
                catch
                {
                }
            }
            return returnValue;
        }

        internal static decimal GetXmlDecimalValueFromDom(XmlDocument dom, string xPath)
        {
            decimal returnValue = 0;
            XmlNode selectedNode = dom.SelectSingleNode(xPath);

            if (selectedNode != null)
            {
                try
                {
                    returnValue = XmlConvert.ToDecimal(selectedNode.InnerText);
                }
                catch
                {
                }
            }
            return returnValue;
        }

        internal static bool DateTimeIsBetweenDateRange(DateTime sourceDateTime, DateRange dateRange)
        {
            return sourceDateTime.CompareTo(dateRange.StartDate) > 0 && sourceDateTime.CompareTo(dateRange.EndDate) < 0;
        }

        internal static string FormatCurrentDateForDisplay()
        {
            return DateTime.Today.FormatDateForDisplay();
        }

        internal static string FormatDateTimeForDisplay(DateTime? dt)
        {
            if (dt.HasValue)
            {
                return Convert.ToDateTime(dt).ToString("dd/MM/yyyy HH:mm");
            }
            else
            {
                return "";
            }
        }

        internal static string FormatTimeForDisplay(double time)
        {
            return DateUtility.FormatTimeForDisplay(Convert.ToDecimal(time));
        }

        internal static string FormatTimeForDisplay(decimal time)
        {
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(time));

            if (ts.Hours == 0)
            {
                return ts.ToString(@"mm\:ss", CultureInfo.InvariantCulture);
            }
            else
            {
                return ts.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture);
            }
        }

        internal static DateTime GetCurrentIst()
        {
            return GetIst(DateTime.UtcNow);
        }

        internal static NpgsqlParameter GetDateParam(string paramName, DateTime date)
        {
            NpgsqlParameter returnValue = new NpgsqlParameter(paramName, DbType.DateTime);

            if (date != DateTime.MinValue)
            {
                returnValue.Value = date;
            }

            return returnValue;
        }

        internal static string GetDateTimeForDisplayFromUnixTimeStamp(object secs)
        {
            try
            {
                double dblSecs = double.Parse((string) secs, CultureInfo.CurrentCulture);
                return DateUtility.GetDateTimeFromUnixTimeStamp(dblSecs).ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            }
            catch { }
            return "0";
        }

        internal static DateTime GetDateTimeFromUnixTimeStamp(double secs)
        {
            return (new DateTime(1970, 1, 1).AddSeconds(secs)).ToLocalTime();
        }

        internal static DateTime GetIst(DateTime time)
        {
            return time.AddHours(5.5);
        }

        // get 9AM today if before 9, else 9AM tomorrow.
        internal static DateTime GetNext9Am()
        {
            if (DateTime.Now.Hour < 9)
            {
                return DateTime.Today.Date.AddHours(9);
            }
            else
            {
                return DateTime.Today.Date.AddDays(1).AddHours(9);
            }
        }

        internal static DateRange GetPreviousHalfMonthDate()
        {
            DateRange returnValue = DateUtility.GetPreviousHalfDate(DateTime.Now);
            returnValue.EndDate = returnValue.EndDate.AddDays(-1);

            return DateUtility.GetPreviousHalfDate(returnValue.EndDate);
        }

        internal static DateRange GetRecentHalfMonthDate()
        {
            return DateUtility.GetPreviousHalfDate(DateTime.Now);
        }

        internal static DateTime GetTimeFromString(string time)
        {
            return DateTime.ParseExact(time, "HH:mm", System.Globalization.CultureInfo.InvariantCulture);
        }

        internal static DateTime ParseDate(this string s)
        {
            if (s == null)
            {
                return DateTime.MinValue;
            }

            try
            {
                string[] formats = new string[] {
          "MM/dd/yyyy HH:mm:ss",
          "dd-MM-yyyy HH:mm:ss",
          "yyyy-dd-MM HH:mm:ss",
          "yyyy-MM-ddTHH:mm:ssK",
          "yyyy-MM-dd"
        };
                return DateTime.ParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        internal static bool TimeBetween(double startHour, double endHour)
        {
            double minsSinceStartOfDay = (DateTime.Now - DateTime.Today).TotalMinutes;

            return minsSinceStartOfDay > (startHour * 60) && minsSinceStartOfDay < (endHour * 60);
        }

        /*
          when current day is:
              <= 15: end-date will be end of the previous month
              > 15: end-date will be 15th of current month
            start-date will be 15 days before end-date
        */

        private static DateRange GetPreviousHalfDate(DateTime dt)
        {
            DateTime returnValue;

            if (dt.Day <= 15)
            {
                // get the first of the month
                returnValue = new DateTime(dt.Year, dt.Month, 1);

                // get last day of previous month
                returnValue = returnValue.AddDays(-1);
            }
            else
            {
                returnValue = new DateTime(dt.Year, dt.Month, 15);
            }

            return new DateRange(returnValue.AddDays(-15), returnValue);
        }
    }
}
