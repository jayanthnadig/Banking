using ASNRTech.CoreService.Core;
using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.Dashboard;
using ASNRTech.CoreService.Data;
using ASNRTech.CoreService.Services;
using ASNRTech.CoreService.Utilities;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ASNRTech.CoreService.Alcs
{
    public class GenericService : BaseService
    {
        internal static async Task<List<LoadDashboard>> GetAllWidgetAsync(TeamHttpContext httpContext)
        {
            string userid = httpContext.ContextUserId;
            List<LoadDashboard> returnValue = new List<LoadDashboard>();
            PostgresService postgresService = new PostgresService();
            List<UserDashboard> objUserDashboard = new List<UserDashboard>();
            using (TeamDbContext dbContext = new TeamDbContext())
            {
                //objUserDashboard = dbContext.UserDashboards.Where(x => x.DashboardUserId == userid && x.Deleted == false).ToList(); //user specific
                //objUserDashboard = dbContext.UserDashboards.Where(x => x.Deleted == false).ToList(); // For all users
                objUserDashboard = dbContext.UserDashboards.Where(x => x.Deleted == false && x.DashboardUserPermission.Contains(httpContext.ContextUserId)).ToList(); // For permitted  users
            }

            if (objUserDashboard.Count != 0)
            {
                foreach (var item in objUserDashboard)
                {
                    List<WidgetRead> objWidgetRead = new List<WidgetRead>();
                    string connString = Utility.GetConnectionString(item.WidgetConnectionString);
                    using (NpgsqlDataReader sqlData = postgresService.ExecuteSqlReturnReader(connString, item.WidgetQuery))
                    {
                        while (sqlData.Read())
                        {
                            objWidgetRead.Add(new WidgetRead
                            {
                                Name = sqlData.GetBlankStringIfNull("name"),
                                Count = sqlData.GetInt32("count")
                            });
                        }
                    }
                    LoadDashboard newItem = new LoadDashboard
                    {
                        DashboardWidgetName = item.DashboardWidgetName,
                        DashboardWidgetId = item.Id,
                        DashboardWidgetType = item.DashboardChartType,
                        DashbaordWidgetData = objWidgetRead.ToArray()
                    };
                    returnValue.Add(newItem);
                }
            }

            return returnValue;
        }

        public static async Task<ResponseBase<List<AllWidgetDropDowns>>> DashboardAddDropDowns(TeamHttpContext teamHttpContext)
        {
            List<ChartTypes> objChartTypes = new List<ChartTypes>();
            List<string> objUserList = new List<string>();
            List<SchedulerTypes> objSchedulerTypes = new List<SchedulerTypes>();
            List<DBConnectionStrings> objDBConnectionStrings = new List<DBConnectionStrings>();
            List<AllWidgetDropDowns> objDashboardAddDropDowns = new List<AllWidgetDropDowns>();
            using (TeamDbContext dbContext = new TeamDbContext())
            {
                var charttypes = from ct in dbContext.ChartTypes
                                 select new
                                 {
                                     ChartId = ct.ChartId,
                                     ChartName = ct.ChartName
                                 };
                foreach (var item in charttypes)
                {
                    ChartTypes chartList = new ChartTypes
                    {
                        ChartId = item.ChartId,
                        ChartName = item.ChartName
                    };

                    objChartTypes.Add(chartList);
                }

                var userlists = from ul in dbContext.Users
                                select new
                                {
                                    UserId = ul.UserId
                                };
                foreach (var item in userlists)
                {
                    objUserList.Add(item.UserId);
                }

                var schedulertypes = from sd in dbContext.Schedulers
                                     select new
                                     {
                                         Id = sd.Id,
                                         SchedulerName = sd.SchedulerName
                                     };
                foreach (var item in schedulertypes)
                {
                    SchedulerTypes schedulerType = new SchedulerTypes
                    {
                        SchedulerId = item.Id,
                        SchedulerName = item.SchedulerName
                    };

                    objSchedulerTypes.Add(schedulerType);
                }

                var dbconnectionstrings = from dbcs in dbContext.DBConnections
                                          select new
                                          {
                                              DBConnectionId = dbcs.DBConnectionId,
                                              DBConnectionName = dbcs.DBConnectionName
                                          };

                foreach (var item in dbconnectionstrings)
                {
                    DBConnectionStrings connectionList = new DBConnectionStrings
                    {
                        DBConnectionId = item.DBConnectionId,
                        DBConnectionName = item.DBConnectionName
                    };

                    objDBConnectionStrings.Add(connectionList);
                }
            }

            AllWidgetDropDowns finallist = new AllWidgetDropDowns
            {
                Charts = objChartTypes,
                Users = objUserList,
                Schedulers = objSchedulerTypes,
                ConnectionStrings = objDBConnectionStrings
            };

            objDashboardAddDropDowns.Add(finallist);

            return GetTypedResponse(teamHttpContext, objDashboardAddDropDowns);
        }

        internal static async Task<List<LoadWidgets>> GetWidgetAsync(TeamHttpContext httpContext, string widgettype, string widgetname)//, string widgetquery, string widgetquerylevel1, string widgetquerylevel2, string widgetquerylevel3, string widgetquerylevel4)
        {
            string userid = httpContext.ContextUserId;
            List<LoadWidgets> returnValue = new List<LoadWidgets>();
            PostgresService postgresService = new PostgresService();
            List<UserDashboard> objUserDashboard = new List<UserDashboard>();
            using (TeamDbContext dbContext = new TeamDbContext())
            {
                objUserDashboard = dbContext.UserDashboards.Where(x => x.CreatedBy == userid && x.DashboardChartType == widgettype && x.DashboardWidgetName == widgetname && x.Deleted == false).ToList();
            }

            if (objUserDashboard.Count != 0)
            {
                foreach (var item in objUserDashboard)
                {
                    List<WidgetRead> objWidgetRead = new List<WidgetRead>();
                    string connString = Utility.GetConnectionString(item.WidgetConnectionString);
                    using (NpgsqlDataReader sqlData = postgresService.ExecuteSqlReturnReader(connString, item.WidgetQuery))
                    {
                        while (sqlData.Read())
                        {
                            objWidgetRead.Add(new WidgetRead
                            {
                                Name = sqlData.GetBlankStringIfNull("name"),
                                Count = sqlData.GetInt32("count")
                            });
                        }
                    }

                    LoadWidgets newItem = new LoadWidgets
                    {
                        DashboardWidgetName = item.DashboardWidgetName,
                        WidgetId = item.Id,
                        DashboardChartType = item.DashboardChartType,
                        DashboardUserPermission = item.DashboardUserPermission,
                        DashboardEmailFormat = item.DashboardEmailFormat,
                        WidgetData = objWidgetRead.ToArray(),
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
                    returnValue.Add(newItem);
                }
            }
            return returnValue;
        }

        private static void AddUserDashboard(UserDashboard userdashboard)
        {
            using (TeamDbContext dbContext = new TeamDbContext())
            {
                if (dbContext.UserDashboards.FirstOrDefault(e => e.Id == userdashboard.Id) == null)
                {
                    dbContext.UserDashboards.Add(userdashboard);
                    dbContext.SaveChanges();
                }
            }
        }

        private static void UpdateUserDashboard(UserDashboard userdashboard)
        {
            using (TeamDbContext dbContext = new TeamDbContext())
            {
                if (dbContext.UserDashboards.AsNoTracking().FirstOrDefault(e => e.Id == userdashboard.Id && e.Deleted == false) != null)
                {
                    dbContext.UserDashboards.UpdateRange(userdashboard);
                    dbContext.SaveChanges();
                }
            }
        }

        internal async static Task<ResponseBase> addWidget(TeamHttpContext httpContext, DashboardWidget widgetdata)
        {
            try
            {
                AddUserDashboard(new UserDashboard
                {
                    DashboardWidgetName = widgetdata.DashboardWidgetName,
                    DashboardChartType = widgetdata.DashboardChartType,
                    DashboardUserPermission = widgetdata.DashboardUserPermission,
                    DashboardEmailFormat = widgetdata.DashboardEmailFormat,
                    WidgetConnectionString = widgetdata.WidgetConnectionString,
                    WidgetSchedulerType = widgetdata.WidgetSchedulerType,
                    WidgetSchedulerEmailIDs = widgetdata.WidgetSchedulerEmailIDs,
                    WidgetQuery = widgetdata.WidgetQuery,
                    Level1ConnectionString = widgetdata.Level1ConnectionString,
                    Level1SchedulerType = widgetdata.Level1SchedulerType,
                    L1SchedulerEmailIDs = widgetdata.L1SchedulerEmailIDs,
                    DashbaordQueryL1 = widgetdata.DashbaordQueryL1,
                    Level2ConnectionString = widgetdata.Level2ConnectionString,
                    Level2SchedulerType = widgetdata.Level2SchedulerType,
                    L2SchedulerEmailIDs = widgetdata.L2SchedulerEmailIDs,
                    DashbaordQueryL2 = widgetdata.DashbaordQueryL2,
                    Level3ConnectionString = widgetdata.Level3ConnectionString,
                    Level3SchedulerType = widgetdata.Level3SchedulerType,
                    L3SchedulerEmailIDs = widgetdata.L3SchedulerEmailIDs,
                    DashbaordQueryL3 = widgetdata.DashbaordQueryL3,
                    Level4ConnectionString = widgetdata.Level4ConnectionString,
                    Level4SchedulerType = widgetdata.Level4SchedulerType,
                    L4SchedulerEmailIDs = widgetdata.L4SchedulerEmailIDs,
                    DashbaordQueryL4 = widgetdata.DashbaordQueryL4,
                    WidgetSendEmail = widgetdata.WidgetSendEmail,
                    CreatedBy = httpContext.ContextUserId,
                    CreatedOn = DateTime.Now
                });
                return GetResponse(httpContext, HttpStatusCode.OK, "Available");
            }
            catch (Exception ex)
            {
                return GetResponse(httpContext, HttpStatusCode.BadRequest, ex.Message.ToString());
            }
        }

        internal async static Task<ResponseBase> updateWidget(TeamHttpContext httpContext, DashboardWidget widgetdata)
        {
            try
            {
                List<UserDashboard> objUserDashboard = new List<UserDashboard>();
                using (TeamDbContext dbContext = new TeamDbContext())
                {
                    if (dbContext.UserDashboards.AsNoTracking().FirstOrDefault(e => e.Id == widgetdata.WidgetId) != null)
                    {
                        objUserDashboard = dbContext.UserDashboards.Where(x => x.Id == widgetdata.WidgetId && x.Deleted == false).ToList();
                    }
                }

                if (objUserDashboard.Count != 0)
                {
                    UpdateUserDashboard(new UserDashboard
                    {
                        Id = widgetdata.WidgetId,
                        DashboardWidgetName = widgetdata.DashboardWidgetName,
                        DashboardChartType = widgetdata.DashboardChartType,
                        DashboardUserPermission = widgetdata.DashboardUserPermission,
                        DashboardEmailFormat = widgetdata.DashboardEmailFormat,
                        WidgetConnectionString = widgetdata.WidgetConnectionString,
                        WidgetSchedulerType = widgetdata.WidgetSchedulerType,
                        WidgetSchedulerEmailIDs = widgetdata.WidgetSchedulerEmailIDs,
                        WidgetQuery = widgetdata.WidgetQuery,
                        Level1ConnectionString = widgetdata.Level1ConnectionString,
                        Level1SchedulerType = widgetdata.Level1SchedulerType,
                        L1SchedulerEmailIDs = widgetdata.L1SchedulerEmailIDs,
                        DashbaordQueryL1 = widgetdata.DashbaordQueryL1,
                        Level2ConnectionString = widgetdata.Level2ConnectionString,
                        Level2SchedulerType = widgetdata.Level2SchedulerType,
                        L2SchedulerEmailIDs = widgetdata.L2SchedulerEmailIDs,
                        DashbaordQueryL2 = widgetdata.DashbaordQueryL2,
                        Level3ConnectionString = widgetdata.Level3ConnectionString,
                        Level3SchedulerType = widgetdata.Level3SchedulerType,
                        L3SchedulerEmailIDs = widgetdata.L3SchedulerEmailIDs,
                        DashbaordQueryL3 = widgetdata.DashbaordQueryL3,
                        Level4ConnectionString = widgetdata.Level4ConnectionString,
                        Level4SchedulerType = widgetdata.Level4SchedulerType,
                        L4SchedulerEmailIDs = widgetdata.L4SchedulerEmailIDs,
                        DashbaordQueryL4 = widgetdata.DashbaordQueryL4,
                        WidgetSendEmail = widgetdata.WidgetSendEmail,
                        CreatedBy = objUserDashboard[0].CreatedBy,
                        CreatedOn = objUserDashboard[0].CreatedOn,
                        ModifiedBy = httpContext.ContextUserId,
                        ModifiedOn = DateTime.Now
                    });
                    return GetResponse(httpContext, HttpStatusCode.OK, "Available");
                }
                return null;
            }
            catch (Exception ex)
            {
                return GetResponse(httpContext, HttpStatusCode.BadRequest, ex.Message.ToString());
            }
        }

        internal static async Task<List<OnScreenClick>> GetGridDataAsync(TeamHttpContext httpContext, OnScreenClick details)
        {
            //string connString = Utility.GetConnectionString("PgAdmin4ConnectionString");
            string connString = string.Empty;
            string userid = httpContext.ContextUserId;
            //string userid = "Admin";
            List<OnScreenClick> returnValue = new List<OnScreenClick>();
            PostgresService postgresService = new PostgresService();
            List<UserDashboard> objUserDashboard = new List<UserDashboard>();
            string querylevel = string.Empty;
            List<string[]> ResultSet = new List<string[]>();
            MatchCollection allMatchResults = null;
            Regex regex = new Regex(@"@\w*@");

            using (TeamDbContext dbContext = new TeamDbContext())
            {
                //objUserDashboard = dbContext.UserDashboards.Where(x => x.DashboardUserId == userid && x.Deleted == false && x.Id == details.ClickedWidgetId).ToList(); // For specific user
                objUserDashboard = dbContext.UserDashboards.Where(x => x.Deleted == false && x.Id == details.ClickedWidgetId).ToList(); // for all users
            }

            if (objUserDashboard.Count != 0)
            {
                foreach (var item in objUserDashboard)
                {
                    switch (details.ClickLevel)
                    {
                        case "L1":
                            connString = Utility.GetConnectionString(item.Level1ConnectionString);
                            querylevel = item.DashbaordQueryL1;
                            allMatchResults = regex.Matches(querylevel);
                            if (allMatchResults.Count > 0)
                                querylevel = querylevel.Replace(allMatchResults[0].Value, details.ClickedOnValue);
                            break;

                        case "L2":
                            connString = Utility.GetConnectionString(item.Level2ConnectionString);
                            querylevel = item.DashbaordQueryL2;
                            break;

                        case "L3":
                            connString = Utility.GetConnectionString(item.Level3ConnectionString);
                            querylevel = item.DashbaordQueryL3;
                            break;

                        case "L4":
                            connString = Utility.GetConnectionString(item.Level4ConnectionString);
                            querylevel = item.DashbaordQueryL4;
                            break;
                    }

                    if (details.ClickLevel != "L1")
                    {
                        var dict = details.GridInput.ToDictionary(m => m.Name, m => m.Value);
                        allMatchResults = regex.Matches(querylevel);
                        if (allMatchResults.Count > 0)
                        {
                            foreach (var match in allMatchResults)
                            {
                                dict.TryGetValue(((System.Text.RegularExpressions.Capture) match).Value.Substring(1, ((System.Text.RegularExpressions.Capture) match).Value.Length - 2), out string result);
                                querylevel = querylevel.Replace(match.ToString(), result);
                            }
                        }
                    }

                    NpgsqlDataReader dr = postgresService.ExecuteSqlReturnReader(connString, querylevel);
                    DataTable dtSchema = dr.GetSchemaTable();
                    DataTable dt = new DataTable();
                    List<DataColumn> listCols = new List<DataColumn>();

                    if (dtSchema != null)
                    {
                        foreach (DataRow drow in dtSchema.Rows)
                        {
                            string columnName = Convert.ToString(drow["ColumnName"]);
                            DataColumn column = new DataColumn(columnName, (Type) (drow["DataType"]));
                            column.Unique = (bool) drow["IsUnique"];
                            column.AutoIncrement = (bool) drow["IsAutoIncrement"];
                            listCols.Add(column);
                            dt.Columns.Add(column);
                        }
                    }

                    // Read rows from DataReader and populate the DataTable
                    while (dr.Read())
                    {
                        DataRow dataRow = dt.NewRow();
                        for (int i = 0; i < listCols.Count; i++)
                        {
                            dataRow[((DataColumn) listCols[i])] = dr[i];
                        }
                        dt.Rows.Add(dataRow);
                    }
                    string[] columnNames = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();

                    ResultSet = dt.Select().Select(drr => drr.ItemArray.Select(x => x.ToString()).ToArray()).ToList();

                    OnScreenClick newItem = new OnScreenClick
                    {
                        ClickLevel = details.ClickLevel,
                        ClickedWidgetId = details.ClickedWidgetId,
                        ClickedOnValue = details.ClickedOnValue,
                        GridColumns = columnNames,
                        GridData = ResultSet,
                        GridInput = null
                    };
                    returnValue.Add(newItem);
                }
            }
            return returnValue;
        }
    }
}
