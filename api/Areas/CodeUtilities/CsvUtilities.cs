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

namespace ASNRTech.CoreService.CsvUtilities {
    internal static class HelperMethods {

        internal static List<T> ConvertByteArrayToObjectList<S, T>(byte[] data) where S : ClassMap {
            using (Stream memoryStream = new MemoryStream(data)) {
                StreamReader streamReader = new StreamReader(memoryStream);
                using (CsvReader csv = new CsvReader(streamReader)) {
                    csv.Configuration.RegisterClassMap<S>();
                    return csv.GetRecords<T>().ToList();
                }
            }
        }

        internal static List<T> ConvertCsvToObjectList<S, T>(string requestData) where S : ClassMap {
            if (!string.IsNullOrWhiteSpace(requestData)) {
                byte[] fileBytes = Convert.FromBase64String(requestData);
                try {
                    return ConvertByteArrayToObjectList<S, T>(fileBytes);
                }
                catch (Exception ex) {
                    throw new ArgumentException($"File contains some invalid values");
                }
            }
            return new List<T>();
        }

        //internal static byte[] ConvertObjectListToCsv<S, T>(IEnumerable<T> data, string headerText = "") where S : ClassMap {
        //    if (typeof(T).IsSubclassOf(typeof(ExportBaseModel))) {
        //        int rowCount = 1;
        //        foreach (T item in data) {
        //            ExportBaseModel row = item as ExportBaseModel;
        //            row.RowNumber = rowCount++;
        //        }
        //    }

        //    using (MemoryStream memoryStream = new MemoryStream())
        //    using (StreamWriter streamWriter = new StreamWriter(memoryStream))
        //    using (CsvWriter csv = new CsvWriter(streamWriter)) {
        //        if (!string.IsNullOrWhiteSpace(headerText)) {
        //            csv.WriteRecord(new HeaderRecord { HeaderText = headerText });
        //            csv.NextRecord();
        //        }
        //        csv.Configuration.RegisterClassMap<S>();
        //        csv.WriteHeader<T>();
        //        csv.NextRecord();
        //        csv.WriteRecords(data);
        //        csv.Flush();
        //        streamWriter.Flush();
        //        memoryStream.Flush();

        //        memoryStream.Position = 0;
        //        return memoryStream.ToArray();
        //    }
        //}

        //internal static byte[] ConvertObjectListToCsv<T>(List<T> data, string headerText = "") {
        //    if (typeof(T).IsSubclassOf(typeof(ExportBaseModel))) {
        //        int rowCount = 1;
        //        foreach (T item in data) {
        //            ExportBaseModel row = item as ExportBaseModel;
        //            row.RowNumber = rowCount++;
        //        }
        //    }

        //    using (MemoryStream memoryStream = new MemoryStream())
        //    using (StreamWriter streamWriter = new StreamWriter(memoryStream))
        //    using (CsvWriter csv = new CsvWriter(streamWriter)) {
        //        if (!string.IsNullOrWhiteSpace(headerText)) {
        //            csv.WriteRecord(new HeaderRecord { HeaderText = headerText });
        //            csv.NextRecord();
        //        }
        //        csv.WriteHeader<T>();
        //        csv.NextRecord();
        //        csv.WriteRecords(data);
        //        csv.Flush();
        //        streamWriter.Flush();
        //        memoryStream.Flush();

        //        memoryStream.Position = 0;
        //        return memoryStream.ToArray();
        //    }
        //}

        //internal static FileStreamResult ConvertObjectListToFileStream<S, T>(IEnumerable<T> data, string headerText, string password, string type = "csv") where S : ClassMap {
        //    string fileName = string.Empty;
        //    if (!string.IsNullOrWhiteSpace(headerText)) {
        //        fileName = headerText + $" export-{DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}.";
        //    }
        //    else {
        //        fileName = $"export-{DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}.";
        //    }
        //    if (string.Equals(type, "pdf", System.StringComparison.OrdinalIgnoreCase)) {
        //        fileName += "pdf";
        //        return HelperMethods.ConvertObjectListToPdfFileStream<S, T>(data, headerText, password, false, fileName);
        //    }
        //    else {
        //        fileName += "csv";
        //        return HelperMethods.ConvertObjectListToCsvFileStream<S, T>(data, headerText, password, fileName);
        //    }
        //}

