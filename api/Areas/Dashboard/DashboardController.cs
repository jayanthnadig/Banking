using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASNRTech.CoreService.Core;
using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.Enums;
using ASNRTech.CoreService.Security;

namespace ASNRTech.CoreService.Dashboard
{
    [ApiController]
    //[TeamAuthorize(AccessType.Admin | AccessType.Client, true)]
    public class DashboardController : TeamControllerBase
    {
        [HttpGet]
        [Route("v1/dashboard/allwidget/{userId}")]
        public async Task<ResponseBase<List<LoadWidgets>>> GetAllWidgetAsync()
        {
            return await DashboardService.GetAllWidgetAsync(new TeamHttpContext(HttpContext)).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("v1/dashboard/addoreditwidget/{userId}")]
        public async Task<ResponseBase<List<LoadWidgets>>> AddorEditWidgetAsync([FromBody]DashboardWidget dashboardwidget)
        {
            return await DashboardService.AddorEditWidgetAsync(new TeamHttpContext(HttpContext), dashboardwidget).ConfigureAwait(false);
        }

        [HttpDelete]
        [Route("v1/dashboard/deletewidget/{userId}/widgetId/{widgetId}")]
        public async Task<ResponseBase> Delete(int widgetId)
        {
            return await DashboardService.DeleteWidgetAsync(new TeamHttpContext(HttpContext), widgetId).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("v1/dashboard/dashboardwidgetclick/{userId}")]
        public async Task<ResponseBase<List<OnScreenClick>>> dashboardwidgetclick([FromBody]OnScreenClick widgetclick)
        {
            return await DashboardService.DashboardWidgetClick(new TeamHttpContext(HttpContext), widgetclick).ConfigureAwait(false);
        }
    }
}
