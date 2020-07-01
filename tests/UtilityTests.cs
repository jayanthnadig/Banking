using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;
using TeamLease.CssService.AmazonUtils;
using TeamLease.CssService.Config;

namespace TeamLease.CssService.Tests {
  [TestClass]
  public class UtilityTests {
    private HttpContext httpContext = new DefaultHttpContext();

    [TestInitialize]
    public async Task Init() {
      Program.BuildConfiguration();
    }

    [TestMethod]
    public async Task DownloadFileFromS3() {
      const string downloadUrl = "s3://css-invoice-payment-advices/e3a7a11a-e23b-477a-a058-f83ad9b827b7";
      byte[] data = await S3.DownloadFileAsync(downloadUrl).ConfigureAwait(false);

      Assert.AreNotEqual(0, data.Length, "Downloaded file is blank");
    }

    [TestMethod]
    public async Task SignedUrl() {
      string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"data\EvilBender.jpg");

      byte[] bytes = File.ReadAllBytes(filePath);
      string file = Convert.ToBase64String(bytes);

      string fileUrl = await S3.UploadFileAsync(file, "css-client-logo-uat", string.Empty, false).ConfigureAwait(false);

      string signedUrl = S3.GetSignedUrl(fileUrl);

      Assert.AreNotEqual(0, signedUrl.Length, "Signed URL is blank");
    }

    [TestMethod]
    public async Task SignedUrlWithBytes() {
      string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"data\evilbender.jpg");

      byte[] bytes = File.ReadAllBytes(filePath);

      string fileUrl = await S3.UploadFileAsync(bytes, "css-profile-image-test", string.Empty, false).ConfigureAwait(false);

      string signedUrl = S3.GetSignedUrl(fileUrl);

      Assert.AreNotEqual(0, signedUrl.Length, "Signed URL is blank");
    }
  }
}
