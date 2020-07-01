using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASNRTech.CoreService.Core;
using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.Enums;
using ASNRTech.CoreService.Security;

namespace ASNRTech.CoreService.Dashboard {
    [ApiController]
    //[TeamAuthorize(AccessType.Admin | AccessType.Client, true)]
    public class DashboardController : TeamControllerBase {

        [HttpGet]
        [Route("v1/dashboard/widget/{userId}/widgettype{widgettype}/widgetname{widgetname}/widgetquery{widgetquery}")]
        public async Task<ResponseBase<List<LoadWidgets>>> GetWidgetAsync(string widgettype, string widgetname, string widgetquery) {
            return await DashboardService.GetWidgetAsync(new TeamHttpContext(HttpContext), widgettype, widgetname, widgetquery).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("v1/dashboard/allwidget/{userId}")]
        public async Task<ResponseBase<List<LoadWidgets>>> GetAllWidgetAsync() {
            return await DashboardService.GetAllWidgetAsync(new TeamHttpContext(HttpContext)).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("v1/dashboard/widget/{userId}/widgettype{widgettype}/widgetname{widgetname}/widgetquery{widgetquery}")]
        public async Task<ResponseBase<List<LoadWidgets>>> PostWidgetAsync([FromBody]DashboardWidget dashboardwidget) {
            return await DashboardService.PostWidgetAsync(new TeamHttpContext(HttpContext), dashboardwidget).ConfigureAwait(false);
        }

        //[HttpGet]
        //[Route("v1/dashboard/widget/{userId}")]
        //public ResponseBase<List<Transactions>> GetTransactions() {
        //    return DashboardService.GetTransactions(new TeamHttpContext(HttpContext));
        //}
    }
}
