using Amazon;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ASNRTech.CoreService.Data;
using ASNRTech.CoreService.Logging;
using ASNRTech.CoreService.Utilities;

namespace ASNRTech.CoreService.Email {
    public static class EmailService {
        private static readonly string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();

        private static List<string> CleanupEmails(List<string> incomingList) {
            List<string> emails = new List<string>();
            //emails1.CopyTo(emails);
            foreach (string item in incomingList) {
                if (item.Contains(",", StringComparison.InvariantCulture)) {
                    emails.AddRange(item.Split(','));
                }
                else if (item.Contains(";", StringComparison.InvariantCulture)) {
                    emails.AddRange(item.Split(';'));
                }
                else {
                    emails.Add(item);
                }
            }

            emails = emails.Select(s => s.Trim()).ToList();
            if (!Utility.IsProduction) {
                // don't send to non white-listed accounts
                emails = emails.Where(IsInternalEmail).ToList();
            }

            return emails;
        }

        internal static async Task SendAsync(EmailDto dto) {
            const string methodName = "SendMail";
            const string BODY_HTML = "<html><body style=\"font-family:'Verdana'\">{0}<br/<br/><font style=\"color: gray; font-size: 0.9em;\">Sent from ASNRTech at {1}</font></body></html>";

            MimeMessage message = new MimeMessage();
            if (dto.CcAdmin) {
                message.Bcc.Add(new MailboxAddress(Utility.GetConfigValue("notifications:sysAdminEmailAddress")));
            }

            // use the specified from, if it exists
            if (!dto.SendFrom.HasValue()) {
                dto.SendFrom = Utility.GetConfigValue("notifications:defaultFromAddress");
            }
            message.From.Add(new MailboxAddress(dto.SendFrom));

            dto.To = CleanupEmails(dto.To);
            dto.Cc = CleanupEmails(dto.Cc);
            dto.Bcc = CleanupEmails(dto.Bcc);

            if (!Utility.IsProduction) {
                // all external emails, so send this to administrator
                if (dto.To.Count == 0) {
                    dto.To.Add(Utility.GetConfigValue("notifications:sysAdminEmailAddress"));
                }

                // identify as test mails
                dto.Subject = $"{Utility.Environment}: {dto.Subject}";
            }

            message.Subject = dto.Subject;
            foreach (string s in dto.To) {
                message.To.Add(new MailboxAddress(s));
            }
            foreach (string s in dto.Cc) {
                message.Cc.Add(new MailboxAddress(s));
            }
            foreach (string s in dto.Bcc) {
                message.Bcc.Add(new MailboxAddress(s));
            }

            //AWSCredentials credentials = Utility.GetAwsKeyAndSecret();
            //using (MemoryStream stream = new MemoryStream()) {
            //    BodyBuilder body = new BodyBuilder();

            //    if (string.IsNullOrEmpty(dto.Body)) {
            //        dto.Body = "";
            //    }

            //    body.HtmlBody = string.Format(BODY_HTML, dto.Body, DateUtility.GetCurrentDateTimeForDisplay);

            //    if (!string.IsNullOrWhiteSpace(dto.AttachmentS3Url)) {
            //        dto.AttachmentData = await S3.DownloadFileAsync(dto.AttachmentS3Url).ConfigureAwait(false);
            //    }

            //    if (dto.AttachmentData != null && dto.AttachmentData.GetLongLength(0) != 0) {
            //        body.Attachments.Add(dto.AttachmentName, dto.AttachmentData);
            //    }
            //    message.Body = body.ToMessageBody();
            //    message.WriteTo(stream);

            //    SendRawEmailRequest sendRequest = new SendRawEmailRequest {
            //        RawMessage = new RawMessage(stream)
            //    };

            //    using (AmazonSimpleEmailServiceClient client = new AmazonSimpleEmailServiceClient(credentials, RegionEndpoint.APSouth1)) {
            //        try {
            //            SendRawEmailResponse response = await client.SendRawEmailAsync(sendRequest).ConfigureAwait(false);
            //        }
            //        catch (Exception ex) {
            //            LoggerService.LogException("", "Email Service", "SendAsync", ex, true);
            //        }
            //    }
            //}

            using (TeamDbContext dbContext = new TeamDbContext()) {
                dbContext.Emails.Add(dto);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }

            LoggerService.LogInfo(className, methodName, "Mail Sent to: {0} with subject: {1}", dto.To.ToString(), dto.Subject);
        }

        internal static async Task SendAsync(EmailDto email, StringBuilder sb) {
            if (sb != null) {
                using (StreamWriter sw = new StreamWriter(new MemoryStream())) {
                    sw.Write(sb.ToString());
                    sw.Flush();

                    email.AttachmentData = sw.BaseStream.ToByteArray();
                }
            }
            await EmailService.SendAsync(email).ConfigureAwait(false);
        }

        internal static async Task SendAsync(string toMailAddress, string subject, string body) {
            EmailDto dto = new EmailDto();
            dto.To.Add(toMailAddress);
            dto.Subject = subject;
            dto.Body = body;

            await EmailService.SendAsync(dto).ConfigureAwait(false);
        }

        private static bool IsInternalEmail(string emailAddress) {
            emailAddress = emailAddress.ToUpperInvariant();
            string[] whitelist = Utility.GetConfigValue("notifications:emailWhiteList").ToUpperInvariant().Split(',');

            try {
                MailAddress address = new MailAddress(emailAddress);
                string host = address.Host;

                if (whitelist.Contains(host)) {
                    return true;
                }
                else if (whitelist.Contains(emailAddress)) {
                    return true;
                }
                return false;
            }
            catch {
                return false;
            }
        }
    }
}
