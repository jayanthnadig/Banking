using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
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
using TeamLease.CssService.Invoices;
using TeamLease.CssService.PaymentDetails;
using TeamLease.CssService.Utilities;

namespace TeamLease.CssService.Tests {
  [TestClass]
  public class Tests {

    [TestInitialize]
    public void Init() {
      Program.BuildConfiguration();
    }

    /* banking test cases */

    [Xunit.Fact]
    public async Task AllBankingWidgetMethod() {
      Program.BuildConfiguration();
      var data = await BankingService.GetBankingDetailsByTypeAsync(new TeamHttpContext(MockData.httpContext), 0, 5, BankingMode.All).ConfigureAwait(false);
      string result = data.Code == HttpStatusCode.OK ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task ChequeBankingWidgetMethod() {
      Program.BuildConfiguration();
      var data = await BankingService.GetBankingDetailsByTypeAsync(new TeamHttpContext(MockData.httpContext), 0, 5, BankingMode.Cheque).ConfigureAwait(false);
      string result = data.Code == HttpStatusCode.OK ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task NeftBankingWidgetMethod() {
      Program.BuildConfiguration();
      var data = await BankingService.GetBankingDetailsByTypeAsync(new TeamHttpContext(MockData.httpContext), 0, 5, Enums.BankingMode.Neft).ConfigureAwait(false);
      string result = data.Code == HttpStatusCode.OK ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task NoBankBankingWidgetMethod() {
      Program.BuildConfiguration();
      var data = await BankingService.GetBankingDetailsByTypeAsync(new TeamHttpContext(MockData.httpContext), 0, 5, Enums.BankingMode.NoDetails).ConfigureAwait(false);

      string result = data.Code == HttpStatusCode.OK ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task AllExportBankingDetailsByTypeMethod() {
      Program.BuildConfiguration();
      var data = await BankingService.ExportBankingDetailsByTypeAsync(new TeamHttpContext(MockData.httpContext), MockData.year, MockData.month, Enums.BankingMode.All, MockData.Exporttype, MockData.exportPWD).ConfigureAwait(false);
      string result = data.FileStream.Length > 0 ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task ChequeExportBankingDetailsByTypeMethod() {
      Program.BuildConfiguration();
      var data = await BankingService.ExportBankingDetailsByTypeAsync(new TeamHttpContext(MockData.httpContext), MockData.year, MockData.month, Enums.BankingMode.Cheque, MockData.Exporttype, MockData.exportPWD).ConfigureAwait(false);
      string result = data.FileStream.Length > 0 ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task NeftExportBankingDetailsByTypeMethod() {
      Program.BuildConfiguration();
      var data = await BankingService.ExportBankingDetailsByTypeAsync(new TeamHttpContext(MockData.httpContext), MockData.year, MockData.month, Enums.BankingMode.Neft, MockData.Exporttype, MockData.exportPWD).ConfigureAwait(false);
      string result = data.FileStream.Length > 0 ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task NoDetailsExportBankingDetailsByTypeMethod() {
      Program.BuildConfiguration();
      var data = await BankingService.ExportBankingDetailsByTypeAsync(new TeamHttpContext(MockData.httpContext), MockData.year, MockData.month, Enums.BankingMode.NoDetails, MockData.Exporttype, MockData.exportPWD).ConfigureAwait(false);
      string result = data.FileStream.Length > 0 ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    /* AssociateService test cases*/

    [Xunit.Fact]
    public async Task ActiveAssociateMethod() {
      Program.BuildConfiguration();
      var data = await AssociateService.GetActiveAsync(new TeamHttpContext(MockData.httpContext), MockData.year, MockData.month, 0, 5).ConfigureAwait(false);
      string result = data.Code == HttpStatusCode.OK ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task AllAssociateMethod() {
      Program.BuildConfiguration();
      var data = await AssociateService.GetAllAsync(new TeamHttpContext(MockData.httpContext), MockData.year, MockData.month, 0, 5).ConfigureAwait(false);
      string result = data.Code == HttpStatusCode.OK ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task ContractExpiryAsyncMethod() {
      Program.BuildConfiguration();
      var data = await AssociateService.GetContractExpiryAsync(new TeamHttpContext(MockData.httpContext), MockData.year, MockData.month, 0, 5).ConfigureAwait(false);
      string result = data.Code == HttpStatusCode.OK ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task NewJoineeAsyncMethod() {
      Program.BuildConfiguration();
      var data = await AssociateService.GetNewJoineeAsync(new TeamHttpContext(MockData.httpContext), MockData.year, MockData.month, 0, 5).ConfigureAwait(false);
      string result = data.Code == HttpStatusCode.OK ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task ResigneeAsyncMethod() {
      Program.BuildConfiguration();
      var data = await AssociateService.GetResigneeAsync(new TeamHttpContext(MockData.httpContext), MockData.year, MockData.month, 0, 5).ConfigureAwait(false);
      string result = data.Code == HttpStatusCode.OK ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task ActiveAssociateReportsMethod() {
      Program.BuildConfiguration();
      var data = await AssociateService.ExportActiveAssociatesAsync(new TeamHttpContext(MockData.httpContext), MockData.year, MockData.month, MockData.Exporttype, MockData.exportPWD).ConfigureAwait(false);
      string result = data.FileStream.Length > 0 ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task AllAssociateReportsMethod() {
      Program.BuildConfiguration();
      var data = await AssociateService.ExportAllAssociatesAsync(new TeamHttpContext(MockData.httpContext), MockData.year, MockData.month, MockData.Exporttype, MockData.exportPWD).ConfigureAwait(false);
      string result = data.FileStream.Length > 0 ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task NewjoineeAssociateReportsMethod() {
      Program.BuildConfiguration();
      var data = await AssociateService.ExportNewjoineeAssociatesAsync(new TeamHttpContext(MockData.httpContext), MockData.year, MockData.month, MockData.Exporttype, MockData.exportPWD).ConfigureAwait(false);
      string result = data.FileStream.Length > 0 ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task ResigneeAssociateReportsMethod() {
      Program.BuildConfiguration();
      var data = await AssociateService.ExportResigneeAssociatesAsync(new TeamHttpContext(MockData.httpContext), MockData.year, MockData.month, "csv", MockData.exportPWD).ConfigureAwait(false);
      string result = data.FileStream.Length > 0 ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    /* payment details */

    [Xunit.Fact]
    public async Task GetPaymentDetailsMethod() {
      Program.BuildConfiguration();
      var data = await PaymentDetailsService.GetPaymentDetailsAsync(new TeamHttpContext(MockData.httpContext), MockData.year, MockData.month, 0, 5).ConfigureAwait(false);
      string result = data.Code == HttpStatusCode.OK ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task ExportPaymentDetailsMethod() {
      Program.BuildConfiguration();
      var data = await PaymentDetailsService.ExportPaymentDetailsAsync(new TeamHttpContext(MockData.httpContext), MockData.year, MockData.month, "csv", MockData.exportPWD).ConfigureAwait(false);
      string result = data.FileStream.Length > 0 ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task GetInvoicePaymentDetailsMethod() {
      Program.BuildConfiguration();
      var data = await InvoiceService.GetAsync(new TeamHttpContext(MockData.httpContext), MockData.year).ConfigureAwait(false);
      string result = data.Code == HttpStatusCode.OK ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task GetBankNamesMethod() {
      Program.BuildConfiguration();
      var data = await UtilityService.GetBankNamesAsync(new TeamHttpContext(MockData.httpContext)).ConfigureAwait(false);
      string result = data.Code == HttpStatusCode.OK ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task GetPayModesMethod() {
      Program.BuildConfiguration();
      var data = await UtilityService.GetPayModesAsync(new TeamHttpContext(MockData.httpContext)).ConfigureAwait(false);
      string result = data.Code == HttpStatusCode.OK ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    /*invoice test cases*/

    [Xunit.Fact]
    public async Task GetinvoiceDetails() {
      Program.BuildConfiguration();
      var data = await InvoiceService.GetAsync(new TeamHttpContext(MockData.httpContext), MockData.year).ConfigureAwait(false);
      string result = data.Code == HttpStatusCode.OK ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    /*associate profile*/

    [Xunit.Fact]
    public async Task GetDocuments() {
      Program.BuildConfiguration();
      var data = await AssociateProfileService.GetDocumentsAsync(new TeamHttpContext(MockData.httpContext), MockData.EmpId).ConfigureAwait(false);
      string result = data.Code == HttpStatusCode.OK ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task GetProfileDetailsAsync() {
      Program.BuildConfiguration();
      var data = await AssociateProfileService.GetDetailsAsync(new TeamHttpContext(MockData.httpContext), MockData.EmpId).ConfigureAwait(false);
      string result = data.Code == HttpStatusCode.OK ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    /* dash board test cases*/

    [Xunit.Fact]
    public async Task DashboardBankingWidgetMethod() {
      Program.BuildConfiguration();
      var data = await DashboardService.GetBankingWidgetAsync(new TeamHttpContext(MockData.httpContext)).ConfigureAwait(false);
      string result = data.Code == HttpStatusCode.OK ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task ContractExpiryDateMethod() {
      Program.BuildConfiguration();
      var data = await DashboardService.GetContractExpiryDateAsync(new TeamHttpContext(MockData.httpContext)).ConfigureAwait(false);
      string result = data.Code == HttpStatusCode.OK ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task GetAssociateWidgetAsyncMethod() {
      Program.BuildConfiguration();
      var data = await DashboardService.GetAssociateWidgetAsync(new TeamHttpContext(MockData.httpContext), MockData.year, MockData.month).ConfigureAwait(false);
      string result = data.Code == HttpStatusCode.OK ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task PayoutWidgetMethod() {
      Program.BuildConfiguration();
      var data = await DashboardService.GetPayoutWidgetAsync(new TeamHttpContext(MockData.httpContext)).ConfigureAwait(false);
      string result = data.RowCount >= 0 ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    [Xunit.Fact]
    public async Task PayrollCalendarWidgetMethod() {
      Program.BuildConfiguration();
      var data = await DashboardService.GetPayrollCalendarWidgetAsync(new TeamHttpContext(MockData.httpContext), MockData.year, MockData.month).ConfigureAwait(false);
      string result = data.Code == HttpStatusCode.OK ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    /* client service*/

    [Xunit.Fact]
    public async Task GetClientsProfileMethod() {
      Program.BuildConfiguration();
      var data = ClientService.GetClientProfile(new TeamHttpContext(MockData.httpContext));
      string result = data.Code == HttpStatusCode.OK ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    //[Xunit.Fact]
    //public async Task GetAnchorsMethod()
    //{
    //    var data = AlcsSyncService.GetAnchors(MockData.dateTime);
    //    string result = data.Count >= 0 ? "OK" : "ERROR";
    //    Xunit.Assert.Equal("OK", result);
    //}

    /* Advertisements */

    [Xunit.Fact]
    public void AdvertisementsMethod() {
      Program.BuildConfiguration();
      var data = AdvertisementService.Get(new TeamHttpContext(MockData.httpContext));
      string result = data.Code == HttpStatusCode.OK ? "OK" : "ERROR";
      Xunit.Assert.Equal("OK", result);
    }

    internal static void ValidateResponse(ResponseBase response) {
      if (response is ErrorResponse errorResponse) {
        Assert.Fail(errorResponse.Error);
      }
      if (response.Code != HttpStatusCode.OK) {
        Assert.Fail(response.Error);
      }
    }

    internal static void ValidateResponse<T>(ResponseBase<T> response) {
      if (response.Code != HttpStatusCode.OK) {
        Assert.Fail(response.Error);
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
