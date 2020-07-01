using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace ASNRTech.CoreService.Security {
  public static class SslValidator {

    public static void OverrideValidation() {
      ServicePointManager.ServerCertificateValidationCallback = OnValidateCertificate;
      ServicePointManager.Expect100Continue = false;
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
    }

    private static bool OnValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
      // if no certificate errors, allow
      if (sslPolicyErrors == SslPolicyErrors.None) {
        return true;
      }
      else if (certificate.Subject.Contains("ASNRTech")) {
        return true;
      }
      else {
        // other wise fail
        return false;
      }
    }
  }
}
