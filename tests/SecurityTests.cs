using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamLease.CssService.Config;
using TeamLease.CssService.Core;
using TeamLease.CssService.Core.Models;
using TeamLease.CssService.Enums;
using TeamLease.CssService.Security;
using TeamLease.CssService.Utilities;

namespace TeamLease.CssService.Tests {
  [TestClass]
  public class SecurityTests {
    private TeamHttpContext httpContext;
    private const string USER_ID = "anchor";

    [TestInitialize]
    public async Task Init() {
      Program.BuildConfiguration();
      httpContext = new TeamHttpContext(MockData.httpContext);
    }

    [TestMethod]
    public async Task ChangePasswordAsync() {
      const string password1 = "testing_1";
      const string password2 = "testing_2";

      ChangePasswordModel changePassword = new ChangePasswordModel {
        UserId = USER_ID,
        OldPassword = password1,
        NewPassword = password2
      };

      ResponseBase response = await PasswordService.ChangePasswordAsync(httpContext, changePassword).ConfigureAwait(false);
      Tests.ValidateResponse(response);

      changePassword.OldPassword = password2;
      changePassword.NewPassword = password1;
      response = await PasswordService.ChangePasswordAsync(httpContext, changePassword).ConfigureAwait(false);
      Tests.ValidateResponse(response);
    }

    [TestMethod]
    public void GetResetOptions() {
      ResponseBase<List<KeyValuePair<PasswordResetType, string>>> response = PasswordService.GetResetPasswordOptions(httpContext, USER_ID);
      Tests.ValidateResponse(response);

      Assert.AreNotEqual(0, response.Data.Count);
    }

    [TestMethod]
    public async Task SendResetEmailAsync() {
      ResetKeyRequest resetKey = new ResetKeyRequest {
        UserId = USER_ID,
        ResetType = PasswordResetType.Email
      };
      ResponseBase response = await PasswordService.SendResetKeyAsync(httpContext, resetKey).ConfigureAwait(false);
      Tests.ValidateResponse(response);
    }

    [TestMethod]
    public async Task SendResetOtpAsync() {
      ResetKeyRequest resetKey = new ResetKeyRequest {
        UserId = USER_ID,
        ResetType = PasswordResetType.Phone
      };
      ResponseBase response = await PasswordService.SendResetKeyAsync(httpContext, resetKey).ConfigureAwait(false);
      Tests.ValidateResponse(response);
    }

    [TestMethod]
    public void ClearExpiredRecords() {
      PasswordService.ClearExpiredRecords();
    }

    //[TestMethod]
    //public void ExpiredJwt() {
    //  IActionResult result = Jwt(HttpUtility.CreateExpiredToken());

    //  Assert.AreEqual((new UnauthorizedResult()).GetType(), result.GetType());
    //}

    //[TestMethod]
    //public void InValidClientKey() {
    //  IActionResult result = Key(AuthorizationMethod.ClientPreSharedKey, "abcd");

    //  Assert.AreEqual((new UnauthorizedResult()).GetType(), result.GetType());
    //}

    //[TestMethod]
    //public void InValidJwt() {
    //  IActionResult result = Jwt("abcd");

    //  Assert.AreEqual((new UnauthorizedResult()).GetType(), result.GetType());
    //}

    //[TestMethod]
    //public void InValidServerKey() {
    //  IActionResult result = Key(AuthorizationMethod.ServerPreSharedKey, "abcd");

    //  Assert.AreEqual((new UnauthorizedResult()).GetType(), result.GetType());
    //}

    //[TestMethod]
    //public void NoHeadersClientKey() {
    //  NoHeaders(AuthorizationMethod.ClientPreSharedKey);
    //}

    //[TestMethod]
    //public void NoHeadersDynamicKey() {
    //  NoHeaders(AuthorizationMethod.DynamicKey);
    //}

    //[TestMethod]
    //public void NoHeadersJwt() {
    //  NoHeaders(AuthorizationMethod.Jwt);
    //}

    //[TestMethod]
    //public void NoHeadersServerKey() {
    //  NoHeaders(AuthorizationMethod.ServerPreSharedKey);
    //}

    //[TestMethod]
    //public void ValidClientKey() {
    //  IActionResult result = Key(AuthorizationMethod.ClientPreSharedKey, DiscountingTests.partnerConfig.ApiKeyForClientCalls);

    //  Assert.IsNull(result);
    //}

    //[TestMethod]
    //public void ValidJwt() {
    //  IActionResult result = Jwt(HttpUtility.CreateJwtToken());

    //  Assert.IsNull(result);
    //}

    //[TestMethod]
    //public void ValidServerKey() {
    //  IActionResult result = Key(AuthorizationMethod.ServerPreSharedKey, DiscountingTests.partnerConfig.ApiKeyForServerCalls);

    //  Assert.IsNull(result);
    //}

    //[TestMethod]
    //public async Task ValidDynamicKeyAsync() {
    //  await DynamicKeyAsync("d-130", 0).ConfigureAwait(false);
    //}

    //[TestMethod]
    //public async Task ExpiredDynamicKeyAsync() {
    //  await DynamicKeyAsync("d-131", 1).ConfigureAwait(false);
    //}

