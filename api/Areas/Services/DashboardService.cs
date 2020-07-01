using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ASNRTech.CoreService.Core;
using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.Dashboard;
using ASNRTech.CoreService.Data;
using ASNRTech.CoreService.Enums;
using ASNRTech.CoreService.Security;
using ASNRTech.CoreService.Services;
using ASNRTech.CoreService.Utilities;
using Npgsql;

namespace ASNRTech.CoreService.Alcs {
    public class DashboardService : BaseService {

        internal static async Task<List<LoadWidgets>> GetAllWidgetAsync(TeamHttpContext httpContext) {
            string connString = Utility.GetConnectionString("DefaultConnection");
            //string userid = httpContext.CurrentUser.UserId;
            string userid = "Admin";
            List<LoadWidgets> returnValue = new List<LoadWidgets>();
            PostgresService postgresService = new PostgresService();
            List<UserDashboard> objUserDashboard = new List<UserDashboard>();
            using (TeamDbContext dbContext = new TeamDbContext()) {
                objUserDashboard = dbContext.UserDashboards.Where(x => x.DashboardUserId == userid).ToList();
            }

            if (objUserDashboard.Count != 0) {
                foreach (var item in objUserDashboard) {
                    List<WidgetRead> objWidgetRead = new List<WidgetRead>();
                    using (NpgsqlDataReader sqlData = postgresService.ExecuteSqlReturnReader(connString, item.DashbaordQuery)) {
                        while (sqlData.Read()) {
                            objWidgetRead.Add(new WidgetRead {
                                Name = sqlData.GetBlankStringIfNull("name"),
                                Count = sqlData.GetInt32("count")
                            });
                        }
                    }
                    LoadWidgets newItem = new LoadWidgets {
                        WidgetName = item.DashboardWidgetName,
                        WidgetID = item.DashboardId,
                        WidgetType = item.DashboardChartType,
                        WidgetData = objWidgetRead.ToArray()
                    };
                    returnValue.Add(newItem);
                }
            }
            //List<Transactions> TransactionList = new List<Transactions>();
            //using (TeamDbContext dbContext = new TeamDbContext()) {
            //    TransactionList = dbContext
            //    .Transactions
            //    .ToList();
            //}
            return returnValue;
        }

