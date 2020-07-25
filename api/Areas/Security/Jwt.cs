using ASNRTech.CoreService.Enums;
using ASNRTech.CoreService.Logging;
using ASNRTech.CoreService.Utilities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ASNRTech.CoreService.Security
{
    public static class Jwt
    {
        internal static KeyValuePair<JwtTokenValidationStatus, LoginResponseModel> Validate(string token)
        {
            if (!token.HasValue())
            {
                return new KeyValuePair<JwtTokenValidationStatus, LoginResponseModel>(JwtTokenValidationStatus.NoToken, null);
            }

            try
            {
                string preSharedKey = Utilities.Utility.GetConfigValue("jwtTokenKey");
                using (HMACSHA256 hmac = new HMACSHA256(Encoding.ASCII.GetBytes(preSharedKey)))
                {
                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    SecurityToken validatedToken = new JwtSecurityToken();
                    JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

                    TokenValidationParameters validationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(hmac.Key),
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };

                    ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                    if (jwtToken.ValidFrom < DateTime.UtcNow && jwtToken.ValidTo > DateTime.UtcNow)
                    {
                        string userId = GetClaim(claimsPrincipal, Constants.CLAIM_USER_ID);
                        string sessionId = GetClaim(claimsPrincipal, Constants.CLAIM_SESSION_ID);

                        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(sessionId))
                        {
                            return new KeyValuePair<JwtTokenValidationStatus, LoginResponseModel>(JwtTokenValidationStatus.Invalid, null);
                        }
                        LoginResponseModel loginModel = new LoginResponseModel
                        {
                            UserId = userId,
                            SessionId = sessionId
                        };

                        return new KeyValuePair<JwtTokenValidationStatus, LoginResponseModel>(JwtTokenValidationStatus.Valid, loginModel);
                    }
                    else
                    {
                        return new KeyValuePair<JwtTokenValidationStatus, LoginResponseModel>(JwtTokenValidationStatus.Expired, null);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.LogException("", "JwtValidate", "ValidateJwtToken", ex);
                return new KeyValuePair<JwtTokenValidationStatus, LoginResponseModel>(JwtTokenValidationStatus.Invalid, null);
            }
        }

        private static string GetClaim(ClaimsPrincipal claimsPrincipal, string claim)
        {
            foreach (Claim item in claimsPrincipal.Claims)
            {
                if (item.Type == claim)
                {
                    return item.Value;
                }
            }
            return string.Empty;
        }

        internal static string CreateToken(User user, string sessionId, long validUntilTimestamp)
        {
            string preSharedKey = Utilities.Utility.GetConfigValue("jwtTokenKey");

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(preSharedKey));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtHeader header = new JwtHeader(credentials);

            List<Claim> claims = new List<Claim> {
        new Claim("timestamp", DateTime.UtcNow.GetUnixTimeStamp().ToString(CultureInfo.InvariantCulture)),
        new Claim("exp", validUntilTimestamp.ToString(CultureInfo.InvariantCulture)),
        new Claim(Constants.CLAIM_USER_ID, user.UserId),
        new Claim(Constants.CLAIM_SESSION_ID, sessionId)
      };
            JwtPayload payload = new JwtPayload(claims);

            JwtSecurityToken secToken = new JwtSecurityToken(header, payload);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            return handler.WriteToken(secToken);
        }
    }
}
