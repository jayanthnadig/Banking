using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.Enums;

namespace ASNRTech.CoreService.Dashboard
{
    public class DashboardWidget
    {
        public string WidgetName { get; set; }
        public int WidgetId { get; set; }
        public string WidgetType { get; set; }
        public string WidgetQuery { get; set; }
        public string WidgetQueryLevel1 { get; set; }
        public string WidgetQueryLevel2 { get; set; }
        public string WidgetQueryLevel3 { get; set; }
    }

    [Table("Chart_Type", Schema = "public")]
    public class ChartType : BaseModel
    {
        [Column("chart_id")]
        public int ChartId { get; set; }

        [Column("chart_name")]
        public string ChartName { get; set; }
    }

    [Table("User_Dashboard", Schema = "public")]
    public class UserDashboard : BaseModel
    {
        [Column("ud_userid")]
        public string DashboardUserId { get; set; }

        [Column("ud_charttype")]
        public string DashboardChartType { get; set; }

        [Column("ud_widgetname")]
        public string DashboardWidgetName { get; set; }

        [Column("ud_query")]
        public string DashbaordQuery { get; set; }

        [Column("ud_querylevel1")]
        public string DashbaordQueryL1 { get; set; }

        [Column("ud_querylevel2")]
        public string DashbaordQueryL2 { get; set; }

        [Column("ud_querylevel3")]
        public string DashbaordQueryL3 { get; set; }

        [Column("ud_modifiedon")]
        public DateTime? DashbaordModifiedOn { get; set; }
    }

    [Table("Transactions", Schema = "public")]
    public class Transactions : BaseModel
    {
        [Column("id")]
        public int TransactionId { get; set; }

        [Column("branchname")]
        public string BranchName { get; set; }

        [Column("branchcode")]
        public string BranchCode { get; set; }

        //[Column("total_transactions")]
        //public int TotalTransactions { get; set; }

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

    public class Widgets : LoadWidgets
    {
        public int ChartID { get; set; }
        public string WidgetValues { get; set; }
        public string WidgetQuery { get; set; }
    }

    public class LoadWidgets : DashboardWidget
    {
        public WidgetRead[] WidgetData { get; set; }
    }

    public class OnScreenClick
    {
        public string ClickLevel { get; set; }
        public int ClickedWidgetId { get; set; }
        public string ClickedOnValue { get; set; }
        public string[] GridColumns { get; set; }
        public List<string[]> GridData { get; set; }
        public SelectedGridInput[] GridInput { get; set; }
    }

    public class WidgetRead
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public class SelectedGridInput
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
