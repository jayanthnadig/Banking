using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using TeamLease.CssService.Core;
using TeamLease.CssService.Core.Models;
using TeamLease.CssService.Enums;
using TeamLease.CssService.Security;

namespace TeamLease.CssService.Reports {
  [ApiController]
  [TeamAuthorize(AccessType.Anchor | AccessType.Client, true)]
  public class ReportController : TeamControllerBase {

    [HttpGet]
    [Route("v1/{clientId}/reports/{reportId}/details")]
    public ResponseBase<ReportDto> GetColumns(int reportId) {
      return ReportService.Get(new TeamHttpContext(HttpContext), reportId);
    }

    [HttpGet]
    [Route("v1/{clientId}/reports/custom")]
    public PagedResponse<ReportDto> GetCustom() {
      return ReportService.GetReports(new TeamHttpContext(HttpContext), ReportType.Custom);
    }

    [HttpGet]
    [Route("v1/{clientId}/reports/share")]
    public PagedResponse<ReportShareListEntry> GetShares(int year, int month) {
      return ReportService.GetShares(new TeamHttpContext(HttpContext), year, month);
    }

    [HttpPost]
    [Route("v1/{clientId}/reports/share")]
    public async Task<PagedResponse<ReportShareListEntry>> Share([FromBody] dynamic data) {
      List<int> userIds = data.userIds.ToObject<List<int>>();
      List<int> reportIds = data.reportIds.ToObject<List<int>>();
      int year = data.year;
      int month = data.month;
      string type = data.type;
      string password = data.password;
      return await ReportService.ShareAsync(new TeamHttpContext(HttpContext), userIds, reportIds, year, month, type, password).ConfigureAwait(false);
    }

    [HttpGet]
    [Route("v1/{clientId}/reports/standard")]
    public PagedResponse<ReportDto> GetStandard() {
      return ReportService.GetReports(new TeamHttpContext(HttpContext), ReportType.Standard);
    }

    [HttpPost]
    [Route("v1/{clientId}/reports/custom")]
    public ResponseBase SaveCustomReport([FromBody] ReportDto report) {
      return ReportService.SaveCustomReport(new TeamHttpContext(HttpContext), report);
    }

    [HttpGet]
    [Route("v1/{clientId}/reports/{reportId}/download")]
    public async Task<FileStreamResult> DownloadAsync(int reportId, int year, int month, string type, string password) {
      (int id, FileStreamResult fileStreamResult, string s3Url) = await ReportService.DownloadAsync(new TeamHttpContext(HttpContext), reportId, year, month, type, password, false, false).ConfigureAwait(false);
      return fileStreamResult;
    }

    [HttpDelete]
    [Route("v1/{clientId}/reports/{reportId}")]
    public async Task<ResponseBase> Delete(int reportId) {
      return await ReportService.DeleteAsync(new TeamHttpContext(HttpContext), reportId).ConfigureAwait(false);
    }

    [HttpPost]
    [Route("v1/{clientId}/reports/preview")]
    public async Task<ResponseBase<string>> PreviewAsync([FromBody] ReportDto report, int year, int month, string type, string password) {
      return await ReportService.PreviewAsync(new TeamHttpContext(HttpContext), report, year, month, type, password).ConfigureAwait(false);
    }

    [HttpGet]
    [Route("v1/{clientId}/reports/view")]
    public async Task<ResponseBase<string>> ViewAsync(int reportId, int year, int month, string password) {
      return await ReportService.ViewReportsSignedUrl(new TeamHttpContext(HttpContext), reportId, year, month, password).ConfigureAwait(false);
    }

    [HttpGet]
    [Route("v1/{clientId}/reports/{accessKey}")]
    public async Task<object> DownloadReportWithAccessKey(string accessKey) {
      return await ReportService.DownloadAsync(new TeamHttpContext(HttpContext), accessKey).ConfigureAwait(false);
    }

    [HttpGet]
    [Route("v1/{clientId}/reports/{reportId}/reportData")]
    public async Task<PagedResponse<ExpandoObject>> GetReportData(int reportId, int year, int month, int offset, int limit) {
      return await ReportService.GetReportDataAsync(new TeamHttpContext(HttpContext), reportId, year, month, offset, limit).ConfigureAwait(false);
    }

    [HttpGet]
    [Route("v1/{clientId}/reports/opsreport/reportData")]
    public async Task<PagedResponse<OpsReportDto>> GetOpsReportData(int year, int month, int offset, int limit) {
      return await ReportService.GetOpsDetailsAsync(new TeamHttpContext(HttpContext), month, year, offset, limit).ConfigureAwait(false);
    }

    [HttpGet]
    [Route("v1/{clientId}/reports/opsreport/download")]
    public async Task<FileStreamResult> ExportOpsReportData(int year, int month, int offset, int limit, string exportType, string password) {
      return await ReportService.ExportOpsReportAsync(new TeamHttpContext(HttpContext), year, month, exportType, password).ConfigureAwait(false);
    }
  }
}
