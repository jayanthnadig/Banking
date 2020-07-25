using ASNRTech.CoreService.Alcs;
using ASNRTech.CoreService.Core;
using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.CsvUtilities;
using ASNRTech.CoreService.Data;
using ASNRTech.CoreService.Email;
using ASNRTech.CoreService.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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

        internal static async Task<ResponseBase> GridSendEmail(TeamHttpContext teamHttpContext, OnScreenClick details)
        {
            try
            {
                if (teamHttpContext == null)
                {
                    throw new ArgumentNullException(nameof(teamHttpContext));
                }
                List<OnScreenClick> GridData = await GenericService.GetGridDataAsync(teamHttpContext, details).ConfigureAwait(false);
                var columnnames = from t in GridData[0].GridColumns.Where(x => x.Equals("email") || x.Equals("Email") || x.Equals("EMAIL") || x.Equals("EMail") || x.Equals("e-Mail") || x.Equals("e-mail") || x.Equals("E-mail") || x.Equals("E-Mail") || x.Contains("E-MAIL")) select t;

                int indexOfEmail = -1;
                foreach (var result in columnnames)
                {
                    indexOfEmail = GridData[0].GridColumns.Select((item, i) => new
                    {
                        Item = item,
                        Position = i
                    }).Where(m => m.Item.Equals(result)).First().Position;
                }

                if (indexOfEmail != -1)
                {
                    List<string> emaillist = new List<string>();

                    foreach (var email in GridData[0].GridData)
                    {
                        var list1 = email[indexOfEmail];
                        if (!emaillist.Contains(list1))
                            emaillist.Add(list1);
                    }

                    foreach (var emaildata in emaillist)
                    {
                        var userdatalist = GridData[0].GridData.Where(x => x.Contains(emaildata)).ToList();
                        byte[] csvByteData = HelperMethods.ConvertObjectListToCsv(userdatalist, string.Join(",", GridData[0].GridColumns));
                        string FileName = @"EmailFiles" + "\\" + teamHttpContext.ContextUserId + "_" + details.ClickLevel + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss", CultureInfo.InvariantCulture) + ".csv";
                        using (FileStream document = new FileStream(FileName, FileMode.CreateNew))
                        {
                            document.Write(csvByteData);
                            document.Flush();
                        }
                        //SendMail(emaildata, details.ClickLevel + "_" + "Data for " + DateTime.Now.ToString("dd-MM-yyyy hh:mm"), "Please find the attached report", FileName);
                        SendMail(emaildata, details.ClickLevel + "_" + "Data for " + DateTime.Now.ToString("dd-MM-yyyy hh:mm tt", CultureInfo.InvariantCulture), "Please find the attached report", FileName);

                        File.Delete(FileName);
                    }
                }

                return GetResponse(teamHttpContext);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static void SendMail(string email, string subject, string body, string path)
        {
            EmailService.SendAsync(email, subject, body, path);
        }
    }
}
