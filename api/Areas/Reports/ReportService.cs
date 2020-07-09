using ASNRTech.CoreService.Core;
using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.CsvUtilities;
using ASNRTech.CoreService.Data;
using ASNRTech.CoreService.Services;
using ASNRTech.CoreService.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ASNRTech.CoreService.Reports
{
    public class ReportService : BaseService
    {
        public static async Task<ResponseBase<List<ReportList>>> ReportNames(TeamHttpContext teamHttpContext)
        {
            List<ReportList> objReportList = new List<ReportList>();
            using (TeamDbContext dbContext = new TeamDbContext())
            {
                var reportnames = from rpt in dbContext.ReportConfigs
                                  select new
                                  {
                                      Id = rpt.Id,
                                      ReportName = rpt.ReportName,
                                      Deleted = rpt.Deleted
                                  };
                foreach (var item in reportnames)
                {
                    ReportList reportList = new ReportList
                    {
                        ReportId = item.Id,
                        ReportName = item.ReportName,
                        IsActive = !(item.Deleted)
                    };

                    objReportList.Add(reportList);
                }
            }
            return GetTypedResponse(teamHttpContext, objReportList);
        }

        public static async Task<ResponseBase<List<ReportGrid>>> ViewReport(TeamHttpContext teamHttpContext, int reportId)
        {
            try
            {
                string connString = Utility.GetConnectionString("DefaultConnection");
                //string userid = httpContext.CurrentUser.UserId;
                string userid = "Admin";
                List<ReportGrid> returnValue = new List<ReportGrid>();
                PostgresService postgresService = new PostgresService();
                List<ReportConfig> objReportConfig = new List<ReportConfig>();
                List<string[]> ResultSet = new List<string[]>();

                using (TeamDbContext dbContext = new TeamDbContext())
                {
                    objReportConfig = dbContext.ReportConfigs.Where(x => x.Id == reportId && x.Deleted == false && x.ReportReqUserId == userid).ToList();
                }

                if (objReportConfig.Count != 0)
                {
                    foreach (var item in objReportConfig)
                    {
                        NpgsqlDataReader dr = postgresService.ExecuteSqlReturnReader(connString, item.ReportQuery);
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

                        ReportGrid newItem = new ReportGrid
                        {
                            ReportId = reportId,
                            ReportName = item.ReportName,
                            GridColumns = columnNames,
                            GridData = ResultSet,
                            IsActive = !(item.Deleted)
                        };
                        returnValue.Add(newItem);
                    }
                }
                return GetTypedResponse(teamHttpContext, returnValue);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<ResponseBase<ReportConfigAddEdit>> EditReport(TeamHttpContext teamHttpContext, int reportId)
        {
            List<ReportConfig> objReportConfig = new List<ReportConfig>();
            using (TeamDbContext dbContext = new TeamDbContext())
            {
                objReportConfig = dbContext.ReportConfigs.Where(x => x.Id == reportId).ToList();
            }

            if (objReportConfig.Count != 0)
            {
                foreach (var item in objReportConfig)
                {
                    ReportConfigAddEdit objReportConfigAddEdit = new ReportConfigAddEdit
                    {
                        ReportId = item.Id,
                        ReportName = item.ReportName,
                        ReportQuery = item.ReportQuery,
                        ReportEmail = item.ReportEmail,
                        ReportFormat = item.ReportFileFormat,
                        ReportInterval = item.ReportSchedule,
                        IsActive = !(item.Deleted)
                    };

                    return GetTypedResponse(teamHttpContext, objReportConfigAddEdit);
                }
            }

            return null;
        }

        public static async Task<ResponseBase> AddEditReport(TeamHttpContext teamHttpContext, ReportConfigAddEdit report)
        {
            if (teamHttpContext == null)
            {
                throw new ArgumentNullException(nameof(teamHttpContext));
            }
            if (report.ReportId == -1)
            {
                ResponseBase response = await addReport(teamHttpContext, report).ConfigureAwait(false);
                if (response.Code == HttpStatusCode.OK)
                    return GetResponse(teamHttpContext);
            }
            else
            {
                ResponseBase response = await editReport(teamHttpContext, report).ConfigureAwait(false);
                if (response.Code == HttpStatusCode.OK)
                    return GetResponse(teamHttpContext);
            }
            return null;
        }

        internal static async Task<ResponseBase> addReport(TeamHttpContext httpContext, ReportConfigAddEdit report)
        {
            try
            {
                AddReport(new ReportConfig
                {
                    //ReportReqUserId = httpContext.CurrentUser.UserId,
                    ReportReqUserId = "Admin",
                    ReportName = report.ReportName,
                    ReportQuery = report.ReportQuery,
                    ReportEmail = report.ReportEmail,
                    ReportFileFormat = report.ReportFormat,
                    ReportSchedule = report.ReportInterval,
                    //CreatedBy = httpContext.CurrentUser.UserId,
                    CreatedBy = "Admin",
                    CreatedOn = DateTime.Now,
                    Deleted = !(report.IsActive)
                });
                return GetResponse(httpContext, HttpStatusCode.OK, "Available");
            }
            catch (Exception ex)
            {
                return GetResponse(httpContext, HttpStatusCode.BadRequest, ex.Message.ToString());
            }
        }

        private static void AddReport(ReportConfig addreport)
        {
            using (TeamDbContext dbContext = new TeamDbContext())
            {
                if (dbContext.ReportConfigs.FirstOrDefault(e => e.Id == addreport.Id) == null)
                {
                    dbContext.ReportConfigs.Add(addreport);
                    dbContext.SaveChanges();
                }
            }
        }

        internal static async Task<ResponseBase> editReport(TeamHttpContext httpContext, ReportConfigAddEdit report)
        {
            try
            {
                List<ReportConfig> objReportConfig = new List<ReportConfig>();
                using (TeamDbContext dbContext = new TeamDbContext())
                {
                    if (dbContext.ReportConfigs.AsNoTracking().FirstOrDefault(e => e.Id == report.ReportId) != null)
                    {
                        objReportConfig = dbContext.ReportConfigs.Where(x => x.Id == report.ReportId && x.Deleted == false).ToList();
                    }
                }

                if (objReportConfig.Count != 0)
                {
                    EditReport(new ReportConfig
                    {
                        Id = report.ReportId,
                        //ReportReqUserId = httpContext.CurrentUser.UserId,
                        ReportReqUserId = "Admin",
                        ReportName = report.ReportName,
                        ReportQuery = report.ReportQuery,
                        ReportEmail = report.ReportEmail,
                        ReportFileFormat = report.ReportFormat,
                        ReportSchedule = report.ReportInterval,
                        CreatedBy = objReportConfig[0].CreatedBy,
                        CreatedOn = objReportConfig[0].CreatedOn,
                        //ModifiedBy = httpContext.CurrentUser.UserId,
                        ModifiedBy = "Admin",
                        ModifiedOn = DateTime.Now,
                        ReportModifiedOn = DateTime.Now,
                        Deleted = !(report.IsActive)
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

        private static void EditReport(ReportConfig editreport)
        {
            using (TeamDbContext dbContext = new TeamDbContext())
            {
                if (dbContext.ReportConfigs.AsNoTracking().FirstOrDefault(e => e.Id == editreport.Id) != null)
                {
                    dbContext.ReportConfigs.UpdateRange(editreport);
                    dbContext.SaveChanges();
                }
            }
        }

        internal static async Task<FileStreamResult> DownloadAsync(TeamHttpContext teamHttpContext, int reportId)
        {
            var objReportGrid = ViewReport(teamHttpContext, reportId);
            if (objReportGrid.Result.Code == HttpStatusCode.OK)
            {
                foreach (var item in objReportGrid.Result.Data)
                {
                    byte[] csvByteData = HelperMethods.ConvertObjectListToCsv(item.GridData, string.Join(",", item.GridColumns));
                    MemoryStream memoryStream = new MemoryStream(csvByteData);
                    return new FileStreamResult(memoryStream, "text/csv") { FileDownloadName = item.ReportName + ".csv" };
                }
            }

            return null;
        }
    }
}
