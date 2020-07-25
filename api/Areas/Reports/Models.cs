using ASNRTech.CoreService.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASNRTech.CoreService.Reports
{
    [Table("Report_Config", Schema = "public")]
    public class ReportConfig : BaseModel
    {
        [Column("rc_userid")]
        public string ReportReqUserId { get; set; }

        [Column("rc_name")]
        public string ReportName { get; set; }

        [Column("rc_connectionstring")]
        public string ReportConnectionString { get; set; }

        [Column("rc_query")]
        public string ReportQuery { get; set; }

        [Column("rc_email")]
        public string ReportEmail { get; set; }

        [Column("rc_fileformat")]
        public string ReportFileFormat { get; set; }

        [Column("rc_modifiedon")]
        public DateTime? ReportModifiedOn { get; set; }

        [Column("rc_deliveryoption")]
        public string ReportDeliveryOption { get; set; }

        [Column("rc_SMSphno")]
        public string ReportSMSPhNo { get; set; }

        [Column("rc_SMSmsg")]
        public string ReportDefaultSMSMsg { get; set; }

        [Column("rc_workstarttime")]
        public string ReportWorkStartTime { get; set; }

        [Column("rc_workendtime")]
        public string ReportWorkEndTime { get; set; }

        [Column("rc_frequency")]
        public string ReportFrequecy { get; set; }

        [Column("rc_sendtime")]
        public string ReportSendTime { get; set; }

        [Column("rc_frequencyvalue")]
        public string ReportFrequecyValue { get; set; }
    }

    public class ReportList
    {
        public int ReportId { get; set; }
        public string ReportName { get; set; }
        public bool IsActive { get; set; }
    }

    public class ReportConfigAddEdit : ReportList
    {
        public string ReportConnectionString { get; set; }
        public string ReportQuery { get; set; }
        public string ReportEmail { get; set; }
        public string ReportFormat { get; set; }
        public string ReportDeliveryMode { get; set; }
        public string ReportSMSPhoneNumber { get; set; }
        public string ReportDefaultSMSMSG { get; set; }
        public string ReportWorkStartTime { get; set; }
        public string ReportWorkEndTime { get; set; }
        public string ReportSendFrequency { get; set; }
        public string ReportSendTime { get; set; }
        public string ReportSendFrequencyValue { get; set; }
    }

    public class ReportGrid : ReportList
    {
        public string[] GridColumns { get; set; }
        public List<string[]> GridData { get; set; }
    }
}
