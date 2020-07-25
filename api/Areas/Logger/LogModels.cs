using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASNRTech.CoreService.Logging
{
    [Table("api_log", Schema = "public")]
    public class ApiLogEntry
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ApiLogEntryId { get; set; }

        [Column("request_id")]
        public string RequestId { get; set; }

        [Column("application_name")]
        public string Application { get; set; }

        [Column("accessed_by")]
        public string User { get; set; }

        [Column("machine")]
        public string Machine { get; set; }

        [Column("request_ip")]
        public string RequestIpAddress { get; set; }

        [Column("request_content_type")]
        public string RequestContentType { get; set; }

        [Column("request_body")]
        public string RequestContentBody { get; set; }

        [Column("request_uri")]
        public string RequestUri { get; set; }

        [Column("request_method")]
        public string RequestMethod { get; set; }

        [NotMapped]
        public string RequestRouteTemplate { get; set; }

        [NotMapped]
        public string RequestRouteData { get; set; }

        [Column("request_headers")]
        public string RequestHeaders { get; set; }

        [Column("request_timestamp")]
        public DateTime? RequestTimestamp { get; set; }

        [Column("response_content_type")]
        public string ResponseContentType { get; set; }

        [Column("response_body")]
        public string ResponseContentBody { get; set; }

        [Column("response_status_code")]
        public int? ResponseStatusCode { get; set; }

        [Column("response_headers")]
        public string ResponseHeaders { get; set; }

        [Column("response_timestamp")]
        public DateTime? ResponseTimestamp { get; set; }

        public ApiLogEntry()
        {
            this.RequestIpAddress = string.Empty;
            this.ResponseHeaders = string.Empty;
            this.RequestTimestamp = this.ResponseTimestamp = DateTime.Now;
        }
    }

    [Table("app_log", Schema = "public")]
    public class AppLogEntry
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("request_id")]
        public string RequestId { get; set; }

        [Column("log_date")]
        public DateTime Date { get; set; }

        [Column("log_user")]
        public string User { get; set; }

        [Column("log_level")]
        public string Level { get; set; }

        [Column("log_message")]
        public string Message { get; set; }

        [Column("exception")]
        public string ExceptionMessage { get; set; }

        [Column("admin_notified")]
        public bool AdminNotified { get; set; }
    }

    [Table("app_events", Schema = "public")]
    public class AppEvent
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("event_date")]
        public DateTime Date { get; set; }

        [Column("parent_type")]
        public string ParentType { get; set; }

        [Column("parent_id")]
        public string ParentId { get; set; }

        [Column("event_name")]
        public string Name { get; set; }

        [Column("event_data")]
        public string Data { get; set; }

        public AppEvent()
        {
            this.Date = DateTime.Now;
        }
    }
}
