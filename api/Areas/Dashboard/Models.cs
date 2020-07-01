using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.Enums;

namespace ASNRTech.CoreService.Dashboard {
    public class DashboardWidget {
        public string WidgetName { get; set; }
        public int WidgetID { get; set; }
        public string WidgetType { get; set; }
        public string WidgetQuery { get; set; }
    }

    [Table("Chart_Type", Schema = "public")]
    public class ChartType : BaseModel {

        [Column("chart_id")]
        public int ChartId { get; set; }

        [Column("chart_name")]
        public string ChartName { get; set; }
    }

    [Table("User_Dashboard", Schema = "public")]
    public class UserDashboard : BaseModel {

        [Column("ud_id")]
        public int DashboardId { get; set; }

        [Column("ud_userid")]
        public string DashboardUserId { get; set; }

        [Column("ud_charttype")]
        public string DashboardChartType { get; set; }

        [Column("ud_widgetname")]
        public string DashboardWidgetName { get; set; }

        [Column("ud_query")]
        public string DashbaordQuery { get; set; }

        [Column("ud_createdby")]
        public string DashbaordCreatedBy { get; set; }

        [Column("ud_createdon")]
        public DateTime? DashbaordCreatedOn { get; set; }

        [Column("ud_modifiedon")]
        public DateTime? DashbaordModifiedOn { get; set; }
    }

    [Table("Transactions", Schema = "public")]
    public class Transactions : BaseModel {

        [Column("id")]
        public int TransactionId { get; set; }

        [Column("branchname")]
        public string BranchName { get; set; }

        [Column("branchcode")]
        public string BranchCode { get; set; }

        [Column("total_transactions")]
        public int TotalTransactions { get; set; }

        [Column("makerid")]
        public string MakerId { get; set; }

        [Column("assignedto")]
        public string AssignedTo { get; set; }

        [Column("postingdate")]
        public DateTime? PostingDate { get; set; }

        [Column("functionid")]
        public string FunctionId { get; set; }

        [Column("transaction_status")]
        public string TransactionStatus { get; set; }

        [Column("branch_type")]
        public string BranchType { get; set; }

        [Column("status")]
        public string Status { get; set; }
    }

    public class Widgets : LoadWidgets {
        public int ChartID { get; set; }
        public string WidgetValues { get; set; }
        public string WidgetQuery { get; set; }
    }

    public class LoadWidgets : DashboardWidget {

        //public string WidgetName { get; set; }
        //public int WidgetID { get; set; }
        //public string WidgetType { get; set; }
        public WidgetRead[] WidgetData { get; set; }
    }

    public class WidgetRead {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
