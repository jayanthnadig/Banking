using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ASNRTech.CoreService.Core;
using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.Enums;
using System.Collections.Generic;

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
        [Route("v1/auth/logout")]
        [TeamAuthorize(AccessType.Admin | AccessType.Client, false)]
        public async Task<ResponseBase> Logout()
        {
            return await AuthService.LogoutAsync(new TeamHttpContext(HttpContext)).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("v1/auth/viewallusers/{userId}")]
        //[TeamAuthorize(AccessType.Admin | AccessType.Client, false)]
        //[TeamAuthorize(AccessType.Client, false)]
        public async Task<ResponseBase<List<AddEditNewUser>>> viewallusers()
        {
            return await AuthService.ViewAllUser(new TeamHttpContext(HttpContext)).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("v1/auth/userconfig/{userId}")]
        //[TeamAuthorize(AccessType.Admin | AccessType.Client, false)]
        //[TeamAuthorize(AccessType.Client, false)]
        public async Task<ResponseBase<List<AddEditNewUser>>> addeditnewuser([FromBody]List<AddEditNewUser> adduser)
        {
            return await AuthService.AddEditNewUser(new TeamHttpContext(HttpContext), adduser).ConfigureAwait(false);
        }
    }
}