    //[TestMethod]
    //public async Task InvalidDynamicKeyAsync() {
    //  await DynamicKeyAsync("d-129", 2).ConfigureAwait(false);
    //}

    //// testType: 0: Normal, 1: Expired, 2: Invalid
    //private async Task DynamicKeyAsync(string uid, int testType) {
    //  DriverVendorBase driverVendorBase = DriverService.Get(arthContext, uid) ?? (DriverVendorBase) VendorService.Get(arthContext, uid);

    //  if (driverVendorBase == null) {
    //    Assert.Fail($"Entity now found with {uid}");
    //  }

    //  string phoneNumber = driverVendorBase.Mobile;

    //  string otp = string.Concat(phoneNumber.ToCharArray().Reverse()).Substring(0, 6);
    //  const string entityType = Constants.ENTITY_TYPE_DRIVER;

    //  await TokenService.SendOtpAsync(arthContext, entityType, uid).ConfigureAwait(false);
    //  ResponseBase response = await TokenService.ValidateOtpAndGenerateTokenAsync(arthContext, entityType, uid, otp).ConfigureAwait(false);

    //  if (response.Data == null) {
    //    ErrorResponse errorResponse = (ErrorResponse) response;
    //    Assert.Fail("Failed. {0}:{1}", errorResponse.Code, errorResponse.Error);
    //    return;
    //  }

    //  Token token = (Token) response.Data;

    //  if (testType == 1) {
    //    // expired test. delete tokens
    //    await TokenService.DeleteAsync(arthContext, entityType, uid).ConfigureAwait(false);
    //  }

    //  string accessToken = token.AccessToken;
    //  if (testType == 2) {
    //    // invalid. test with a random key
    //    accessToken = "asdfasd";
    //  }

    //  List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>> {
    //    new KeyValuePair<string, string>(Constants.HEADER_UID, uid),
    //    new KeyValuePair<string, string>(Constants.HEADER_ENTITY_TYPE, entityType)
    //  };

    //  IActionResult result = Key(AuthorizationMethod.DynamicKey, accessToken, headers);

    //  if (testType == 0) {
    //    Assert.IsNull(result);
    //  }
    //  else {
    //    Assert.AreEqual((new UnauthorizedResult()).GetType(), result.GetType());
    //  }
    //}

    //private AuthorizationFilterContext GetFilterContext() {
    //  ActionContext actionContext = new ActionContext {
    //    HttpContext = new DefaultHttpContext(),
    //    ActionDescriptor = new ActionDescriptor(),
    //    RouteData = new RouteData()
    //  };

    //  RouteValueDictionary keyValuePairs = new RouteValueDictionary {
    //    { Constants.ROUTE_PARAM_PARTNER_ID, "suntelematics" }
    //  };
    //  actionContext.RouteData.PushState(null, keyValuePairs, null);
    //  actionContext.HttpContext.Request.Protocol = "http";
    //  actionContext.HttpContext.Request.QueryString = new QueryString(SENT_AT);

    //  List<IFilterMetadata> filters = new List<IFilterMetadata>();

    //  return new AuthorizationFilterContext(actionContext, filters);
    //}

    //private IActionResult Jwt(string token) {
    //  ArthAuthorizeFilter filter = new ArthAuthorizeFilter(AuthorizationMethod.Jwt, true);

    //  AuthorizationFilterContext filterContext = GetFilterContext();
    //  filterContext.HttpContext.Request.Headers.Add(Constants.HEADER_JWT_CLIENT_TOKEN, token);
    //  filter.OnAuthorization(filterContext);

    //  return filterContext.Result;
    //}

    //private void NoHeaders(AuthorizationMethod authorizationMethod) {
    //  ArthAuthorizeFilter filter = new ArthAuthorizeFilter(authorizationMethod, true);

    //  AuthorizationFilterContext filterContext = GetFilterContext();
    //  filter.OnAuthorization(filterContext);

    //  Assert.AreEqual((new UnauthorizedResult()).GetType(), filterContext.Result.GetType());
    //}

    //private IActionResult Key(AuthorizationMethod authorizationMethod, string key, List<KeyValuePair<string, string>> headers = null) {
    //  ArthAuthorizeFilter filter = new ArthAuthorizeFilter(authorizationMethod, true);

    //  AuthorizationFilterContext filterContext = GetFilterContext();
    //  filterContext.HttpContext.Request.Headers.Add(Constants.HEADER_API_KEY, key);

    //  if (headers != null) {
    //    foreach (KeyValuePair<string, string> item in headers) {
    //      filterContext.HttpContext.Request.Headers.Add(item.Key, item.Value);
    //    }
    //  }

    //  string stringToComputeMac = $"staging|{key}|https://{SENT_AT}";
    //  string computedSignature = Utility.GenerateMacSha256(key, stringToComputeMac);

    //  filterContext.HttpContext.Request.Headers.Add(Constants.HEADER_SIGNATURE, computedSignature);
    //  filter.OnAuthorization(filterContext);

    //  return filterContext.Result;
    //}

    [TestMethod]
    public void GetEncPassword() {
      string encPassword = Utility.GetMd5Hash("testing_1");
    }
  }
}
