using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeamLease.CssService.Core;
using TeamLease.CssService.Enums;
using TeamLease.CssService.Jobs;

namespace TeamLease.CssService.Security {
    [ApiController]
    public class SecurityTestController : TeamControllerBase {

        [HttpGet]
        [TeamAuthorize(AccessType.Open, SecurityCheckType.None)]
        [Route("v1/security-test/open")]
        public static void Open() {
        }

        [HttpGet]
        [TeamAuthorize(AccessType.Anchor, true)]
        [Route("v1/security-test/{clientId}/anchor")]
        public static void Anchor() {
        }

        [HttpGet]
        [TeamAuthorize(AccessType.Client, true)]
        [Route("v1/security-test/{clientId}/client")]
        public static void Client() {
        }

        [HttpGet]
        [TeamAuthorize(AccessType.Anchor | AccessType.Client, true)]
        [Route("v1/security-test/{clientId}/both")]
        public static void Both() {
        }
    }
}
