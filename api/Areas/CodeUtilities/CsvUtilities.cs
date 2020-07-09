using CsvHelper;
using CsvHelper.Configuration;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Mvc;
using Spire.Pdf;
using Spire.Pdf.Security;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using ASNRTech.CoreService.Utilities;

namespace ASNRTech.CoreService.CsvUtilities
{
    internal static class HelperMethods
    {
        public static byte[] ConvertObjectListToCsv(List<string[]> data, string headerText)
        {
            List<string> ColumnNames = headerText.Split(',').ToList();
            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamWriter streamWriter = new StreamWriter(memoryStream))
            using (CsvWriter csv = new CsvWriter(streamWriter))
            {
                if (!string.IsNullOrWhiteSpace(headerText))
                {
                    foreach (var item in ColumnNames)
                    {
                        csv.WriteField(item);
                    }
                    foreach (var item in data)
                    {
                        List<string> Values = string.Join(",", item).Split(',').ToList();
                        csv.NextRecord();
                        foreach (var items in Values)
                        {
                            csv.WriteField(items);
                        }
                    }
                }
                csv.Flush();
                streamWriter.Flush();
                memoryStream.Flush();
                memoryStream.Position = 0;
                return memoryStream.ToArray();
            }
        }
    }
}
