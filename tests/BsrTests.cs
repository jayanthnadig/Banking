using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TeamLease.CssService.Config;
using TeamLease.CssService.Core;
using TeamLease.CssService.Core.Models;
using TeamLease.CssService.Data;
using TeamLease.CssService.Enums;
using TeamLease.CssService.Inputs;
using TeamLease.CssService.Security;
using TeamLease.CssService.Utilities;

namespace TeamLease.CssService.Tests {
  [TestClass]
  public class BsrTests {

    [TestInitialize]
    public async Task Init() {
      Program.BuildConfiguration();
    }

    [TestMethod]
    public void Bsr() {
      BsrWorker(true);
      BsrWorker(false);
    }

    private void BsrWorker(bool approve) {
      const string clientId = "TEST";

      HttpContext httpContext = new DefaultHttpContext();
      httpContext.Items.Add(Constants.CONTEXT_USER, new User() { UserId = "anchor" });
      httpContext.Items.Add(Constants.CONTEXT_CLIENT_ID, clientId);
      TeamHttpContext teamHttpContext = new TeamHttpContext(httpContext);
    }

    [TestMethod]
    public async Task GetAsync() {
      const string clientId = "05HMB";
      const int year = 2019;

      HttpContext httpContext = new DefaultHttpContext();
      httpContext.Items.Add(Constants.CONTEXT_USER, new User() { UserId = "anchor" });
      httpContext.Items.Add(Constants.CONTEXT_CLIENT_ID, clientId);
      TeamHttpContext teamHttpContext = new TeamHttpContext(httpContext);

      PagedResponse<BsrStatusResponse> bsrList = await BsrService.Get(teamHttpContext, year).ConfigureAwait(false);
      //ValidateBsr(teamHttpContext, year, month, -2); // invalid alcs-state
    }

    private static void ChangeBsrStatus(TeamHttpContext teamHttpContext, int year, int month, bool approve, HttpStatusCode expectedChangeResult, int expectedStatusResult) {
      ResponseBase response = BsrService.ChangeBsrStatusAsync(teamHttpContext, year, month, approve).Result;
      Assert.AreEqual(expectedChangeResult, response.Code);

      ValidateBsr(teamHttpContext, year, month, expectedStatusResult);
    }

    private static void ValidateBsr(TeamHttpContext teamHttpContext, int year, int month, int expectedStatus) {
      // check status is blank. (no-bsr)
      ResponseBase<BsrStatusResponse> bsrStatus = BsrService.GetLatestBsrStatus(teamHttpContext, year, month);

      Tests.ValidateResponse(bsrStatus);

      Assert.AreEqual(bsrStatus.Data.Status, expectedStatus);
    }
  }
}
