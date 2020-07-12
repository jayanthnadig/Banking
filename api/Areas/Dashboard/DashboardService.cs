using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ASNRTech.CoreService.Alcs;
using ASNRTech.CoreService.Core;
using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.Data;
using ASNRTech.CoreService.Security;
using ASNRTech.CoreService.Services;

namespace ASNRTech.CoreService.Dashboard
{
    public class DashboardService : BaseService
    {
        internal static async Task<ResponseBase<List<LoadWidgets>>> GetAllWidgetAsync(TeamHttpContext teamHttpContext)
        {
            if (teamHttpContext == null)
            {
                throw new ArgumentNullException(nameof(teamHttpContext));
            }
            //DataRequest dataRequest = new DataRequest(year, month, 0, 0);
            List<LoadWidgets> Widgets = await GenericService.GetAllWidgetAsync(teamHttpContext).ConfigureAwait(false);
            return GetTypedResponse(teamHttpContext, Widgets);
        }

        internal static async Task<ResponseBase<List<ChartTypeandDBConnectionString>>> ChartTypeandDBConnectionString(TeamHttpContext teamHttpContext)
        {
            if (teamHttpContext == null)
            {
                throw new ArgumentNullException(nameof(teamHttpContext));
            }
            return await GenericService.ChartTypeandDBConnectionString(teamHttpContext).ConfigureAwait(false);
        }

        internal static async Task<ResponseBase<List<LoadWidgets>>> AddorEditWidgetAsync(TeamHttpContext teamHttpContext, DashboardWidget details)
        {
            if (teamHttpContext == null)
            {
                throw new ArgumentNullException(nameof(teamHttpContext));
            }
            if (details.WidgetId == -1)
            {
                ResponseBase response = await GenericService.createWidget(teamHttpContext, details).ConfigureAwait(false);
                if (response.Code == HttpStatusCode.OK)
                {
                    List<LoadWidgets> Widgets = await GenericService.GetWidgetAsync(teamHttpContext, details.WidgetType, details.WidgetName);//, details.WidgetQuery,details.L1ConnectionString, details.WidgetQueryLevel1, details.L2ConnectionString, details.WidgetQueryLevel2, details.WidgetQueryLevel3, details.WidgetQueryLevel4).ConfigureAwait(false);
                    return GetTypedResponse(teamHttpContext, Widgets);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(details.WidgetId.ToString()))
                {
                    ResponseBase response = await GenericService.editWidget(teamHttpContext, details).ConfigureAwait(false);
                    if (response.Code == HttpStatusCode.OK)
                    {
                        List<LoadWidgets> Widgets = await GenericService.GetWidgetAsync(teamHttpContext, details.WidgetType, details.WidgetName);//, details.WidgetQuery, details.WidgetQueryLevel1, details.WidgetQueryLevel2, details.WidgetQueryLevel3, details.WidgetQueryLevel4).ConfigureAwait(false);
                        return GetTypedResponse(teamHttpContext, Widgets);
                    }
                }
            }
            return null;
        }

        internal static async Task<ResponseBase> DeleteWidgetAsync(TeamHttpContext teamHttpContext, int widgetId)
        {
            try
            {
                using (TeamDbContext dbContext = new TeamDbContext())
                {
                    var widget = await dbContext.UserDashboards.FindAsync(widgetId).ConfigureAwait(false);
                    if (widget != null)
                    {
                        dbContext.UserDashboards.Remove(widget);
                        await dbContext.SaveChangesAsync().ConfigureAwait(false);
                    }
                }
                return GetResponse(teamHttpContext);
            }
            catch (Exception ex)
            {
                return GetResponse(teamHttpContext, HttpStatusCode.BadRequest, ex.Message.ToString());
            }
        }

        internal static async Task<ResponseBase<List<OnScreenClick>>> DashboardWidgetClick(TeamHttpContext teamHttpContext, OnScreenClick details)
        {
            try
            {
                if (teamHttpContext == null)
                {
                    throw new ArgumentNullException(nameof(teamHttpContext));
                }
                List<OnScreenClick> GridData = await GenericService.GetGridDataAsync(teamHttpContext, details).ConfigureAwait(false);
                return GetTypedResponse(teamHttpContext, GridData);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
