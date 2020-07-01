using CsvHelper.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.Enums;

namespace ASNRTech.CoreService.Reports {
    [Table("Report_Config", Schema = "public")]
    public class ReportConfig : BaseModel {

        [Column("config_id")]
        public int ReportConfigId { get; set; }

        [Column("rc_userid")]
        public string ReportReqUserId { get; set; }

        [Column("rc_name")]
        public string ReportName { get; set; }

        [Column("rc_query")]
        public string ReportQuery { get; set; }

        [Column("rc_email")]
        public string ReportEmail { get; set; }

        [Column("rc_fileformat")]
        public string ReportFileFormat { get; set; }

        [Column("rc_schedule")]
        public string ReportSchedule { get; set; }

        [Column("rc_createdby")]
        public string ReportCreatedBy { get; set; }

        [Column("rc_createdon")]
        public DateTime? ReportCreatedOn { get; set; }

        //[Column("rc_modifiedby")]
        //public string ReportModifiedBy { get; set; }

        [Column("rc_modifiedon")]
        public DateTime? ReportModifiedOn { get; set; }
    }
}
