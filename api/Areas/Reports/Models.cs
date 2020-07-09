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

        [Column("rc_query")]
        public string ReportQuery { get; set; }

        [Column("rc_email")]
        public string ReportEmail { get; set; }

        [Column("rc_fileformat")]
        public string ReportFileFormat { get; set; }

        [Column("rc_schedule")]
        public string ReportSchedule { get; set; }

        [Column("rc_modifiedon")]
        public DateTime? ReportModifiedOn { get; set; }
    }

    public class ReportList
    {
        public int ReportId { get; set; }
        public string ReportName { get; set; }
        public bool IsActive { get; set; }
    }

    public class ReportConfigAddEdit : ReportList
    {
        public string ReportQuery { get; set; }
        public string ReportEmail { get; set; }
        public string ReportFormat { get; set; }
        public string ReportInterval { get; set; }
    }

    public class ReportGrid : ReportList
    {
        public string[] GridColumns { get; set; }
        public List<string[]> GridData { get; set; }
    }
}
