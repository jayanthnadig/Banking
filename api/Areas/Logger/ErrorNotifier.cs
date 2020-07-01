using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASNRTech.CoreService.Core;
using ASNRTech.CoreService.Data;
using ASNRTech.CoreService.Email;
using Utility = ASNRTech.CoreService.Utilities.Utility;

namespace ASNRTech.CoreService.Logging {
    public class ErrorNotifier {

        public static async Task NotifyErrorsAsync(TeamHttpContext teamContext) {
            using (TeamDbContext dbContext = new TeamDbContext()) {
                List<AppLogEntry> errors = dbContext.AppLogEntries.Where(e => e.Level == "ERROR" && !e.AdminNotified).Take(25).ToList();
                HashSet<string> sentErrors = new HashSet<string>();
                StringBuilder sb = new StringBuilder();

                foreach (AppLogEntry item in errors) {
                    // don't notify for the same error
                    string messageKey = $"{item.User}:{item.ExceptionMessage}";

                    // skip certain errors
                    // check that we have not notified this user/error combination.
                    if (!Utility.IsDevelopment
                      && !item.ExceptionMessage.Contains("The timeout period elapsed prior to completion of the", StringComparison.InvariantCulture)
                      && !item.ExceptionMessage.Contains("Unauthorized, Please login to continue", StringComparison.InvariantCulture)
                      && !sentErrors.Contains(messageKey)) {
                        sb.Append(Utility.GetFormattedLabelValue("Id", item.Id));
                        sb.Append(Utility.GetFormattedLabelValue("User", item.User));
                        sb.Append(Utility.GetFormattedLabelValue("Thread", item.RequestId));
                        sb.Append(Utility.GetFormattedLabelValue("Time", item.Date.ToString(CultureInfo.InvariantCulture)));
                        sb.Append(Utility.GetFormattedLabelValue("Location", item.Message));
                        sb.Append(Utility.GetFormattedLabelValue("Message", item.ExceptionMessage));
                        sb.Append("<br/><br/>");

                        sentErrors.Add(messageKey);
                    }
                    item.AdminNotified = true;
                }

                string body = sb.ToString();
                if (body.Length != 0) {
                    string adminMailAddress = Utility.GetConfigValue("notifications:sysAdminEmailAddress");
                    await EmailService.SendAsync(adminMailAddress, "IA:Application Errors", body).ConfigureAwait(false);
                }

                if (errors.Count > 0) {
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                }
            }
        }
    }
}
