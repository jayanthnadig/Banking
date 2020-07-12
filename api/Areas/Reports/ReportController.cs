﻿using ASNRTech.CoreService.Core;
using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.Enums;
using ASNRTech.CoreService.Security;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASNRTech.CoreService.Reports
{
    [ApiController]
    [TeamAuthorize(AccessType.Client, true)]
    public class ReportController : TeamControllerBase
    {
        [HttpGet]
        [Route("v1/reports/reportnames/{userId}")]
        public async Task<ResponseBase<List<ReportList>>> ReportNames()
        {
            return await ReportService.ReportNames(new TeamHttpContext(HttpContext)).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("v1/reports/viewreport/{userId}/reportId/{reportId}")]
        public async Task<ResponseBase<List<ReportGrid>>> ViewReport(int reportId)
        {
            return await ReportService.ViewReport(new TeamHttpContext(HttpContext), reportId).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("v1/reports/editreport/{userId}/reportId/{reportId}")]
        public async Task<ResponseBase<ReportConfigAddEdit>> EditReport(int reportId)
        {
            return await ReportService.EditReport(new TeamHttpContext(HttpContext), reportId).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("v1/reports/addeditreport/{userId}")]
        public async Task<ResponseBase> AddEditReport([FromBody] ReportConfigAddEdit report)
        {
            return await ReportService.AddEditReport(new TeamHttpContext(HttpContext), report).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("v1/reports/downloadreport/{userId}/reportId/{reportId}")]
        public async Task<FileStreamResult> DownloadAsync(int reportId)
        {
            return await ReportService.DownloadAsync(new TeamHttpContext(HttpContext), reportId).ConfigureAwait(false);
        }
    }
}
