using ASNRTech.CoreService.Core;
using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASNRTech.CoreService.Security
{
    [ApiController]
    public class AuthController : TeamControllerBase
    {
        [HttpPost]
        [Route("v1/auth/login")]
        public async Task<ResponseBase<LoginResponseModel>> Login([FromBody] LoginModel loginModel)
        {
            return await AuthService.LoginAsync(new TeamHttpContext(HttpContext), loginModel).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("v1/auth/logout/userId/{userId}")]
        public async Task<ResponseBase> Logout(string userId)
        {
            return await AuthService.LogoutAsync(new TeamHttpContext(HttpContext), userId).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("v1/auth/viewallusers/{userId}")]
        [TeamAuthorize(AccessType.Client, true)]
        public async Task<ResponseBase<List<AddEditNewUser>>> viewallusers()
        {
            return await AuthService.ViewAllUser(new TeamHttpContext(HttpContext)).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("v1/auth/userconfig/{userId}")]
        [TeamAuthorize(AccessType.Client, true)]
        public async Task<ResponseBase<List<AddEditNewUser>>> addeditnewuser([FromBody]List<AddEditNewUser> user)
        {
            return await AuthService.AddEditNewUser(new TeamHttpContext(HttpContext), user).ConfigureAwait(false);
        }
    }
}
