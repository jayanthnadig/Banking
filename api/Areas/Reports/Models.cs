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

        [Column("rc_schedulername")]
        public string ReportSchedulerName { get; set; }
    }

    [Table("Scheduler", Schema = "public")]
    public class Scheduler : BaseModel
    {
        [Column("sd_name")]
        public string SchedulerName { get; set; }

        [Column("sd_workstarttime")]
        public string SchedulerWorkStartTime { get; set; }

        [Column("sd_workendtime")]
        public string SchedulerWorkEndTime { get; set; }

        [Column("sd_frequency")]
        public string SchedulerFrequecy { get; set; }

        [Column("sd_sendtime")]
        public string SchedulerSendTime { get; set; }

        [Column("sd_frequencyvalue")]
        public string SchedulerFrequecyValue { get; set; }
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
        public string ReportSchedulerName { get; set; }
    }

    public class ReportGrid : ReportList
    {
        public string[] GridColumns { get; set; }
        public List<string[]> GridData { get; set; }
    }

    public class SchedulerList
    {
        public int SchedulerId { get; set; }
        public string SchedulerName { get; set; }
    }

    public class SchedulerAddUpdate : SchedulerList
    {
        public string SchedulerWorkStartTime { get; set; }
        public string SchedulerWorkEndTime { get; set; }
        public string SchedulerSendFrequency { get; set; }
        public string SchedulerSendTime { get; set; }
        public string SchedulerSendFrequencyValue { get; set; }
    }
}
