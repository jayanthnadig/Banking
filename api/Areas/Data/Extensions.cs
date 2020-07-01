using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Reflection;
using ASNRTech.CoreService.Utilities;

namespace ASNRTech.CoreService.Data {
    internal static class Extensions {

        internal static bool GetBoolean(this DbDataReader dr, string fieldName) {
            return dr.GetBoolean(dr.GetOrdinal(fieldName));
        }

        internal static bool GetBooleanForString(this DbDataReader dr, string fieldName) => Convert.ToBoolean(dr.GetValue(dr.GetOrdinal(fieldName)));

        internal static string GetBooleanForDisplay(this DbDataReader dr, string fieldName) {
            string fieldValue = dr[fieldName].ToString();

            if (bool.TryParse(fieldValue, out bool result)) {
                return result ? "True" : "False";
            }
            else {
                switch (fieldValue) {
                    case "0":
                        return "False";

                    case "1":
                    case "-1":
                        return "True";

                    default:
                        break;
                }
                return fieldValue;
            }
        }

        internal static DateTime GetDate(this DbDataReader dr, string fieldName) {
            return Convert.ToDateTime(dr.GetValue(dr.GetOrdinal(fieldName)));
        }

        internal static string GetDateForMP(this DbDataReader dr, string fieldName) {
            if (DateTime.TryParse(dr[fieldName].ToString(), out DateTime dt)) {
                return dt.ToString("yyyy-MM-dd");
            }
            else {
                return dr[fieldName].ToString();
            }
        }

        internal static DateTime GetDateTime(this DbDataReader dr, string fieldName) {
            return dr.GetDateTime(dr.GetOrdinal(fieldName));
        }

        internal static byte GetByte(this DbDataReader dr, string fieldName) {
            return dr.GetByte(dr.GetOrdinal(fieldName));
        }

        internal static string GetDateTimeForMP(this DbDataReader dr, string fieldName) {
            if (DateTime.TryParse(dr[fieldName].ToString(), out DateTime dt)) {
                return dt.ToUniversalTime().ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            }
            else {
                return dr[fieldName].ToString();
            }
        }

        internal static DateTime? GetDateTimeWithNull(this DbDataReader dr, string fieldName) {
            int x = dr.GetOrdinal(fieldName);

            if (!dr.IsDBNull(x)) {
                DateTime returnValue = dr.GetDateTime(x);

                if (returnValue.Year < 1000) {
                    return null;
                }
                return returnValue;
            }

            return null;
        }

        internal static DateTime GetDateWithDefault(this DbDataReader dr, string fieldName) {
            if (DateTime.TryParse(dr[fieldName].ToString(), out DateTime returnValue)) {
                return returnValue;
            }
            else {
                return DateTime.MinValue;
            }
        }

        internal static double GetDouble(this DbDataReader dr, string fieldName) {
            return Convert.ToDouble(dr.GetValue(dr.GetOrdinal(fieldName)), CultureInfo.InvariantCulture);
        }

        internal static int GetInt32(this DbDataReader dr, string fieldName) {
            return dr.GetInt32(dr.GetOrdinal(fieldName));
        }

        internal static int GetInt16(this DbDataReader dr, string fieldName) {
            return dr.GetInt16(dr.GetOrdinal(fieldName));
        }

        internal static int GetIntWithDefault(this DbDataReader dr, string fieldName) {
            if (int.TryParse(dr[fieldName].ToString(), out int returnValue)) {
                return returnValue;
            }
            else {
                return 0;
            }
        }

        internal static string GetSqlDate(this DateTime dt) {
            return dt.ToString(DateUtility.DB_DATE_FORMAT, CultureInfo.InvariantCulture);
        }

        internal static string GetString(this DbDataReader dr, string fieldName) {
            return dr.GetString(dr.GetOrdinal(fieldName));
        }

        internal static object GetValue(this DbDataReader dr, string fieldName) {
            return dr.GetValue(dr.GetOrdinal(fieldName));
        }

        internal static string GetBlankStringIfNull(this DbDataReader dr, string fieldName) {
            int ordinal = dr.GetOrdinal(fieldName);
            if (dr.GetValue(ordinal) == DBNull.Value) {
                return string.Empty;
            }
            return dr.GetString(ordinal);
        }

        internal static void ToCSV(this DataTable dtDataTable, string strFilePath) {
            StreamWriter sw = new StreamWriter(strFilePath, false);

            //header
            for (int i = 0; i < dtDataTable.Columns.Count; i++) {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1) {
                    sw.Write(",");
                }
            }

            // data
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows) {
                for (int i = 0; i < dtDataTable.Columns.Count; i++) {
                    string value = dr[i].ToString();
                    if (value.Contains(',', StringComparison.InvariantCulture)) {
                        sw.Write($"\"{value}\"");
                    }
                    else {
                        sw.Write(dr[i].ToString());
                    }
                    if (i < dtDataTable.Columns.Count - 1) {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }

        internal static DataTable ToDataTable<T>(List<T> items) {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props) {
                //Defining type of data column gives proper data table
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items) {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++) {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        internal static DataTable MergeColumns(DataTable dt1, DataTable dt2) {
            DataTable result = new DataTable();
            foreach (DataColumn dc in dt1.Columns) {
                result.Columns.Add(new DataColumn(dc.ColumnName, dc.DataType));
            }
            foreach (DataColumn dc in dt2.Columns) {
                result.Columns.Add(new DataColumn(dc.ColumnName, dc.DataType));
            }
            for (int i = 0; i < Math.Max(dt1.Rows.Count, dt2.Rows.Count); i++) {
                DataRow dr = result.NewRow();
                if (i < dt1.Rows.Count) {
                    for (int c = 0; c < dt1.Columns.Count; c++) {
                        dr[c] = dt1.Rows[i][c];
                    }
                }
                if (i < dt2.Rows.Count) {
                    for (int c = 0; c < dt2.Columns.Count; c++) {
                        dr[dt1.Columns.Count + c] = dt2.Rows[i][c];
                    }
                }
                result.Rows.Add(dr);
            }
            return result;
        }
    }
}