        internal static async Task<List<LoadWidgets>> GetWidgetAsync(TeamHttpContext httpContext, string widgettype, string widgetname, string widgetquery) {
            string connString = Utility.GetConnectionString("DefaultConnection");
            //string userid = httpContext.CurrentUser.UserId;
            string userid = "Admin";
            List<LoadWidgets> returnValue = new List<LoadWidgets>();
            PostgresService postgresService = new PostgresService();
            List<UserDashboard> objUserDashboard = new List<UserDashboard>();
            using (TeamDbContext dbContext = new TeamDbContext()) {
                objUserDashboard = dbContext.UserDashboards.Where(x => x.DashboardUserId == userid && x.DashboardChartType == widgettype && x.DashboardWidgetName == widgetname).ToList();
            }

            if (objUserDashboard.Count != 0) {
                foreach (var item in objUserDashboard) {
                    List<WidgetRead> objWidgetRead = new List<WidgetRead>();
                    using (NpgsqlDataReader sqlData = postgresService.ExecuteSqlReturnReader(connString, item.DashbaordQuery)) {
                        while (sqlData.Read()) {
                            objWidgetRead.Add(new WidgetRead {
                                Name = sqlData.GetBlankStringIfNull("name"),
                                Count = sqlData.GetInt32("count")
                            });
                        }
                    }

                    LoadWidgets newItem = new LoadWidgets {
                        WidgetName = item.DashboardWidgetName,
                        WidgetID = item.DashboardId,
                        WidgetType = item.DashboardChartType,
                        WidgetData = objWidgetRead.ToArray()
                    };
                    returnValue.Add(newItem);
                }
            }
            return returnValue;

            //string connString = Utility.GetConnectionString("DefaultConnection");
            ////string userid = httpContext.CurrentUser.UserId;
            //string userid = "Admin";
            //PostgresService postgresService = new PostgresService();
            ////postgresService.AddParam("@userId", httpContext.CurrentUser.UserId);
            //postgresService.AddParam("@userId", "Admin");

            //const string sql = @"select ud_id
            //                   , ud_userid
            //                   , ud_chartid
            //                   , ud_chartname
            //                   , ud_query
            //                   from    public.""User_Dashboard""
            //                   where ud_userid = @userId
            //                   order by ud_id; ";

            //List<UserDashboard> objUserDashboard = new List<UserDashboard>();
            //using (NpgsqlDataReader sqlData = postgresService.ExecuteSqlReturnReader(connString, sql)) {
            //    while (sqlData.Read()) {
            //        objUserDashboard.Add(new UserDashboard {
            //            DashboardId = sqlData.GetInt32("ud_id"),
            //            DashboardUserId = sqlData.GetString("ud_userid"),
            //            DashboardWidgetName = sqlData.GetBlankStringIfNull("ud_widgetname"),
            //            DashbaordQuery = sqlData.GetBlankStringIfNull("ud_query"),
            //            DashboardChartType = sqlData.GetBlankStringIfNull("ud_charttype") //Utility.ParseEnum<Charts>
            //        });
            //    }
            //}

            //List<UserDashboard> objUserDashboard1 = new List<UserDashboard>();
            //using (TeamDbContext dbContext = new TeamDbContext()) {
            //    objUserDashboard1 = dbContext.UserDashboards.Where(x => x.DashboardUserId == userid && x.DashboardChartType == widgettype && x.DashboardChartType == widgetname).ToList();
            //}

            //List<Transactions> TransactionList = new List<Transactions>();
            //using (TeamDbContext dbContext = new TeamDbContext()) {
            //    TransactionList = dbContext
            //    .Transactions
            //    .ToList();
            //}

            //if (objUserDashboard.Count != 0) {
            //}

            ////return GetTypedResponse<List<Widgets>>(httpContext, TransactionList);

            ////string connString = Utility.GetConnectionString("DefaultConnection");
            ////PostgresService postgresService = new PostgresService();
            //postgresService.AddParam("@userId", httpContext.CurrentUser.UserId);

            //SqlServerService sqlServer = new SqlServerService();
            ////sqlServer.AddParam("@userId", httpContext.CurrentUser.UserId);

            ////string connString = Utility.GetConnectionString("Miicrras");
            ////PostgresService postgresService = new PostgresService();
            //SqlServerService sqlService = new SqlServerService();

            //sqlService.AddParam("@userId", "Test");

            ////const string sql = query;

            ////            const string sql = @"select t1.status as Name, COUNT(t1.status) as Count
            ////from [dbo].[Transactions] t1
            ////join Transactions t2 on t1.Id = t2.Id group by t1.status";

            //List<WidgetRead> widget1 = new List<WidgetRead>();
            //using (SqlDataReader sqlData = sqlService.ExecuteSqlReturnReader(connString, widgetquery)) {
            //    while (sqlData.Read()) {
            //        widget1.Add(new WidgetRead {
            //            Name = sqlData.GetBlankStringIfNull("Name"),
            //            //WidgetName = sqlData.GetString("UD_Chart_Name"),
            //            //WidgetValues = sqlData.GetBlankStringIfNull("UD_Query")
            //        });
            //    }
            //}

            //List<Widgets> widget = new List<Widgets>();
            //using (SqlDataReader sqlData = sqlService.ExecuteSqlReturnReader(connString, widgetquery)) {
            //    while (sqlData.Read()) {
            //        widget.Add(new Widgets {
            //            ChartID = sqlData.GetInt32("UD_Chart_Id"),
            //            WidgetName = sqlData.GetString("UD_Chart_Name"),
            //            WidgetValues = sqlData.GetBlankStringIfNull("UD_Query")
            //        });
            //    }
            //}

            //List<LoadWidgets> returnValue = new List<LoadWidgets>();

            ////widgwtName:[{ Authrized: 300,unauthrized: 400},{ Authrized: 300,unauthrized: 400}]

            //LoadWidgets newItem = new LoadWidgets {
            //    //ChartID = widgetid,
            //    WidgetName = widgetname,
            //    WidgetType = null,
            //    //WidgetValues = "Authrized: 300,unauthrized: 400"
            //};
            //returnValue.Add(newItem);
            //return returnValue;
        }

        //private static void AddUserDashboard(UserDashboard userdashboard) {
        //    //userdashboard.DashboardId = user.UserId.ToUpper(CultureInfo.InvariantCulture);
        //    using (TeamDbContext dbContext = new TeamDbContext()) {
        //        if (dbContext.Users.FirstOrDefault(e => e.UserId == user.UserId) == null) {
        //            dbContext.Users.Add(user);
        //            dbContext.SaveChanges();
        //        }
        //    }
        //}

