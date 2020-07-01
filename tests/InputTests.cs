using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using TeamLease.CssService.Config;
using TeamLease.CssService.Core;
using TeamLease.CssService.Core.Models;
using TeamLease.CssService.CsvUtilities;
using TeamLease.CssService.Enums;
using TeamLease.CssService.Inputs;
using TeamLease.CssService.Utilities;

namespace TeamLease.CssService.Tests {
  [TestClass]
  public class InputTests {
    private TeamHttpContext httpContext;
    private int month = 11;
    private int year = 2019;

    [TestMethod]
    public async Task Get() {
      await _Get().ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Import() {
      string remarks = Guid.NewGuid().ToString();
      ResponseBase<InputsPageData> data = await _Get().ConfigureAwait(false);
      string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"data\input_upload.csv");
      byte[] bytes = File.ReadAllBytes(filePath);

      InputsSaveRequest inputsSaveRequest = new InputsSaveRequest {
        SaveType = InputSaveType.Upload,
        Config = data.Data.Config,
        ImportData = Convert.ToBase64String(bytes),
        Year = year,
        Month = month
      };

      // set remarks
      inputsSaveRequest.Config.Remarks = remarks;

      string importData = Utility.CleanUploadData(inputsSaveRequest.ImportData);

      List<Input> inputs = HelperMethods.ConvertCsvToObjectList<InputsCsvMap, Input>(importData);

      data = await InputService.SaveAsync(httpContext, inputsSaveRequest).ConfigureAwait(false);

      Assert.AreEqual(HttpStatusCode.OK, data.Code);
      Assert.AreEqual(remarks, data.Data.Config.Remarks, "Remarks not saved");
      Assert.AreEqual(5, data.Data.Inputs[0].LopDays, "LOP not saved");
    }

    [TestInitialize]
    public async Task Init() {
      Program.BuildConfiguration();
      httpContext = new TeamHttpContext(MockData.httpContext);
    }

    [TestMethod]
    public async Task Save() {
      string remarks = Guid.NewGuid().ToString();
      int lopDays = new Random().Next(28);
      ResponseBase<InputsPageData> data = await _Get().ConfigureAwait(false);

      // set remarks
      data.Data.Config.Remarks = remarks;

      // set lop
      data.Data.Inputs[0].LopDays = lopDays;

      // save
      InputsSaveRequest inputsSaveRequest = new InputsSaveRequest {
        SaveType = InputSaveType.Save,
        Year = year,
        Month = month,
        Config = data.Data.Config,
        Inputs = data.Data.Inputs
      };
      data = await InputService.SaveAsync(httpContext, inputsSaveRequest).ConfigureAwait(false);
      Assert.AreEqual(HttpStatusCode.OK, data.Code);

      // check
      Assert.AreEqual(data.Data.Config.Remarks, remarks, "Remarks not saved");
      Assert.AreEqual(data.Data.Inputs[0].LopDays, lopDays, "LOP not saved");
    }

    [TestMethod]
    public async Task Submit() {
      ResponseBase<InputsPageData> data = await _Get().ConfigureAwait(false);

      InputsSaveRequest inputsSaveRequest = new InputsSaveRequest {
        SaveType = InputSaveType.Submit,
        Config = data.Data.Config,
        Inputs = data.Data.Inputs,
        Year = year,
        Month = month
      };

      data = await InputService.SaveAsync(httpContext, inputsSaveRequest).ConfigureAwait(false);

      Assert.AreEqual(HttpStatusCode.OK, data.Code);
    }

    private async Task<ResponseBase<InputsPageData>> _Get() {
      ResponseBase<InputsPageData> data = await InputService.GetAsync(httpContext, year, month, 0, 20, string.Empty).ConfigureAwait(false);

      Assert.AreEqual(HttpStatusCode.OK, data.Code);

      return data;
    }
  }
}
