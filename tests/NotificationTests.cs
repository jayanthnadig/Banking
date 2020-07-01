using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TeamLease.CssService.Config;
using TeamLease.CssService.Core;
using TeamLease.CssService.Core.Models;
using TeamLease.CssService.Enums;
using TeamLease.CssService.Inputs;
using TeamLease.CssService.Notification;

namespace TeamLease.CssService.Tests {
  [TestClass]
  public class NotificationTests {

    [TestInitialize]
    public async Task Init() {
      Program.BuildConfiguration();
    }

    [TestMethod]
    public void GetNotificationsMethod() {
      var data = NotificationService.Get(MockData.teamHttpContext);
      Tests.ValidateResponse(data);
    }

    [TestMethod]
    public void NotificationsCountMethod() {
      ResponseBase<int> data = NotificationService.GetCount(MockData.teamHttpContext);
      Tests.ValidateResponse(data);
    }

    [TestMethod]
    public void MarkAsReadMethod() {
      ResponseBase data = NotificationService.Read(MockData.teamHttpContext, MockData.NId);
      Tests.ValidateResponse(data);
    }

    [TestMethod]
    public void MarkAllAsReadMethod() {
      ResponseBase data = NotificationService.ReadAll(MockData.teamHttpContext);
      Tests.ValidateResponse(data);
    }

    [TestMethod]
    public async Task SendToAllAnchorsAsync() {
      ResponseBase data = await NotificationService.SendMessageAsync(MockData.teamHttpContext, "Test Subject", "Test Body", NotificationTarget.Anchors, NotificationType.Both).ConfigureAwait(false);
      Tests.ValidateResponse(data);
    }

    [TestMethod]
    public async Task SendToUserAsync() {
      ResponseBase data = await NotificationService.SendMessageAsync(MockData.teamHttpContext, "ANCHOR", "Test Subject", "Test Body").ConfigureAwait(false);
      Tests.ValidateResponse(data);
    }
  }
}
