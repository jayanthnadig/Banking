using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Spire.Pdf;
using Spire.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TeamLease.CssService.Alcs;
using TeamLease.CssService.AmazonUtils;
using TeamLease.CssService.Client;
using TeamLease.CssService.Core;
using TeamLease.CssService.Core.Models;
using TeamLease.CssService.CsvUtilities;
using TeamLease.CssService.Data;
using TeamLease.CssService.Email;
using TeamLease.CssService.Enums;
using TeamLease.CssService.Inputs;
using TeamLease.CssService.Security;
using TeamLease.CssService.Services;
using TeamLease.CssService.Utilities;

namespace TeamLease.CssService.Reports {
  public class ReportService : BaseService {

    internal static async Task<ResponseBase> DeleteAsync(TeamHttpContext teamHttpContext, int reportId) {
      using (TeamDbContext dbContext = new TeamDbContext()) {
        ReportDto report = await dbContext.Reports.FindAsync(reportId).ConfigureAwait(false);

        if (report != null && report.ClientId == teamHttpContext.ContextClientId && report.Type == ReportType.Custom) {
          dbContext.Reports.Remove(report);
          await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
      }

      return GetResponse(teamHttpContext);
    }

    internal static async Task<(int Id, FileStreamResult fileStreamResult, string s3Url)> DownloadAsync(TeamHttpContext teamHttpContext, int reportId, int year, int month, string exportType, string password, bool isBsrReport, bool isForView = false) {
      List<ReportColumnMaster> columns;
      ReportDto report = new ReportDto();

      using (TeamDbContext dbContext = new TeamDbContext()) {
        report = Get(teamHttpContext, reportId).Data;
        columns = report.Columns.Where(e => e.Selected).OrderBy(e => e.Order).ToList();
      }
      if (string.IsNullOrEmpty(report.Procedure)) {
        report.Procedure = Constants.PROC_CSS_GET_CUSTOM_REPORT_DETAILS;
      }

      List<ExpandoObject> data = await RunReport(teamHttpContext, year, month, columns, report).ConfigureAwait(false);

      // creating file-stream result to the downloaded report data
      FileStreamResult fileStreamReport = null;
      if (isBsrReport) {
        fileStreamReport = HelperMethods.ConvertObjectListToFileStream(data, GetHeaderValue(report.Name));
      }
      else {
        fileStreamReport = HelperMethods.ConvertObjectListToFileStream(data, GetHeaderValue(report.Name), password, isForView, exportType);
      }

      // storing report to s3
      string s3ReportUrl = await SaveReportToS3(teamHttpContext, fileStreamReport).ConfigureAwait(false);

      int Id = SaveReportInstance(teamHttpContext, reportId, year, month, exportType, s3ReportUrl);

      fileStreamReport.FileStream.Position = 0;

      //returning report-instance id and report as file-stream result
      return (Id, fileStreamReport, s3ReportUrl);
    }

    private static async Task<string> GetReportTableAsync(TeamHttpContext teamHttpContext, int year, int month, int reportId) {
      List<ExpandoObject> returnValue = new List<ExpandoObject>();
      List<ReportColumnMaster> columns;
      ReportDto report = new ReportDto();

      using (TeamDbContext dbContext = new TeamDbContext()) {
        report = Get(teamHttpContext, reportId).Data;
        columns = report.Columns.Where(e => e.Selected).OrderBy(e => e.Order).ToList();
      }
      if (string.IsNullOrEmpty(report.Procedure)) {
        report.Procedure = Constants.PROC_CSS_GET_CUSTOM_REPORT_DETAILS;
      }

      using (SqlDataReader sqlData = await AlcsReportService.Get(teamHttpContext, report.Procedure, year, month, 0, Constants.DEFAULT_MAX_LIMIT).ConfigureAwait(false)) {
        StringBuilder sb = new StringBuilder();
        sb.Append(Constants.TABLE_STYLE);
        sb.Append("<h2 " + Constants.REPORT_HEADER_STYLE + " >\n" + report.Name + " </h2>");
        sb.Append("<TABLE>\n");

        int SLNo = 1;
        bool firstRow = true;
        if (sqlData.HasRows) {
          while (sqlData.Read()) {
            ExpandoObject expando = new ExpandoObject();
            sb.Append("<TR>\n");

            if (firstRow) {
              firstRow = false;
              if (report.Procedure != Constants.PROC_CSS_GET_CUSTOM_REPORT_DETAILS) {
                List<ReportColumnMaster> dbReportColumnMasters = new List<ReportColumnMaster>();

                // read all columns from the data-reader
                for (int i = 0; i < sqlData.FieldCount; i++) {
                  dbReportColumnMasters.Add(new ReportColumnMaster { ColumnName = sqlData.GetName(i), DisplayName = sqlData.GetName(i) });
                }

                // get column names
                HashSet<string> allPGColumnNames = new HashSet<string>(columns.Select(s => s.ColumnName));

                // find columns not present in config, but present in reader
                List<ReportColumnMaster> remainingColumns = dbReportColumnMasters.Where(m => !allPGColumnNames.Contains(m.ColumnName) && m.ColumnName != "Sl.No").ToList();

                // add them to column collection
                columns.AddRange(remainingColumns);
              }
              sb.Append("<TR>\n");
              foreach (ReportColumnMaster item in columns) {
                sb.Append("<TH style='min-width:" + item.Width + "px;padding-right:15px;'>");
                sb.Append(item.DisplayName.ToUpper(CultureInfo.InvariantCulture));
                sb.Append("</TH>");
              }
              sb.Append("</TR> <!-- HEADER -->\n");
            }

            foreach (ReportColumnMaster item in columns) {
              sb.Append("<TD>");
              try {
                if (item.ColumnName == "S No") {
                  sb.Append(SLNo++);
                }
                else {
                  object value = sqlData.GetValue(item.ColumnName);
                  sb.Append(GetStringValue(item.DataType, value));
                }
              }
              catch {
                // if any column missing writing column name with empty data to report
              }
              finally {
                sb.Append("</TD>");
              }
            }

            sb.Append("</TR>\n");
          }
        }
        else {
          sb.Append("<TR>\n");
          foreach (ReportColumnMaster item in columns) {
            sb.Append("<TH style='min-width:" + item.Width + "px;padding-right:15px;'>");
            sb.Append(item.DisplayName);
            sb.Append("</TH>");
          }
          sb.Append("</TR> <!-- HEADER -->\n");
        }

        sb.Append("</TABLE>");

        return sb.ToString();
      }
    }

    internal static async Task<object> DownloadAsync(TeamHttpContext httpContext, string accessKey) {
      string s3Url = string.Empty;

      using (TeamDbContext dbContext = new TeamDbContext(httpContext)) {
        ReportShare reportShare = dbContext.ReportShares.FirstOrDefault(e => e.AccessKey == accessKey);
        if (reportShare.Recipient != httpContext.CurrentUser.Id) {
          return GetNotFoundResponse(httpContext, "report does not exists with this user");
        }
        if (reportShare != null) {
          ReportInstance reportInstance = dbContext.ReportInstances.FirstOrDefault(e => e.Id == reportShare.ReportInstanceId);

          if (reportInstance != null) {
            if (reportShare.AccessHistory == null) {
              reportShare.AccessHistory = new List<DateTime>();
              reportShare.AccessHistory.Add(DateTime.Now);
            }
            else {
              reportShare.AccessHistory.Add(DateTime.Now);
            }

            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            s3Url = reportInstance.StorageUrl;
          }
        }
      }

      if (!string.IsNullOrWhiteSpace(s3Url)) {
        byte[] reportData = await S3.DownloadFileAsync(s3Url).ConfigureAwait(false);

        return new FileStreamResult(new MemoryStream(reportData), "application/zip") { FileDownloadName = "report.zip" };
      }

      return new FileStreamResult(null, "");
    }

    internal static async Task<FileStreamResult> ExportOpsReportAsync(TeamHttpContext teamHttpContext, int year, int month, string exportType, string password) {
      PagedResponse<OpsReportDto> opsReportDtos = await ReportService.GetOpsDetailsAsync(teamHttpContext, month, year, 0, Constants.DEFAULT_MAX_LIMIT).ConfigureAwait(false);

      return HelperMethods.ConvertObjectListToFileStream<OpsReportDtoCsvMap, OpsReportDto>(opsReportDtos.DataEntries, Constants.HEADER_OPS_REPORT, password, exportType);
    }

    internal static ResponseBase<ReportDto> Get(TeamHttpContext teamContext, int reportId) {
      bool newCustomReport = reportId < 1;
      using (TeamDbContext dbContext = new TeamDbContext()) {
        ReportDto currentReport;

        if (newCustomReport) {
          currentReport = new ReportDto {
            Type = ReportType.Custom
          };
        }
        else {
          currentReport = dbContext.Reports.Find(reportId);
          if (currentReport == null) {
            return GetNotFoundResponse<ReportDto>(teamContext);
          }
        }

        if (currentReport.Type == ReportType.Custom) {
          // get base custom report
          ReportDto baseReport = dbContext.Reports.FirstOrDefault(e => e.Name == Constants.REPORT_CUSTOM_ALL_COLUMNS);
          if (baseReport == null) {
            return GetNotFoundResponse<ReportDto>(teamContext, "Base Custom Report not found");
          }

          // get all custom report columns
          currentReport.Columns = dbContext.ReportColumnsMaster.Where(e => e.ReportId == baseReport.Id && !e.Deleted).ToList();

          if (!newCustomReport) {
            // existing report, so get selected columns
            List<ReportColumn> paramsOfThisReport = dbContext.ReportColumns.Where(e => e.ReportId == reportId && !e.Deleted).ToList();

            currentReport.Columns.ForEach(_ => {
              if (paramsOfThisReport.Exists(e => e.ColumnId == _.Id)) {
                _.Selected = true;
              }
            });
          }
        }
        else {
          currentReport.Columns = dbContext.ReportColumnsMaster.Where(e => e.ReportId == reportId && !e.Deleted).ToList();

          currentReport.Columns.ForEach(_ => _.Selected = true);
        }

        return GetTypedResponse<ReportDto>(teamContext, currentReport);
      }
    }

    internal static async Task<PagedResponse<OpsReportDto>> GetOpsDetailsAsync(TeamHttpContext teamHttpContext, int month, int year, int offset, int limit) {
      SqlServerService sqlServer = new SqlServerService();
      sqlServer.AddParam("@UserId", teamHttpContext.CurrentUser.UserId);
      sqlServer.AddParam("@ProcessingMonth", month);
      sqlServer.AddParam("@ProcessingYear", year);
      sqlServer.AddParam("@offset", offset);
      sqlServer.AddParam("@limit", limit);

      List<OpsReportDto> returnValue = new List<OpsReportDto>();
      using (SqlDataReader sqlData = await sqlServer.ExecuteProcedureReturnReaderAsync(Utility.MiicrrasConnString, Constants.PROC_OPS_REPORT).ConfigureAwait(false)) {
        while (sqlData.Read()) {
          OpsReportDto opsReport = new OpsReportDto {
            ClientId = sqlData.GetBlankStringIfNull("client_id"),
            NoBankDetails = sqlData.GetInt32("no_bpm"),
            StopPay = sqlData.GetInt32("stop_pay"),
            DojPending = sqlData.GetIntWithDefault("pending_doj"),
          };

          returnValue.Add(opsReport);
        }
      }
      List<InputConfig> Inputconfigs = new List<InputConfig>();
      using (TeamDbContext dbContext = new TeamDbContext()) {
        HashSet<string> clientIDs = new HashSet<string>(returnValue.Select(s => s.ClientId));
        Inputconfigs = dbContext.InputConfig.Where(m => m.Year == year
            && m.Month == month
            && clientIDs.Contains(m.ClientId)).ToList();
      }

      foreach (OpsReportDto opsReportDto in returnValue) {
        InputConfig pgInputconfigDto = Inputconfigs.Find(x => x.ClientId == opsReportDto.ClientId);

        if (pgInputconfigDto != null) {
          opsReportDto.InputSaved = true;
          opsReportDto.InputSubmited = (pgInputconfigDto.AlcsSyncStatus == InputAlcsSyncStatusOfClient.Success);

          opsReportDto.BsrStatus = pgInputconfigDto.BsrStatus;
          opsReportDto.BsrApproved = pgInputconfigDto.BsrStatus == BsrStatus.Approved;
          opsReportDto.InvoiceGenerationStatus = pgInputconfigDto.InvoiceGenerationStatus;
          opsReportDto.InvoiceViewed = pgInputconfigDto.InvoiceViewed;
        }
      }
      int count = await AlcsReportService.GetCount(teamHttpContext, Constants.REPORT_OPS, year, month).ConfigureAwait(false);
      return GetPagingResponse<OpsReportDto>(teamHttpContext, returnValue, count);
    }

    internal static async Task<PagedResponse<ExpandoObject>> GetReportDataAsync(TeamHttpContext teamHttpContext, int reportId, int year, int month, int offset, int limit) {
      List<ReportColumnMaster> columns;
      ReportDto report = new ReportDto();
      List<ExpandoObject> data = new List<ExpandoObject>();

      using (TeamDbContext dbContext = new TeamDbContext()) {
        report = Get(teamHttpContext, reportId).Data;
        columns = report.Columns.Where(e => e.Selected).OrderBy(e => e.Order).ToList();
      }

      using (SqlDataReader sqlData = await AlcsReportService.Get(teamHttpContext, report.Procedure, year, month, offset, limit).ConfigureAwait(false)) {
        while (sqlData.Read()) {
          ExpandoObject expando = new ExpandoObject();

          foreach (ReportColumnMaster item in columns) {
            try {
              object value = sqlData.GetValue(item.ColumnName);

              expando.TryAdd(item.ColumnName, GetStringValue(item.DataType, value));
            }
            catch {
              // if any column missing writing column name with empty data to report
              expando.TryAdd(item.ColumnName, "");
            }
          }

          data.Add(expando);
        }
      }
      int count = await AlcsReportService.GetCount(teamHttpContext, report.Name, year, month).ConfigureAwait(false);
      return GetPagingResponse<ExpandoObject>(teamHttpContext, data, count);
    }

    internal static int GetReportId(TeamHttpContext teamHttpContext, string reportName) {
      using (TeamDbContext dbContext = new TeamDbContext()) {
        ReportDto report = dbContext.Reports.FirstOrDefault(e => e.Name == reportName && !e.Deleted);

        if (report != null) {
          return report.Id;
        }
      }
      return -1;
    }

    internal static PagedResponse<ReportDto> GetReports(TeamHttpContext teamHttpContext, ReportType reportType) {
      using (TeamDbContext dbContext = new TeamDbContext()) {
        List<ReportDto> reports = dbContext.Reports.Where(e => e.Type == reportType
            && !e.Deleted
            && (string.IsNullOrWhiteSpace(e.ClientId) || e.ClientId == teamHttpContext.ContextClientId)).ToList();

        return GetPagingResponse(teamHttpContext, reports);
      }
    }

    internal static PagedResponse<ReportShareListEntry> GetShares(TeamHttpContext httpContext, int year, int month) {
      List<ReportShare> shares;
      List<ReportDto> reports;
      List<ReportShareListEntry> returnValue = new List<ReportShareListEntry>();

      DateRange dateRange = DateUtility.GetMonthDateRange(year, month);

      // get the users with access to this client
      List<User> users = ClientService.GetUsers(httpContext);

      using (TeamDbContext dbContext = new TeamDbContext(httpContext)) {
        // get all reports for this client
        reports = dbContext.Reports.Where(e => (string.IsNullOrWhiteSpace(e.ClientId) || e.ClientId == httpContext.ContextClientId) && !e.Deleted).ToList();

        // get the shares for this client in this month
        shares = dbContext.ReportShares.Where(e => e.ClientId == httpContext.ContextClientId
            && e.CreatedOn >= dateRange.StartDate
            && e.CreatedOn <= dateRange.EndDate
            && !e.Deleted).ToList();
      }

      foreach (ReportShare item in shares) {
        // set recipient-name and report-name
        item.RecipientName = users.Find(e => e.Id == item.Recipient)?.Name();
        item.ReportName = reports.Find(e => e.Id == item.ReportId)?.Name;
      }

      // get distinct shares
      IEnumerable<string> uniqueShares = shares.Select(e => e.ShareId).Distinct();

      foreach (string item in uniqueShares) {
        // get the share-record in this share
        List<ReportShare> reportShares = shares.Where(e => e.ShareId == item).ToList();

        // for each share, set the recipients and reports
        ReportShareListEntry newEntry = new ReportShareListEntry {
          ShareId = item,
          CreatedOn = reportShares[0].CreatedOn,
          Recipients = shares.Where(e => e.ShareId == item).Select(e => e.RecipientName).Distinct(),
          Reports = shares.Where(e => e.ShareId == item).Select(e => e.ReportName).Distinct()
        };

        returnValue.Add(newEntry);
      }

      // reverse chronological order
      returnValue = returnValue.OrderByDescending(e => e.CreatedOn).ToList();

      return GetPagingResponse<ReportShareListEntry>(httpContext, returnValue);
    }

    internal static async Task<ResponseBase<string>> PreviewAsync(TeamHttpContext teamHttpContext, ReportDto report, int year, int month, string type, string password) {
      // create a report
      int reportId = ReportService.SaveCustomReport(teamHttpContext, report).Data;

      // get the data
      var s3SignedUrl = await ReportService.ViewReportsSignedUrl(teamHttpContext, reportId, year, month, password).ConfigureAwait(false);

      // delete the temp report
      await ReportService.DeleteAsync(teamHttpContext, reportId).ConfigureAwait(false);

      return s3SignedUrl;
    }

    internal static ResponseBase SaveCustomReport(TeamHttpContext teamHttpContext, ReportDto report) {
      bool newReport = (report.Id < 1);
      ReportDto currentReport;

      using (TeamDbContext dbContext = new TeamDbContext()) {
        if (newReport) {
          currentReport = new ReportDto {
            ClientId = teamHttpContext.ContextClientId,
            Name = report.Name,
            Type = ReportType.Custom
          };
          dbContext.Reports.Add(currentReport);
        }
        else {
          currentReport = dbContext.Reports.Find(report.Id);

          if (currentReport == null) {
            return GetNotFoundResponse(teamHttpContext, "Report not found");
          }

          currentReport.Name = report.Name;
        }

        dbContext.SaveChanges();

        // remove all currently selected columns
        dbContext.ReportColumns.RemoveRange(dbContext.ReportColumns.Where(e => e.ReportId == currentReport.Id));

        // add new columns
        report.Columns.Where(e => e.Selected).ToList().ForEach((_) => {
          ReportColumn newParam = new ReportColumn { ReportId = currentReport.Id, ColumnId = _.Id };
          dbContext.ReportColumns.Add(newParam);
        });
        dbContext.SaveChanges();
      }

      return GetResponse(teamHttpContext, currentReport.Id);
    }

    internal static async Task<string> SaveReportToS3(TeamHttpContext teamHttpContext, FileStreamResult fileStreamResult) {
      byte[] data = fileStreamResult.FileStream.ToByteArray();
      string base64FormatData = Convert.ToBase64String(data);
      string bucketName = Utility.GetConfigValue("awsConfig:buckets:reports_uploads");
      string contentType = fileStreamResult.ContentType;
      return await S3.UploadFileAsync(base64FormatData, bucketName, contentType, true).ConfigureAwait(false);
      ;
    }

    internal static async Task<PagedResponse<ReportShareListEntry>> ShareAsync(TeamHttpContext httpContext, List<int> userIds, List<int> reportIds, int year, int month, string exportType, string password) {
      string shareId = Guid.NewGuid().ToString();
      List<User> users = new List<User>();
      List<ReportDto> reports = new List<ReportDto>();
      const string BODY_HTML = "The following report(s) has been shared with you by {0}.<br/<br/>{1}";

      string serverUrl = Utility.GetConfigValue("serverUrl");

      using (TeamDbContext dbContext = new TeamDbContext(httpContext)) {
        // get reports
        foreach (int reportId in reportIds) {
          ReportDto report = dbContext.Reports.FirstOrDefault(e => e.Id == reportId && !e.Deleted);

          // TODO: Check user has access to share
          if (report != null) {
            if (!string.IsNullOrWhiteSpace(report.ClientId) && (report.ClientId != httpContext.ContextClientId)) {
              return GetPagingResponse<ReportShareListEntry>(httpContext, HttpStatusCode.Unauthorized, "Unauthorized");
            }
            reports.Add(report);
          }
        }

        // get email-ids
        foreach (int userId in userIds) {
          User user = dbContext.Users.FirstOrDefault(e => e.Id == userId && !e.Deleted);

          if (user != null) {
            users.Add(user);
          }
        }

        // run the reports and store in s3
        List<KeyValuePair<int, int>> reportInstances = new List<KeyValuePair<int, int>>();

        foreach (int reportId in reportIds) {
          // for each reportId creating report and storing in s3 and writing report-instance id to report-share table
          (int reportInstanceId, FileStreamResult fs, string s3Url) = await DownloadAsync(httpContext, reportId, year, month, exportType, password, false).ConfigureAwait(false);
          reportInstances.Add(new KeyValuePair<int, int>(reportId, reportInstanceId));
        }
        string reportLinks = string.Empty;

        // share each report, user combination
        foreach (int userId in userIds) {
          reportLinks = string.Empty;

          foreach (KeyValuePair<int, int> reportInstance in reportInstances) {
            //  for each userId storing a record in report-share table for a reportId
            string reportAccessKey = Guid.NewGuid().ToString();

            dbContext.ReportShares.Add(new ReportShare {
              ClientId = httpContext.ContextClientId,
              ShareId = shareId,
              Recipient = userId,
              ReportId = reportInstance.Key,
              AccessKey = reportAccessKey,
              ReportInstanceId = reportInstance.Value
            });

            ReportDto report = reports.Find(e => e.Id == reportInstance.Key);
            if (report != null) {
              reportLinks += $"<a href='{serverUrl}/reports/{reportAccessKey}'>{report.Name}</a><br/>";
            }
          }

          // send email
          string emailAddress = users.Find(e => e.Id == userId)?.Email;
          if (!string.IsNullOrWhiteSpace(emailAddress)) {
            string body = string.Format(BODY_HTML, httpContext.CurrentUser.Name(), reportLinks);

            await EmailService.SendAsync(emailAddress, "Report(s) Shared", body).ConfigureAwait(false);
          }
        }

        await dbContext.SaveChangesAsync().ConfigureAwait(false);
      }

      return GetShares(httpContext, DateTime.Today.Year, DateTime.Today.Month);
    }

    internal static async Task<ResponseBase<string>> ViewReportsSignedUrl(TeamHttpContext teamHttpContext, int reportId, int year, int month, string password) {
      ReportDto report = new ReportDto();
      using (TeamDbContext dbContext = new TeamDbContext()) {
        report = Get(teamHttpContext, reportId).Data;
      }
      //formatting table string from report data
      var reportTableString = await GetReportTableAsync(teamHttpContext, year, month, reportId).ConfigureAwait(false);

      //saving file as html
      byte[] byteArray = Encoding.ASCII.GetBytes(reportTableString);
      MemoryStream stream = new MemoryStream(byteArray);
      string fileName = report.Name + $" export-{DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}.";
      FileStreamResult fs = new FileStreamResult(new MemoryStream(byteArray), "text/html") { FileDownloadName = fileName + ".html" };

      //storing it into s3
      string s3ReportUrl = await SaveReportToS3(teamHttpContext, fs).ConfigureAwait(false);
      var reportSignedUrl = S3.GetSignedUrl(s3ReportUrl, report.Name);
      return GetTypedResponse<string>(teamHttpContext, reportSignedUrl);
    }

    private static string GetHeaderValue(string reportName) {
      switch (reportName) {
        case Constants.REPORT_ASSOCIATE_MASTER_CTC_ACTIVE_ASSOCIATES:
          return Constants.HEADER_ASSOCIATE_MASTER_CTC_ACTIVE_ASSOCIATES;

        case Constants.REPORT_CONTRACT_EXTENSION:
          return Constants.HEADER_CONTRACT_EXTENSION;

        case Constants.REPORT_INDUCTION:
          return Constants.HEADER_INDUCTION;

        case Constants.REPORT_SALARY_REGISTER:
          return Constants.HEADER_SALARY_REGISTER;

        case Constants.REPORT_SALARY_REVISION:
          return Constants.HEADER_SALARY_REVISION;

        case Constants.REPORT_STOP_PAYMENT:
          return Constants.HEADER_STOP_PAYMENT;

        case Constants.REPORT_CLIENT_SPECIFIC_EMP_MASTER:
          return Constants.HEADER_RPT_EMPLOYEEMASTER_CLIENTSPECIFIC;

        case Constants.REPORT_CUSTOM_ALL_COLUMNS:
          return Constants.HEADER_CUSTOM_REPORT;

        case Constants.REPORT_BILLING_SALARY_REGISTER:
          return Constants.HEADER_BILLING_SALARY_REGISTER_REPORT;

        default:
          return reportName;
      }
    }

    private static string GetStringValue(ReportColumnDataType dataType, object value) {
      switch (dataType) {
        case ReportColumnDataType.StringType:
          break;

        case ReportColumnDataType.IntType:
          break;

        case ReportColumnDataType.DoubleType:
          break;

        case ReportColumnDataType.DateTimeType:
          break;

        default:
          return value.ToString();
      }

      return value.ToString();
    }

    private static async Task<List<ExpandoObject>> RunReport(TeamHttpContext teamHttpContext, int year, int month, List<ReportColumnMaster> columns, ReportDto report) {
      List<ExpandoObject> returnValue = new List<ExpandoObject>();
      using (SqlDataReader sqlData = await AlcsReportService.Get(teamHttpContext, report.Procedure, year, month, 0, Constants.DEFAULT_MAX_LIMIT).ConfigureAwait(false)) {
        bool firstRow = true;
        if (sqlData.HasRows) {
          while (sqlData.Read()) {
            ExpandoObject expando = new ExpandoObject();

            //for first row and for standard reports only
            if (firstRow && report.Procedure != Constants.PROC_CSS_GET_CUSTOM_REPORT_DETAILS) {
              firstRow = false;
              List<ReportColumnMaster> dbReportColumnMasters = new List<ReportColumnMaster>();

              // read all columns from the data-reader
              for (int i = 0; i < sqlData.FieldCount; i++) {
                dbReportColumnMasters.Add(new ReportColumnMaster { ColumnName = sqlData.GetName(i), DisplayName = sqlData.GetName(i) });
              }

              // get column names
              HashSet<string> allPGColumnNames = new HashSet<string>(columns.Select(s => s.ColumnName));

              // find columns not present in config, but present in reader
              List<ReportColumnMaster> remainingColumns = dbReportColumnMasters.Where(m => !allPGColumnNames.Contains(m.ColumnName) && m.ColumnName != "Sl.No").ToList();

              // add them to column collection
              columns.AddRange(remainingColumns);
            }

            foreach (ReportColumnMaster item in columns) {
              try {
                if (item.ColumnName == "S No") {
                  expando.TryAdd(item.ColumnName, "");
                }
                else {
                  object value = sqlData.GetValue(item.ColumnName);
                  expando.TryAdd(item.ColumnName.ToUpper(CultureInfo.InvariantCulture), GetStringValue(item.DataType, value));
                }
              }
              catch {
                // if any column missing writing column name with empty data to report
                //expando.TryAdd(item.ColumnName, ""); // for BSR report
              }
            }

            returnValue.Add(expando);
          }
        }
        else {
          ExpandoObject expando = new ExpandoObject();
          foreach (ReportColumnMaster item in columns) {
            expando.TryAdd(item.ColumnName, "");
          }
          returnValue.Add(expando);
        }
      }

      return returnValue;
    }

    private static int SaveReportInstance(TeamHttpContext teamHttpContext, int reportId, int year, int month, string exportType, string s3ReportUrl) {
      //parameter data contains the parameters passed to reports download
      var parameterdata = new { year, month, exportType };
      int Id = 0;

      // storing report s3 url into report instance table
      using (TeamDbContext dbContext = new TeamDbContext()) {
        ReportInstance reportInstance = new ReportInstance {
          ClientId = teamHttpContext.ContextClientId,
          ReportId = reportId,
          ParameterData = JsonConvert.SerializeObject(parameterdata),
          StorageUrl = s3ReportUrl
        };
        dbContext.ReportInstances.Add(reportInstance);
        dbContext.SaveChanges();
        Id = reportInstance.Id;
      }

      return Id;
    }
  }
}
