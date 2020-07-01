using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ASNRTech.CoreService.Core;
using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.Data;
using ASNRTech.CoreService.Enums;
using ASNRTech.CoreService.Services;
using ASNRTech.CoreService.Utilities;

namespace ASNRTech.CoreService.Security {
    public class AuthService : BaseService {

        public static async Task<ResponseBase<LoginResponseModel>> LoginAsync(TeamHttpContext teamHttpContext, LoginModel loginModel) {
            if (teamHttpContext == null) {
                throw new ArgumentNullException(nameof(teamHttpContext));
            }

            if (loginModel == null) {
                throw new ArgumentNullException(nameof(loginModel));
            }

            LoginResponseModel loginResponse = new LoginResponseModel {
                Status = LoginStatus.InValid
            };

            User user = new User();

            user = await ValidatePassword(teamHttpContext, loginModel).ConfigureAwait(false);

            if (user != null) {
                using (TeamDbContext dbContext = new TeamDbContext()) {
                    if (user.UserId != null) {
                        teamHttpContext.SetValue(Constants.CONTEXT_USER, user.UserId);
                        loginResponse.Status = LoginStatus.LoggedIn;
                    }
                    else {
                        loginResponse.Status = LoginStatus.NoAccess;
                    }

                    if (loginResponse.Status == LoginStatus.LoggedIn) {
                        user.LastLoggedIn = DateTime.Now;
                        await dbContext.SaveChangesAsync().ConfigureAwait(false);
                    }
                }
            }

            if (loginResponse.Status == LoginStatus.LoggedIn) {
                string sessionId = Guid.NewGuid().ToString();
                await CreateSessionAsync(user.UserId, sessionId).ConfigureAwait(false);
                await CacheService.StoreSessionAsync(teamHttpContext, sessionId).ConfigureAwait(false);
                await CacheService.StoreUserAsync(teamHttpContext, user).ConfigureAwait(false);

                int validity = Utility.IsProduction ? 1 : 7;
                long validUntil = DateTime.UtcNow.AddDays(validity).GetUnixTimeStamp();
                loginResponse.Token = Jwt.CreateToken(user, sessionId, validUntil);
                loginResponse.Email = user.Email;
                loginResponse.UserId = user.UserId;
                loginResponse.Type = user.UserType;
                loginResponse.SessionId = sessionId;
                loginResponse.ExpiryTimestamp = validUntil;

                return GetTypedResponse<LoginResponseModel>(teamHttpContext, loginResponse);
            }
            else {
                return GetUnauthorizedResponse<LoginResponseModel>(teamHttpContext, loginResponse);
            }
        }

        private async static Task<User> ValidatePassword(TeamHttpContext teamHttpContext, LoginModel loginModel) {
            loginModel.UserId = loginModel.UserId.ToUpper(CultureInfo.InvariantCulture);

            using (TeamDbContext dbContext = new TeamDbContext()) {
                User user = dbContext.Users.FirstOrDefault(e => e.UserId == loginModel.UserId && !e.Deleted);

                if (user?.Status == UserStatus.Active) {
                    string encPassword = Utility.GetMd5Hash(loginModel.Password);
                    if (user.Password == encPassword) {
                        return user;
                    }
                }
                return null;
            }
        }

        public static async Task<ResponseBase> LogoutAsync(TeamHttpContext teamHttpContext) {
            if (teamHttpContext == null) {
                throw new ArgumentNullException(nameof(teamHttpContext));
            }

            string userId = teamHttpContext.CurrentUser.UserId.ToUpper(CultureInfo.InvariantCulture);

            using (TeamDbContext dbContext = new TeamDbContext()) {
                List<UserSession> activeSessions = dbContext.UserSessions.Where(e => e.UserId == userId && !e.Deleted && e.Active).ToList();
                foreach (UserSession item in activeSessions) {
                    item.Active = false;
                    await CacheService.RemoveSessionAsync(teamHttpContext, item.SessionId).ConfigureAwait(false);
                }
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            await CacheService.RemoveUserAsync(teamHttpContext, teamHttpContext.CurrentUser).ConfigureAwait(false);

            return GetResponse(teamHttpContext);
        }

        private static async Task CreateSessionAsync(string userId, string sessionId) {
            using (TeamDbContext dbContext = new TeamDbContext()) {
                dbContext.UserSessions.Add(new UserSession {
                    UserId = userId,
                    SessionId = sessionId,
                    LastApiCall = DateTime.Now
                });
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
