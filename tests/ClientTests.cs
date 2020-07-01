using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using TeamLease.CssService.Advertisement;
using TeamLease.CssService.Associate;
using TeamLease.CssService.AssociateProfile;
using TeamLease.CssService.Banking;
using TeamLease.CssService.Client;
using TeamLease.CssService.Config;
using TeamLease.CssService.Core;
using TeamLease.CssService.Core.Models;
using TeamLease.CssService.Dashboard;
using TeamLease.CssService.Enums;
using TeamLease.CssService.Inputs;
using TeamLease.CssService.Invoices;
using TeamLease.CssService.Notification;
using TeamLease.CssService.PaymentDetails;
using TeamLease.CssService.Security;
using TeamLease.CssService.Utilities;

namespace TeamLease.CssService.Tests {
  [TestClass]
  public class ClientTests {

    [TestInitialize]
    public void Init() {
      Program.BuildConfiguration();
    }

    [TestMethod]
    public void GetClients() {
      var data = ClientService.GetClient((string) MockData.httpContext.GetValue(Constants.CONTEXT_CLIENT_ID));
      string result = !string.IsNullOrEmpty(data.ClientName) ? "OK" : "ERROR";
      Assert.AreEqual("OK", result);
    }

    [TestMethod]
    public void GetClientUsers() {
      List<User> data = ClientService.GetUsers(MockData.teamHttpContext);

      Assert.AreNotEqual<int>(0, data.Count, "No Users found");
    }

    internal static void ValidateResponse(ResponseBase response) {
      if (response is ErrorResponse errorResponse) {
        Assert.Fail(errorResponse.Error);
      }
    }

    internal static void ValidateResponse(Task<ResponseBase> response) {
      if (response.Result is ErrorResponse errorResponse) {
        Assert.Fail(errorResponse.Error);
      }
    }

    internal static dynamic LoadJson<T>(string fileName) {
      string filePath = Path.Combine(Directory.GetCurrentDirectory(), "data", fileName);
      using (StreamReader r = new StreamReader(filePath)) {
        string json = r.ReadToEnd();
        return JsonConvert.DeserializeObject<T>(json);
      }
    }
  }
}
