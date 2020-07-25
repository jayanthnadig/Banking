using ASNRTech.CoreService.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASNRTech.CoreService.Email
{
    [Table("emails", Schema = "public")]
    public class EmailDto : BaseModel
    {
        [Column("subject")]
        public string Subject { get; set; }

        [Column("body")]
        public string Body { get; set; }

        [Column("to")]
        public List<string> To { get; set; }

        [Column("cc")]
        public List<string> Cc { get; set; }

        [Column("bcc")]
        public List<string> Bcc { get; set; }

        [Column("send_from")]
        public string SendFrom { get; set; }

        [Column("attachment_name")]
        public string AttachmentName { get; set; }

        [Column("attachment_url")]
        public string AttachmentS3Url { get; set; }

        [Column("attachment_data")]
        public byte[] AttachmentData { get; set; }

        [NotMapped]
        public bool CcAdmin { get; set; }

        public EmailDto()
        {
            this.Subject = string.Empty;
            this.AttachmentName = "Data.csv";
            this.Body = string.Empty;
            this.CcAdmin = false;
            this.To = new List<string>();
            this.Cc = new List<string>();
            this.Bcc = new List<string>();
            this.AttachmentData = Array.Empty<byte>();
            this.AttachmentS3Url = string.Empty;
            this.SendFrom = "";
        }
    }
}