        internal static FileStreamResult ConvertObjectListToFileStream(IEnumerable<ExpandoObject> data, string headerText, string password, bool isForView, string type = "csv") {
            string fileName = string.Empty;
            if (!string.IsNullOrWhiteSpace(headerText)) {
                fileName = headerText + $" export-{DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}.";
            }
            else {
                fileName = $"export-{DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}.";
            }
            if (string.Equals(type, "pdf", System.StringComparison.OrdinalIgnoreCase)) {
                fileName += "pdf";
                return HelperMethods.ConvertObjectListToPdfFileStream(data, headerText, password, isForView, fileName);
            }
            else {
                fileName += "csv";
                return HelperMethods.ConvertObjectListToCsvFileStream(data, headerText, password, fileName);
            }
        }

        internal static FileStreamResult ConvertObjectListToFileStream(IEnumerable<ExpandoObject> data, string headerText, string fileName = "") {
            if (string.IsNullOrWhiteSpace(fileName)) {
                fileName = $"export-{DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}.";
            }
            fileName += "csv";

            byte[] csvByteData = HelperMethods.ConvertObjectListToCsv(data, headerText);

            MemoryStream memoryStream = new MemoryStream(csvByteData);
            return new FileStreamResult(memoryStream, "text/csv") { FileDownloadName = fileName };
        }

        internal static FileStreamResult DownloadAsZipFile(MemoryStream memoryStream, string filename, string password) {
            var outputMemStream = new MemoryStream();
            if (string.IsNullOrEmpty(password)) {
                password = Utility.GetConfigValue("exports:defaultpassword");
            }
            using (var zipStream = new ZipOutputStream(outputMemStream)) {
                zipStream.Password = password;

                zipStream.SetLevel(9);
                ZipEntry newEntry = new ZipEntry(filename);
                newEntry.DateTime = DateTime.Now;
                zipStream.PutNextEntry(newEntry);

                StreamUtils.Copy(memoryStream, zipStream, new byte[4096]);
                zipStream.CloseEntry();
                newEntry.AESKeySize = 256;

                zipStream.IsStreamOwner = false;
            }

            outputMemStream.Position = 0;
            FileStreamResult result2 = new FileStreamResult(outputMemStream, "application/zip");
            result2.FileDownloadName = string.Format("{0}.zip", filename);
            return result2;
        }

        internal static (int ColumnCount, bool DataRowsExist) GetNumberOfColumnsAndRowsExist(string requestData) {
            byte[] fileBytes = Convert.FromBase64String(requestData);
            using (Stream memoryStream = new MemoryStream(fileBytes)) {
                StreamReader streamReader = new StreamReader(memoryStream);
                using (CsvReader csv = new CsvReader(streamReader)) {
                    int columnCount = 0;
                    bool dataRowsExist = false;
                    if (csv.Read()) {
                        csv.ReadHeader();

                        List<string> headers = csv.Context.HeaderRecord.ToList();

                        columnCount = headers.Count;

                        if (csv.Read()) {
                            dataRowsExist = true;
                        }
                    }
                    return (ColumnCount: columnCount, DataRowsExist: dataRowsExist);
                }
            }
        }

