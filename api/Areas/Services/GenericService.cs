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
        internal static async Task<List<LoadWidgets>> GetAllWidgetAsync(TeamHttpContext httpContext)
        {
            string connString = Utility.GetConnectionString("DefaultConnection");
            //string userid = httpContext.CurrentUser.UserId;
            string userid = "Admin";
            List<LoadWidgets> returnValue = new List<LoadWidgets>();
            PostgresService postgresService = new PostgresService();
            List<UserDashboard> objUserDashboard = new List<UserDashboard>();
            using (TeamDbContext dbContext = new TeamDbContext())
            {
                objUserDashboard = dbContext.UserDashboards.Where(x => x.DashboardUserId == userid && x.Deleted == false).ToList();
            }

            if (objUserDashboard.Count != 0)
            {
                foreach (var item in objUserDashboard)
                {
                    List<WidgetRead> objWidgetRead = new List<WidgetRead>();
                    using (NpgsqlDataReader sqlData = postgresService.ExecuteSqlReturnReader(connString, item.DashbaordQuery))
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
                        WidgetName = item.DashboardWidgetName,
                        WidgetId = item.Id,
                        WidgetType = item.DashboardChartType,
                        WidgetData = objWidgetRead.ToArray(),
                        WidgetQuery = item.DashbaordQuery,
                        WidgetQueryLevel1 = item.DashbaordQueryL1,
                        WidgetQueryLevel2 = item.DashbaordQueryL2,
                        WidgetQueryLevel3 = item.DashbaordQueryL3
                    };
                    returnValue.Add(newItem);
                }
            }

            return returnValue;
        }

        internal static async Task<List<LoadWidgets>> GetWidgetAsync(TeamHttpContext httpContext, string widgettype, string widgetname, string widgetquery, string widgetquerylevel1, string widgetquerylevel2, string widgetquerylevel3)
        {
            string connString = Utility.GetConnectionString("DefaultConnection");
            //string userid = httpContext.CurrentUser.UserId;
            string userid = "Admin";
            List<LoadWidgets> returnValue = new List<LoadWidgets>();
            PostgresService postgresService = new PostgresService();
            List<UserDashboard> objUserDashboard = new List<UserDashboard>();
            using (TeamDbContext dbContext = new TeamDbContext())
            {
                objUserDashboard = dbContext.UserDashboards.Where(x => x.DashboardUserId == userid && x.DashboardChartType == widgettype && x.DashboardWidgetName == widgetname && x.Deleted == false).ToList();
            }

            if (objUserDashboard.Count != 0)
            {
                foreach (var item in objUserDashboard)
                {
                    List<WidgetRead> objWidgetRead = new List<WidgetRead>();
                    using (NpgsqlDataReader sqlData = postgresService.ExecuteSqlReturnReader(connString, item.DashbaordQuery))
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
                        WidgetName = item.DashboardWidgetName,
                        WidgetId = item.Id,
                        WidgetType = item.DashboardChartType,
                        WidgetData = objWidgetRead.ToArray(),
                        WidgetQuery = item.DashbaordQuery,
                        WidgetQueryLevel1 = item.DashbaordQueryL1,
                        WidgetQueryLevel2 = item.DashbaordQueryL2,
                        WidgetQueryLevel3 = item.DashbaordQueryL3
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

        private static void EditUserDashboard(UserDashboard userdashboard)
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

        internal async static Task<ResponseBase> createWidget(TeamHttpContext httpContext, DashboardWidget widgetdata)
        {
            try
            {
                AddUserDashboard(new UserDashboard
                {
                    //DashboardUserId = httpContext.CurrentUser.UserId,
                    DashboardUserId = "Admin",
                    DashboardChartType = widgetdata.WidgetType,
                    DashboardWidgetName = widgetdata.WidgetName,
                    DashbaordQuery = widgetdata.WidgetQuery,
                    //CreatedBy = httpContext.CurrentUser.UserId,
                    CreatedBy = "Admin",
                    DashbaordQueryL1 = widgetdata.WidgetQueryLevel1,
                    DashbaordQueryL2 = widgetdata.WidgetQueryLevel2,
                    DashbaordQueryL3 = widgetdata.WidgetQueryLevel3,
                    CreatedOn = DateTime.Now,
                    DashbaordModifiedOn = null,
                });
                return GetResponse(httpContext, HttpStatusCode.OK, "Available");
            }
            catch (Exception ex)
            {
                return GetResponse(httpContext, HttpStatusCode.BadRequest, ex.Message.ToString());
            }
        }

        internal async static Task<ResponseBase> editWidget(TeamHttpContext httpContext, DashboardWidget widgetdata)
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
                    EditUserDashboard(new UserDashboard
                    {
                        Id = widgetdata.WidgetId,
                        //DashboardUserId = httpContext.CurrentUser.UserId,
                        DashboardUserId = "Admin",
                        DashboardChartType = widgetdata.WidgetType,
                        DashboardWidgetName = widgetdata.WidgetName,
                        DashbaordQuery = widgetdata.WidgetQuery,
                        DashbaordQueryL1 = widgetdata.WidgetQueryLevel1,
                        DashbaordQueryL2 = widgetdata.WidgetQueryLevel2,
                        DashbaordQueryL3 = widgetdata.WidgetQueryLevel3,
                        CreatedBy = objUserDashboard[0].CreatedBy,
                        CreatedOn = objUserDashboard[0].CreatedOn,
                        //ModifiedBy = httpContext.CurrentUser.UserId,
                        ModifiedBy = "Admin",
                        ModifiedOn = DateTime.Now,
                        DashbaordModifiedOn = DateTime.Now,
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
            string connString = Utility.GetConnectionString("DefaultConnection");
            //string userid = httpContext.CurrentUser.UserId;
            string userid = "Admin";
            List<OnScreenClick> returnValue = new List<OnScreenClick>();
            PostgresService postgresService = new PostgresService();
            List<UserDashboard> objUserDashboard = new List<UserDashboard>();
            string querylevel = string.Empty;
            List<string[]> ResultSet = new List<string[]>();
            MatchCollection allMatchResults = null;
            Regex regex = new Regex(@"@\w*@");

            using (TeamDbContext dbContext = new TeamDbContext())
            {
                objUserDashboard = dbContext.UserDashboards.Where(x => x.DashboardUserId == userid && x.Deleted == false && x.Id == details.ClickedWidgetId).ToList();
            }

            if (objUserDashboard.Count != 0)
            {
                foreach (var item in objUserDashboard)
                {
                    switch (details.ClickLevel)
                    {
                        case "L1":
                            querylevel = item.DashbaordQueryL1;
                            allMatchResults = regex.Matches(querylevel);
                            if (allMatchResults.Count > 0)
                                querylevel = querylevel.Replace(allMatchResults[0].Value, details.ClickedOnValue);
                            break;

                        case "L2":
                            querylevel = item.DashbaordQueryL2;
                            break;

                        case "L3":
                            querylevel = item.DashbaordQueryL3;
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
