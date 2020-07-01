using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Globalization;
using ASNRTech.CoreService.Utilities;

namespace ASNRTech.CoreService.CsvUtilities {
  public class EnumConverter : DefaultTypeConverter {

    public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) {
      if (value == null) {
        return string.Empty;
      }
      return value.ToString();
    }
  }
  public class HeaderRecord {
    public string HeaderText { get; set; }
  }

  public class DefaultDateConverter : DefaultTypeConverter {

    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) {
      if (string.IsNullOrEmpty(text)) {
        return null;
      }
      else if (DateTime.TryParse(text, CultureInfo.CreateSpecificCulture("en-GB"), DateTimeStyles.None, out DateTime dtValue)) {
        return dtValue;
      }
      return DateTime.MinValue;
    }

    internal static string ConvertToString(object value, string dateFormat) {
      if (value == null) {
        return string.Empty;
      }
      if (value.GetType() == typeof(DateTime)) {
        return ((DateTime) value).ToString(dateFormat, CultureInfo.InvariantCulture);
      }
      return value.ToString();
    }
  }

  public class DDMMMYYYYConverter : DefaultDateConverter {

    public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) {
      return ConvertToString(value, Constants.DEFAULT_DATE_FORMAT);
    }
  }

  public class IndiaDateConverter : DefaultDateConverter {

    public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) {
      return ConvertToString(value, Constants.INDIA_DATE_FORMAT);
    }
  }

  public class IndiaDateTimeConverter : DefaultDateConverter {

    public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) {
      return ConvertToString(value, "dd/MM/yyyy HH:mm");
    }
  }

  public class IntConverter : DefaultTypeConverter {

    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) {
      if (int.TryParse(text, out int intValue)) {
        return intValue;
      }
      return -1;
    }

    public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) {
      return value.ToString();
    }
  }

  public class BooleanConverter : DefaultTypeConverter {

    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) {
      if (string.IsNullOrEmpty(text)) {
        return false;
      }
      if (bool.TryParse(text, out var b)) {
        return b;
      }

      if (short.TryParse(text, out var sh)) {
        if (sh == 0) {
          return false;
        }
        if (sh == 1) {
          return true;
        }
      }

      if (char.ToUpper(text[0]) == 'Y') {
        return true;
      }
      else if (char.ToUpper(text[0]) == 'N') {
        return false;
      }

      return false;
    }
  }
}