        private static byte[] ConvertObjectListToCsv(IEnumerable<ExpandoObject> data, string headerText = "") {
            int rowCount = 0;
            foreach (object items in data) {
                rowCount++;
                var item = (IDictionary<string, Object>) items;
                if (item.ContainsKey("S No"))
                    item["S No"] = rowCount;
            }

            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamWriter streamWriter = new StreamWriter(memoryStream))
            using (CsvWriter csv = new CsvWriter(streamWriter)) {
                if (!string.IsNullOrWhiteSpace(headerText)) {
                    csv.WriteRecord(new HeaderRecord { HeaderText = headerText });
                    csv.NextRecord();
                    foreach (var items in data) {
                        foreach (var item in (IDictionary<string, object>) items) {
                            csv.WriteField(item.Key);
                        }
                        csv.NextRecord();
                        break;
                    }
                    csv.WriteRecords(data);
                }
                else {
                    foreach (var element in data) {
                        csv.WriteRecord(element);
                        csv.NextRecord();
                    }
                }
                csv.Flush();
                streamWriter.Flush();
                memoryStream.Flush();

                memoryStream.Position = 0;
                return memoryStream.ToArray();
            }
        }

        //private static FileStreamResult ConvertObjectListToCsvFileStream<S, T>(IEnumerable<T> data, string headerText, string password, string fileName = "") where S : ClassMap {
        //    byte[] csvByteData = HelperMethods.ConvertObjectListToCsv<S, T>(data, headerText);

        //    return ConvertObjectListToCsvFileStream(csvByteData, password, fileName);
        //}

        private static FileStreamResult ConvertObjectListToCsvFileStream(IEnumerable<ExpandoObject> data, string headerText, string password, string fileName = "") {
            byte[] csvByteData = HelperMethods.ConvertObjectListToCsv(data, headerText);
            return ConvertObjectListToCsvFileStream(csvByteData, password, fileName);
        }

        //internal static void ConvertObjectListToCsvFileStream<T>(List<T> data, string fileName = "") {
        //    if (string.IsNullOrWhiteSpace(fileName)) {
        //        fileName = $"export-{DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}.csv";
        //    }
        //    byte[] csvByteData = HelperMethods.ConvertObjectListToCsv<T>(data);
        //    using (Stream file = File.OpenWrite(fileName)) {
        //        file.Write(csvByteData, 0, csvByteData.Length);
        //    }
        //}

        private static FileStreamResult ConvertObjectListToCsvFileStream(byte[] csvByteData, string password, string fileName = "") {
            if (string.IsNullOrWhiteSpace(fileName)) {
                fileName = $"export-{DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}.csv";
            }

            MemoryStream memoryStream = new MemoryStream(csvByteData);
            return DownloadAsZipFile(memoryStream, fileName, password);
        }

        //private static string ConvertObjectListToCsvString<S, T>(IEnumerable<T> data) where S : ClassMap {
        //    byte[] byteData = ConvertObjectListToCsv<S, T>(data);

        //    return Encoding.UTF8.GetString(byteData);
        //}

