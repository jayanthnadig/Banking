using ASNRTech.CoreService.Core;
using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.Enums;
using ASNRTech.CoreService.Security;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASNRTech.CoreService.Dashboard
{
    [ApiController]
    [TeamAuthorize(AccessType.Client, false)]
    public class DashboardController : TeamControllerBase
    {
        [HttpGet]
        [Route("v1/dashboard/allwidget/{userId}")]
        public async Task<ResponseBase<List<LoadWidgets>>> GetAllWidgetAsync()
        {
            return await DashboardService.GetAllWidgetAsync(new TeamHttpContext(HttpContext)).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("v1/dashboard/chartsandconnstring/{userId}")]
        public async Task<ResponseBase<List<ChartTypeandDBConnectionString>>> DashboardDropdowns()
        {
            return await DashboardService.ChartTypeandDBConnectionString(new TeamHttpContext(HttpContext)).ConfigureAwait(false);
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
        public async Task<ResponseBase<List<OnScreenClick>>> Dashboardwidgetclick([FromBody]OnScreenClick widgetclick)
        {
            return await DashboardService.DashboardWidgetClick(new TeamHttpContext(HttpContext), widgetclick).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("v1/dashboard/gridsendemail/{userId}")]
        public async Task<ResponseBase> Gridsendemail([FromBody]OnScreenClick widgetclick)
        {
            return await DashboardService.GridSendEmail(new TeamHttpContext(HttpContext), widgetclick).ConfigureAwait(false);
        }
    }
}