        //internal async static Task<ResponseBase> createWidget(TeamHttpContext httpContext, DashboardWidget widgetdata) {
        //    AddUserDashboard(new UserDashboard {
        //        das = "Admin",
        //        FirstName = "Admin",
        //        LastName = "Admin",
        //        Email = "admin@asnrtech.com",
        //        UserType = UserType.Admin,
        //        Source = "DIRECT",
        //        Password = Utility.GetMd5Hash("testing_1")
        //    });

        //    using (TeamDbContext dbContext = new TeamDbContext()) {
        //    }

        //        SqlServerService sqlServer = new SqlServerService();
        //    string candID = candiadate.candidateID;
        //    bool isPanMandatory = false;
        //    string procname = ProcedureNames.UPDATE_CANDIATE;
        //    if (string.IsNullOrEmpty(candiadate.candidateID)) {
        //        candID = GetCandID();
        //        procname = ProcedureNames.CREATE_CANDIATE;
        //    }

        //    sqlServer.AddParam("@clientId", httpContext.ContextClientId);
        //    sqlServer.AddParam("@name", candiadate.Name);
        //    sqlServer.AddParam("@jobloc", candiadate.JobLoc);
        //    sqlServer.AddParam("@designation", candiadate.Designation);
        //    sqlServer.AddParam("@email", candiadate.Email);
        //    sqlServer.AddParam("@dob", candiadate.Dob);
        //    sqlServer.AddParam("@mobile", candiadate.Mobile);
        //    sqlServer.AddParam("@aadhar", candiadate.Aadhar);
        //    sqlServer.AddParam("@contractstart", candiadate.ContractFrom);
        //    sqlServer.AddParam("@zone", candiadate.Zone);
        //    sqlServer.AddParam("@jobcategory", candiadate.JobCateogry);
        //    sqlServer.AddParam("@industrytype", candiadate.IndustryType);
        //    sqlServer.AddParam("@contractend", candiadate.ContractTo);
        //    sqlServer.AddParam("@repmanager", candiadate.ReportingManager);
        //    sqlServer.AddParam("@mgremail", candiadate.ManagerEmail);
        //    sqlServer.AddParam("@mgrmobile", candiadate.ManagerMobile);
        //    sqlServer.AddParam("@salarytype", candiadate.SalaryType);
        //    sqlServer.AddParam("@salary", candiadate.Amount);
        //    sqlServer.AddParam("@source", candiadate.RequestType);
        //    sqlServer.AddParam("@candid", candID);
        //    sqlServer.AddParam("@LOGINID", httpContext.CurrentUser.UserId);
        //    sqlServer.AddParam("@state", candiadate.state);
        //    sqlServer.AddParam("@breakupcode", candiadate.Paygroupcode);
        //    sqlServer.AddParam("@requirement", candiadate.Requirement);
        //    using (DataSet sqlData = sqlServer.ExecuteProcedureReturnDataSet(Utility.MiicrrasConnString, procname)) {
        //        if (sqlData.Tables[0].Rows[0]["message"].ToString().Contains("approval")) {
        //            return GetResponse(httpContext, HttpStatusCode.PartialContent, sqlData.Tables[0].Rows[0]["message"].ToString());
        //        }
        //        else {
        //            if (candiadate.SalaryType.ToUpper() == "CTC") {
        //                if (Convert.ToInt32(candiadate.Amount) > 300000) {
        //                    isPanMandatory = true;
        //                }
        //            }
        //            else {
        //                if ((Convert.ToInt32(candiadate.Amount) * 12) > 300000) {
        //                    isPanMandatory = true;
        //                }
        //            }
        //            dynamic obj = new { candID, isPanMandatory };
        //            return GetResponse(httpContext, obj);
        //        }
        //    }
        //}

        //internal static void TransactionDetails(TeamHttpContext httpContext, string message, ContractExtendDetails extendDetails) {
        //    using (TeamDbContext dbContext = new TeamDbContext()) {
        //        dbContext.uploadLogs.Add(new UploadLog() {
        //            ClientId = httpContext.ContextClientId,
        //            AmId = extendDetails.amid,
        //            ErrorMessage = message,
        //            type = (int) UploadType.Contract
        //        });
        //        dbContext.SaveChanges();
        //    }
        //}
    }
}
