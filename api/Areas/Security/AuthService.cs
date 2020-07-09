using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ASNRTech.CoreService.Core;
using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.Data;
using ASNRTech.CoreService.Enums;
using ASNRTech.CoreService.Services;
using ASNRTech.CoreService.Utilities;
using Microsoft.EntityFrameworkCore;

namespace ASNRTech.CoreService.Security
{
    public class AuthService : BaseService
    {
        public static async Task<ResponseBase<LoginResponseModel>> LoginAsync(TeamHttpContext teamHttpContext, LoginModel loginModel)
        {
            if (teamHttpContext == null)
            {
                throw new ArgumentNullException(nameof(teamHttpContext));
            }

            if (loginModel == null)
            {
                throw new ArgumentNullException(nameof(loginModel));
            }

            LoginResponseModel loginResponse = new LoginResponseModel
            {
                Status = LoginStatus.InValid
            };

            User user = new User();

            user = await ValidatePassword(teamHttpContext, loginModel).ConfigureAwait(false);

            if (user != null)
            {
                using (TeamDbContext dbContext = new TeamDbContext())
                {
                    if (user.UserId != null)
                    {
                        teamHttpContext.SetValue(Constants.CONTEXT_USER, user.UserId);
                        loginResponse.Status = LoginStatus.LoggedIn;
                    }
                    else
                    {
                        loginResponse.Status = LoginStatus.NoAccess;
                    }

                    if (loginResponse.Status == LoginStatus.LoggedIn)
                    {
                        user.LastLoggedIn = DateTime.Now;
                        await dbContext.SaveChangesAsync().ConfigureAwait(false);
                    }
                }
            }

            if (loginResponse.Status == LoginStatus.LoggedIn)
            {
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
            else
            {
                return GetUnauthorizedResponse<LoginResponseModel>(teamHttpContext, loginResponse);
            }
        }

        private async static Task<User> ValidatePassword(TeamHttpContext teamHttpContext, LoginModel loginModel)
        {
            loginModel.UserId = loginModel.UserId.ToUpper(CultureInfo.InvariantCulture);

            using (TeamDbContext dbContext = new TeamDbContext())
            {
                User user = dbContext.Users.FirstOrDefault(e => e.UserId == loginModel.UserId && !e.Deleted);

                if (user?.Status == UserStatus.Active)
                {
                    string encPassword = Utility.GetMd5Hash(loginModel.Password);
                    if (user.Password == encPassword)
                    {
                        return user;
                    }
                }
                return null;
            }
        }

        public static async Task<ResponseBase> LogoutAsync(TeamHttpContext teamHttpContext)
        {
            if (teamHttpContext == null)
            {
                throw new ArgumentNullException(nameof(teamHttpContext));
            }

            string userId = teamHttpContext.CurrentUser.UserId.ToUpper(CultureInfo.InvariantCulture);

            using (TeamDbContext dbContext = new TeamDbContext())
            {
                List<UserSession> activeSessions = dbContext.UserSessions.Where(e => e.UserId == userId && !e.Deleted && e.Active).ToList();
                foreach (UserSession item in activeSessions)
                {
                    item.Active = false;
                    await CacheService.RemoveSessionAsync(teamHttpContext, item.SessionId).ConfigureAwait(false);
                }
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            await CacheService.RemoveUserAsync(teamHttpContext, teamHttpContext.CurrentUser).ConfigureAwait(false);

            return GetResponse(teamHttpContext);
        }

        private static async Task CreateSessionAsync(string userId, string sessionId)
        {
            using (TeamDbContext dbContext = new TeamDbContext())
            {
                dbContext.UserSessions.Add(new UserSession
                {
                    UserId = userId,
                    SessionId = sessionId,
                    LastApiCall = DateTime.Now
                });
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public static async Task<ResponseBase<List<AddEditNewUser>>> ViewAllUser(TeamHttpContext teamHttpContext)
        {
            if (teamHttpContext == null)
            {
                throw new ArgumentNullException(nameof(teamHttpContext));
            }
            List<AddEditNewUser> AllUsers = await GetUserAsync(teamHttpContext, -1).ConfigureAwait(false);
            return GetTypedResponse(teamHttpContext, AllUsers);
        }

        public static async Task<ResponseBase<List<AddEditNewUser>>> AddEditNewUser(TeamHttpContext teamHttpContext, List<AddEditNewUser> details)
        {
            if (teamHttpContext == null)
            {
                throw new ArgumentNullException(nameof(teamHttpContext));
            }

            List<AddEditNewUser> AllEditedUser = new List<AddEditNewUser>();

            for (int i = 0; i <= details.Count - 1; i++)
            {
                if (details[i].TableId == -1)
                {
                    ResponseBase user = await AddUser(teamHttpContext, details[i]).ConfigureAwait(false);
                    if (user.Code == HttpStatusCode.OK)
                    {
                        var USERID = -1;
                        using (TeamDbContext dbContext = new TeamDbContext())
                        {
                            USERID = dbContext.Users.OrderByDescending(x => x.Id).First().Id;
                        }
                        List<AddEditNewUser> NewUser = await GetUserAsync(teamHttpContext, USERID).ConfigureAwait(false);
                        return GetTypedResponse(teamHttpContext, NewUser);
                    }
                }
                else
                {
                    ResponseBase user = await EditUser(teamHttpContext, details[i]).ConfigureAwait(false);
                    if (user.Code == HttpStatusCode.OK)
                    {
                        int.TryParse(details[i].TableId.ToString(), out int userid);
                        List<AddEditNewUser> NewUser = await GetUserAsync(teamHttpContext, userid).ConfigureAwait(false);
                        AllEditedUser.Add(NewUser[0]);
                    }
                }
            }
            return GetTypedResponse(teamHttpContext, AllEditedUser);
        }

        internal static async Task<ResponseBase> AddUser(TeamHttpContext teamHttpContext, AddEditNewUser details)
        {
            try
            {
                AddUserDatabase(new User
                {
                    UserId = details.UserId.ToUpper(CultureInfo.InvariantCulture),
                    Password = Utility.GetMd5Hash(details.Password),
                    UserType = UserType.Client,
                    Email = details.UserEmail,
                    //CreatedBy = teamHttpContext.CurrentUser.UserId,
                    CreatedBy = "Admin",
                    CreatedOn = DateTime.Now,
                    Status = UserStatus.Active,
                });
                return GetResponse(teamHttpContext, HttpStatusCode.OK, "Available");
            }
            catch (Exception ex)
            {
                return GetResponse(teamHttpContext, HttpStatusCode.BadRequest, ex.Message.ToString());
            }
        }

        private static void AddUserDatabase(User newuser)
        {
            using (TeamDbContext dbContext = new TeamDbContext())
            {
                if (dbContext.Users.FirstOrDefault(e => e.Id == newuser.Id) == null)
                {
                    dbContext.Users.Add(newuser);
                    dbContext.SaveChanges();
                }
            }
        }

        private static void EditUserDatabase(User existinguser)
        {
            using (TeamDbContext dbContext = new TeamDbContext())
            {
                if (dbContext.Users.AsNoTracking().FirstOrDefault(e => e.Id == existinguser.Id && e.Deleted == false) != null)
                {
                    dbContext.Users.UpdateRange(existinguser);
                    dbContext.SaveChanges();
                }
            }
        }

        internal static async Task<List<AddEditNewUser>> GetUserAsync(TeamHttpContext httpContext, int userid)
        {
            string connString = Utility.GetConnectionString("DefaultConnection");
            List<AddEditNewUser> returnValue = new List<AddEditNewUser>();
            PostgresService postgresService = new PostgresService();
            List<User> objUser = new List<User>();

            using (TeamDbContext dbContext = new TeamDbContext())
            {
                if (userid != -1)
                    objUser = dbContext.Users.Where(x => x.Deleted == false && x.Id == userid).ToList();
                else
                    objUser = dbContext.Users.Where(x => x.Deleted == false && x.UserType != UserType.Admin).ToList();
            }

            if (objUser.Count != 0)
            {
                foreach (var item in objUser)
                {
                    var userstatus = (item.Status == UserStatus.Active) ? true : false;

                    AddEditNewUser newItem = new AddEditNewUser
                    {
                        UserId = item.UserId,
                        Password = "*******",
                        UserEmail = item.Email,
                        TableId = item.Id,
                        IsActive = userstatus
                    };
                    returnValue.Add(newItem);
                }

                return returnValue;
            }
            else
                return null;
        }

        internal async static Task<ResponseBase> EditUser(TeamHttpContext httpContext, AddEditNewUser details)
        {
            try
            {
                List<User> objUser = new List<User>();
                using (TeamDbContext dbContext = new TeamDbContext())
                {
                    if (dbContext.Users.AsNoTracking().FirstOrDefault(e => e.Id == details.TableId) != null)
                    {
                        objUser = dbContext.Users.Where(x => x.Id == details.TableId).ToList();
                    }
                }

                var userstatus = (details.IsActive) ? 1 : 2;

                if (objUser.Count != 0)
                {
                    EditUserDatabase(new User
                    {
                        Id = details.TableId,
                        UserId = details.UserId.ToUpper(CultureInfo.InvariantCulture),
                        Password = (details.Password.Contains("*******")) ? objUser[0].Password : Utility.GetMd5Hash(details.Password),
                        Email = details.UserEmail,
                        //ModifiedBy = teamHttpContext.CurrentUser.UserId,
                        ModifiedBy = "Admin",
                        ModifiedOn = DateTime.Now,
                        UserType = UserType.Client,
                        CreatedBy = objUser[0].CreatedBy,
                        CreatedOn = objUser[0].CreatedOn,
                        Status = (UserStatus) userstatus,
                    });
                    return GetResponse(httpContext, HttpStatusCode.OK, "Available");
                }
                return null;
            }
            catch (Exception ex)
            {
                return GetResponse(httpContext, HttpStatusCode.BadRequest, ex.Message.ToString());
            }
        }
    }
}
