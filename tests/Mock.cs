using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using TeamLease.CssService.Client;
using TeamLease.CssService.Core;
using TeamLease.CssService.Core.Models;
using TeamLease.CssService.Data;
using TeamLease.CssService.Enums;
using TeamLease.CssService.Security;
using TeamLease.CssService.Utilities;

namespace TeamLease.CssService.Tests {
  public static class MockData {
    internal static DateTime dateTime { get; set; }
    internal static int year { get; set; }
    internal static int month { get; set; }
    internal static DateRange dateRange { get; set; }
    internal static DataRequest dataRequest { get; set; }
    internal static HttpContext httpContext { get; set; }
    internal static TeamHttpContext teamHttpContext { get; set; }
    internal static LoginModel loginModel { get; set; }
    internal static string userId { get; set; }
    internal static TeamDbContext dbContext { get; set; }
    internal static User user { get; set; }
    internal static int NId { get; set; }
    internal static string Exporttype { get; set; }
    internal static string EmpId { get; set; }
    internal static string exportPWD { get; set; }
    internal static string clientId { get; set; }

    static MockData() {
      clientId = "05HMB";
      httpContext = new DefaultHttpContext();
      dateRange = DateUtility.GetMonthDateRange(DateTime.Today.Year, DateTime.Today.Month);
      httpContext.Items.Add(Constants.CONTEXT_USER, new User() { UserId = "anchor" });
      httpContext.Items.Add(Constants.CONTEXT_CLIENT_ID, clientId);
      dateTime = DateTime.Now;
      year = 2019;
      month = 2;
      loginModel = new LoginModel() { UserId = "anchor", Password = "testing" };
      dataRequest = new DataRequest(year, 1, 0, 5);
      userId = "anchor";
      dbContext = new TeamDbContext();
      user = new User() { Status = UserStatus.Active, Clients = new List<ClientDto>() };
      NId = 1;
      EmpId = "2C47D89302";
      exportPWD = "welcomecss";
      teamHttpContext = new TeamHttpContext(httpContext);
    }
  }
}
