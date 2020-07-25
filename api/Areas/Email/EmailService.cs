using ASNRTech.CoreService.Data;
using ASNRTech.CoreService.Logging;
using ASNRTech.CoreService.Utilities;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ASNRTech.CoreService.Email
{
    public static class EmailService
    {
        private static readonly string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();

        private static List<string> CleanupEmails(List<string> incomingList)
        {
            List<string> emails = new List<string>();
            //emails1.CopyTo(emails);
            foreach (string item in incomingList)
            {
                if (item.Contains(",", StringComparison.InvariantCulture))
                {
                    emails.AddRange(item.Split(','));
                }
                else if (item.Contains(";", StringComparison.InvariantCulture))
                {
                    emails.AddRange(item.Split(';'));
                }
                else
                {
                    emails.Add(item);
                }
            }

            emails = emails.Select(s => s.Trim()).ToList();
            if (!Utility.IsProduction)
            {
                // don't send to non white-listed accounts
                emails = emails.Where(IsInternalEmail).ToList();
            }

            return emails;
        }

        internal static async Task SendAsync(EmailDto dto)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(Utility.GetConfigValue("notifications:defaultFromAddress"));
                mail.To.Add(dto.To[0]);
                mail.Subject = dto.Subject;
                mail.Body = dto.Body;
                mail.IsBodyHtml = false;
                mail.Attachments.Add(new Attachment(dto.AttachmentS3Url));

                using (SmtpClient smtp = new SmtpClient(mail.From.ToString(), Convert.ToInt32(Utility.GetConfigValue("notifications:port"))))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(Utility.GetConfigValue("notifications:defaultFromAddress"), Utility.GetConfigValue("notifications:password"));
                    smtp.EnableSsl = true;
                    try
                    {
                        smtp.Send(mail);
                    }
                    catch (Exception ex)
                    {
                        LoggerService.LogException("", "Email Service", "SendAsync", ex, true);
                    }
                }
            }
            using (TeamDbContext dbContext = new TeamDbContext())
            {
                dbContext.Emails.Add(dto);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }

            LoggerService.LogInfo(className, "LevelDetails", "Mail Sent to: {0} with subject: {1}", dto.To.ToString(), dto.Subject);
        }

        internal static async Task SendAsync(EmailDto email, StringBuilder sb)
        {
            if (sb != null)
            {
                using (StreamWriter sw = new StreamWriter(new MemoryStream()))
                {
                    sw.Write(sb.ToString());
                    sw.Flush();

                    email.AttachmentData = sw.BaseStream.ToByteArray();
                }
            }
            await EmailService.SendAsync(email).ConfigureAwait(false);
        }

        internal static async Task SendAsync(string toMailAddress, string subject, string body, string path)
        {
            EmailDto dto = new EmailDto();
            dto.To.Add(toMailAddress);
            dto.Subject = subject;
            dto.Body = body;
            dto.AttachmentS3Url = path;

            await EmailService.SendAsync(dto).ConfigureAwait(false);
        }

        private static bool IsInternalEmail(string emailAddress)
        {
            emailAddress = emailAddress.ToUpperInvariant();
            string[] whitelist = Utility.GetConfigValue("notifications:emailWhiteList").ToUpperInvariant().Split(',');

            try
            {
                MailAddress address = new MailAddress(emailAddress);
                string host = address.Host;

                if (whitelist.Contains(host))
                {
                    return true;
                }
                else if (whitelist.Contains(emailAddress))
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
