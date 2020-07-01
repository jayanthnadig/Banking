using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamLease.CssService.Enums;
using TeamLease.CssService.Security;
using TeamLease.CssService.Utilities;

namespace TeamLease.CssService.Alcs {
  [ApiController]
  [TeamAuthorize(AccessType.Anchor, true)]
  public class TestAlcsController : ControllerBase {

    [HttpGet]
    [Route("v1/test-sp/{clientId}")]
    public async Task<List<SpData>> GetSpResultChecksAsync() {
      if (!Utility.IsProduction) {
        TestSPDataService testSp = new TestSPDataService();
        return await testSp.Test(HttpContext).ConfigureAwait(false);
      }
      else {
        return null;
      }
    }
  }
}
