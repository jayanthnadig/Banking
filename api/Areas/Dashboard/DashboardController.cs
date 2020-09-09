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
        public async Task<ResponseBase<List<LoadDashboard>>> GetAllWidgetAsync()
        {
            return await DashboardService.GetAllWidgetAsync(new TeamHttpContext(HttpContext)).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("v1/dashboard/allwidgetdropDowns/{userId}")]
        public async Task<ResponseBase<List<AllWidgetDropDowns>>> AllWidgetDropDowns()
        {
            return await DashboardService.AllWidgetDropDowns(new TeamHttpContext(HttpContext)).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("v1/dashboard/addorupdatewidget/{userId}")]
        public async Task<ResponseBase<List<LoadWidgets>>> AddorUpdateWidgetAsync([FromBody]DashboardWidget dashboardwidget)
        {
            return await DashboardService.AddorUpdateWidgetAsync(new TeamHttpContext(HttpContext), dashboardwidget).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("v1/dashboard/editwidget/{userId}")]
        public async Task<ResponseBase<DashboardWidget>> EditWidgetAsync(int editwidgetId)
        {
            return await DashboardService.EditWidgetAsync(new TeamHttpContext(HttpContext), editwidgetId).ConfigureAwait(false);
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