        internal static string ConvertObjectListToBase64String(Char[] data) {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data));
        }

        //private static FileStreamResult ConvertObjectListToPdfFileStream<S, T>(IEnumerable<T> data, string headerText, string password, bool isForView = false, string fileName = "") where S : ClassMap {
        //    byte[] csvByteData = HelperMethods.ConvertObjectListToCsv<S, T>(data, headerText);
        //    return ConvertObjectListToPdfFileStream(csvByteData, headerText, password, isForView, fileName);
        //}

        private static FileStreamResult ConvertObjectListToPdfFileStream(IEnumerable<ExpandoObject> data, string headerText, string password, bool isForView, string fileName = "") {
            byte[] csvByteData = HelperMethods.ConvertObjectListToCsv(data, headerText);

            return ConvertObjectListToPdfFileStream(csvByteData, headerText, password, isForView, fileName);
        }

        private static FileStreamResult ConvertObjectListToPdfFileStream(byte[] csvByteData, string headerText, string password, bool isForView, string fileName = "") {
            if (string.IsNullOrWhiteSpace(fileName)) {
                fileName = $"export-{DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}.pdf";
            }

            using (MemoryStream memoryStream = new MemoryStream(csvByteData))
            using (Workbook wb = new Workbook()) {
                wb.LoadFromStream(memoryStream, ",", 1, 1);
                Worksheet sheet = wb.Worksheets[0];

                for (int i = 1; i < sheet.Columns.Length; i++) {
                    sheet.SetColumnWidth(i, 18);
                    sheet.Columns[i].AutoFitColumns();
                    sheet.Columns[i].Style.WrapText = true;
                }
                wb.DefaultFontSize = 12;
                MemoryStream pdfStream = new MemoryStream();
                sheet.PageSetup.Orientation = PageOrientationType.Landscape;
                sheet.PageSetup.PaperSize = PaperSizeType.A2Paper;
                sheet.SaveToPdfStream(pdfStream);

                if (isForView) {
                    PdfDocument pdfDocument = new PdfDocument(pdfStream);
                    pdfDocument.Security.KeySize = PdfEncryptionKeySize.Key256Bit;
                    if (string.IsNullOrEmpty(password)) {
                        password = Utility.GetConfigValue("exports:defaultpassword");
                    }
                    pdfDocument.Security.OwnerPassword = password;
                    pdfDocument.Security.UserPassword = password;
                    pdfDocument.Security.Permissions = PdfPermissionsFlags.Default;
                    MemoryStream pdfDocStream = new MemoryStream();
                    pdfDocument.SaveToStream(pdfDocStream);
                    return new FileStreamResult(pdfDocStream, "application/pdf") { FileDownloadName = fileName };
                }
                return DownloadAsZipFile(pdfStream, fileName, password);
            }
        }

        internal static IActionResult DownloadFromFileStream(string filePath, string fileName) {
            try {
                if (!string.IsNullOrEmpty(filePath)) {
                    byte[] csvByteData = File.ReadAllBytes(filePath);
                    MemoryStream memoryStream = new MemoryStream(csvByteData);
                    return new FileStreamResult(memoryStream, "text/csv") { FileDownloadName = fileName };
                }
                else {
                    var response = new {
                        Code = 204,
                        Message = "No Data Found"
                    };

                    return new ObjectResult(response);
                }
            }
            catch (Exception ex) {
                throw new ArgumentException($"File not found / No Data Found");
            }
        }

        //internal static List<T> ConvertCsvToObjectList<T>(string UploadPath, TeamHttpContext teamHttpContext) {
        //    try {
        //        using (Stream memoryStream = new MemoryStream(Convert.FromBase64String(UploadPath))) {
        //            StreamReader streamReader = new StreamReader(memoryStream);
        //            using (CsvReader csv = new CsvReader(streamReader)) {
        //                OLBulkUploadCsvMap map = new OLBulkUploadCsvMap(teamHttpContext);
        //                csv.Configuration.RegisterClassMap(map);
        //                return csv.GetRecords<T>().ToList();
        //            }
        //        }
        //    }
        //    catch (Exception) {
        //        return new List<T>();
        //    }
        //}

        internal static List<T> ConvertCsvToObjectLists<S, T>(string requestData) where S : ClassMap {
            try {
                if (!string.IsNullOrWhiteSpace(requestData)) {
                    byte[] fileBytes = Convert.FromBase64String(requestData);
                    return ConvertByteArrayToObjectList<S, T>(fileBytes);
                }
                else return new List<T>();
            }
            catch (Exception ex) {
                return new List<T>();
            }
        }

        internal static FileStreamResult ConvertObjectListToFileStream_Header(string headerText, string fileName) {
            byte[] csvByteData = HelperMethods.ConvertObjectListToCsv(headerText);
            MemoryStream memoryStream = new MemoryStream(csvByteData);
            return new FileStreamResult(memoryStream, "text/csv") { FileDownloadName = fileName };
        }

        private static byte[] ConvertObjectListToCsv(string headerText) {
            List<string> ColumnNames = headerText.Split(',').ToList();
            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamWriter streamWriter = new StreamWriter(memoryStream))
            using (CsvWriter csv = new CsvWriter(streamWriter)) {
                if (!string.IsNullOrWhiteSpace(headerText)) {
                    foreach (var item in ColumnNames) {
                        csv.WriteField(item);
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
