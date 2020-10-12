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
        internal static async Task<ResponseBase<List<LoadDashboard>>> GetAllWidgetAsync(TeamHttpContext teamHttpContext)
        {
            if (teamHttpContext == null)
            {
                throw new ArgumentNullException(nameof(teamHttpContext));
            }
            //DataRequest dataRequest = new DataRequest(year, month, 0, 0);
            List<LoadDashboard> Widgets = await GenericService.GetAllWidgetAsync(teamHttpContext).ConfigureAwait(false);
            return GetTypedResponse(teamHttpContext, Widgets);
        }

        internal static async Task<ResponseBase<List<AllWidgetDropDowns>>> AllWidgetDropDowns(TeamHttpContext teamHttpContext)
        {
            if (teamHttpContext == null)
            {
                throw new ArgumentNullException(nameof(teamHttpContext));
            }
            return await GenericService.DashboardAddDropDowns(teamHttpContext).ConfigureAwait(false);
        }

        internal static async Task<ResponseBase<List<LoadWidgets>>> AddorUpdateWidgetAsync(TeamHttpContext teamHttpContext, DashboardWidget details)
        {
            if (teamHttpContext == null)
            {
                throw new ArgumentNullException(nameof(teamHttpContext));
            }
            if (details.WidgetId == -1)
            {
                ResponseBase response = await GenericService.addWidget(teamHttpContext, details).ConfigureAwait(false);
                if (response.Code == HttpStatusCode.OK)
                {
                    List<LoadWidgets> Widgets = await GenericService.GetWidgetAsync(teamHttpContext, details.DashboardChartType, details.DashboardWidgetName);//, details.WidgetQuery,details.L1ConnectionString, details.WidgetQueryLevel1, details.L2ConnectionString, details.WidgetQueryLevel2, details.WidgetQueryLevel3, details.WidgetQueryLevel4).ConfigureAwait(false);
                    return GetTypedResponse(teamHttpContext, Widgets);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(details.WidgetId.ToString()))
                {
                    ResponseBase response = await GenericService.updateWidget(teamHttpContext, details).ConfigureAwait(false);
                    if (response.Code == HttpStatusCode.OK)
                    {
                        List<LoadWidgets> Widgets = await GenericService.GetWidgetAsync(teamHttpContext, details.DashboardChartType, details.DashboardWidgetName);//, details.WidgetQuery, details.WidgetQueryLevel1, details.WidgetQueryLevel2, details.WidgetQueryLevel3, details.WidgetQueryLevel4).ConfigureAwait(false);
                        return GetTypedResponse(teamHttpContext, Widgets);
                    }
                }
            }
            return null;
        }

        public static async Task<ResponseBase<DashboardWidget>> EditWidgetAsync(TeamHttpContext teamHttpContext, int editwidgetId)
        {
            List<UserDashboard> objEditWidget = new List<UserDashboard>();
            using (TeamDbContext dbContext = new TeamDbContext())
            {
                objEditWidget = dbContext.UserDashboards.Where(x => x.Id == editwidgetId).ToList();
            }

            if (objEditWidget.Count != 0)
            {
                foreach (var item in objEditWidget)
                {
                    DashboardWidget objWidgetEdit = new DashboardWidget
                    {
                        DashboardWidgetName = item.DashboardWidgetName,
                        WidgetId = item.Id,
                        DashboardChartType = item.DashboardChartType,
                        DashboardUserPermission = item.DashboardUserPermission,
                        DashboardEmailFormat = item.DashboardEmailFormat,
                        WidgetConnectionString = item.WidgetConnectionString,
                        WidgetSchedulerType = item.WidgetSchedulerType,
                        WidgetSchedulerEmailIDs = item.WidgetSchedulerEmailIDs,
                        WidgetQuery = item.WidgetQuery,
                        Level1ConnectionString = item.Level1ConnectionString,
                        Level1SchedulerType = item.Level1SchedulerType,
                        L1SchedulerEmailIDs = item.L1SchedulerEmailIDs,
                        DashbaordQueryL1 = item.DashbaordQueryL1,
                        Level2ConnectionString = item.Level2ConnectionString,
                        Level2SchedulerType = item.Level2SchedulerType,
                        L2SchedulerEmailIDs = item.L2SchedulerEmailIDs,
                        DashbaordQueryL2 = item.DashbaordQueryL2,
                        Level3ConnectionString = item.Level3ConnectionString,
                        Level3SchedulerType = item.Level3SchedulerType,
                        L3SchedulerEmailIDs = item.L3SchedulerEmailIDs,
                        DashbaordQueryL3 = item.DashbaordQueryL3,
                        Level4ConnectionString = item.Level4ConnectionString,
                        Level4SchedulerType = item.Level4SchedulerType,
                        L4SchedulerEmailIDs = item.L4SchedulerEmailIDs,
                        DashbaordQueryL4 = item.DashbaordQueryL4,
                        WidgetSendEmail = item.WidgetSendEmail
                    };

                    return GetTypedResponse(teamHttpContext, objWidgetEdit);
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

        internal static async Task<ResponseBase> GridSendEmailSMS(TeamHttpContext teamHttpContext, GridSendEmailSMS details)
        {
            try
            {
                if (teamHttpContext == null)
                {
                    throw new ArgumentNullException(nameof(teamHttpContext));
                }
                OnScreenClick objOnScreenClick = new OnScreenClick
                {
                    ClickLevel = details.ClickLevel,
                    ClickedWidgetId = details.ClickedWidgetId,
                    ClickedOnValue = details.ClickedOnValue,
                    GridInput = details.GridInput
                };

                List<OnScreenClick> GridData = await GenericService.GetGridDataAsync(teamHttpContext, objOnScreenClick).ConfigureAwait(false);

                if (GridData.Count > 0)
                {
                    if (details.SendType == "Email")
                    {
                        if (details.SendEmailOption == "Send To")
                        {
                            //List<string> emailids = details.RecipientEmails.Split(',').ToList<string>();
                            byte[] specificByteData = HelperMethods.ConvertObjectListToCsv(GridData[0].GridData.ToList(), string.Join(",", GridData[0].GridColumns));
                            CreateDocumentForAttachment(teamHttpContext, details.ClickLevel, specificByteData, details.RecipientEmails);
                        }
                        else
                        {
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

                                    CreateDocumentForAttachment(teamHttpContext, details.ClickLevel, csvByteData, emaildata);
                                }
                            }
                        }
                    }
                    else
                    {
                        return null;
                    }
                }

                return GetResponse(teamHttpContext);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static void CreateDocumentForAttachment(TeamHttpContext teamHttpContext, string clicklevel, byte[] csvbyte, string emailid)
        {
            string FileName = @"EmailFiles" + "\\" + teamHttpContext.ContextUserId + "_" + clicklevel + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss", CultureInfo.InvariantCulture) + ".csv";
            using (FileStream document = new FileStream(FileName, FileMode.CreateNew))
            {
                document.Write(csvbyte);
                document.Flush();
            }
            SendMail(emailid, clicklevel + "_" + "Data for " + DateTime.Now.ToString("dd-MM-yyyy hh:mm tt", CultureInfo.InvariantCulture), "Please find the attached report", FileName);

            File.Delete(FileName);
        }

        private static void SendMail(string email, string subject, string body, string path)
        {
            EmailService.SendAsync(email, subject, body, path);
        }
    }
}
