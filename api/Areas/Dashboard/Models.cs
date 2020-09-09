using ASNRTech.CoreService.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASNRTech.CoreService.Dashboard
{
    [Table("Chart_Type", Schema = "public")]
    public class ChartType
    {
        [Key]
        [Column("chart_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChartId { get; set; }

        [Column("chart_name")]
        public string ChartName { get; set; }
    }

    [Table("DBConnections", Schema = "public")]
    public class DBConnection
    {
        [Key]
        [Column("dbconnection_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DBConnectionId { get; set; }

        [Column("dbconnection_name")]
        public string DBConnectionName { get; set; }
    }

    [Table("User_Dashboard", Schema = "public")]
    public class UserDashboard : BaseModel
    {
        [Column("ud_widgetname")]
        public string DashboardWidgetName { get; set; }

        [Column("ud_charttype")]
        public string DashboardChartType { get; set; }

        [Column("ud_userpermission")]
        public string DashboardUserPermission { get; set; }

        [Column("ud_emailformat")]
        public string DashboardEmailFormat { get; set; }

        [Column("ud_widgetconnectionstring")]
        public string WidgetConnectionString { get; set; }

        [Column("ud_widgetschedulertype")]
        public string WidgetSchedulerType { get; set; }

        [Column("ud_widgetscheduleremailids")]
        public string WidgetSchedulerEmailIDs { get; set; }

        [Column("ud_widgetquery")]
        public string WidgetQuery { get; set; }

        [Column("ud_l1connectionstring")]
        public string Level1ConnectionString { get; set; }

        [Column("ud_l1schedulertype")]
        public string Level1SchedulerType { get; set; }

        [Column("ud_l1scheduleremailids")]
        public string L1SchedulerEmailIDs { get; set; }

        [Column("ud_querylevel1")]
        public string DashbaordQueryL1 { get; set; }

        [Column("ud_l2connectionstring")]
        public string Level2ConnectionString { get; set; }

        [Column("ud_l2schedulertype")]
        public string Level2SchedulerType { get; set; }

        [Column("ud_l2scheduleremailids")]
        public string L2SchedulerEmailIDs { get; set; }

        [Column("ud_querylevel2")]
        public string DashbaordQueryL2 { get; set; }

        [Column("ud_l3connectionstring")]
        public string Level3ConnectionString { get; set; }

        [Column("ud_l3schedulertype")]
        public string Level3SchedulerType { get; set; }

        [Column("ud_l3scheduleremailids")]
        public string L3SchedulerEmailIDs { get; set; }

        [Column("ud_querylevel3")]
        public string DashbaordQueryL3 { get; set; }

        [Column("ud_l4connectionstring")]
        public string Level4ConnectionString { get; set; }

        [Column("ud_l4schedulertype")]
        public string Level4SchedulerType { get; set; }

        [Column("ud_l4scheduleremailids")]
        public string L4SchedulerEmailIDs { get; set; }

        [Column("ud_querylevel4")]
        public string DashbaordQueryL4 { get; set; }

        [Column("ud_sendemail")]
        public bool WidgetSendEmail { get; set; }
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

        [Column("email")]
        public string UserEmail { get; set; }
    }

    public class DashboardWidget
    {
        public string DashboardWidgetName { get; set; }
        public int WidgetId { get; set; }
        public string DashboardChartType { get; set; }
        public string DashboardUserPermission { get; set; }
        public string DashboardEmailFormat { get; set; }
        public string WidgetConnectionString { get; set; }
        public string WidgetSchedulerType { get; set; }
        public string WidgetSchedulerEmailIDs { get; set; }
        public string WidgetQuery { get; set; }
        public string Level1ConnectionString { get; set; }
        public string Level1SchedulerType { get; set; }
        public string L1SchedulerEmailIDs { get; set; }
        public string DashbaordQueryL1 { get; set; }
        public string Level2ConnectionString { get; set; }
        public string Level2SchedulerType { get; set; }
        public string L2SchedulerEmailIDs { get; set; }
        public string DashbaordQueryL2 { get; set; }
        public string Level3ConnectionString { get; set; }
        public string Level3SchedulerType { get; set; }
        public string L3SchedulerEmailIDs { get; set; }
        public string DashbaordQueryL3 { get; set; }
        public string Level4ConnectionString { get; set; }
        public string Level4SchedulerType { get; set; }
        public string L4SchedulerEmailIDs { get; set; }
        public string DashbaordQueryL4 { get; set; }
        public bool WidgetSendEmail { get; set; }
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

    public class LoadDashboard
    {
        public string DashboardWidgetName { get; set; }
        public int DashboardWidgetId { get; set; }
        public string DashboardWidgetType { get; set; }
        public WidgetRead[] DashbaordWidgetData { get; set; }
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

    public class ChartTypes
    {
        public int ChartId { get; set; }
        public string ChartName { get; set; }
    }

    public class SchedulerTypes
    {
        public int SchedulerId { get; set; }
        public string SchedulerName { get; set; }
    }

    public class DBConnectionStrings
    {
        public int DBConnectionId { get; set; }
        public string DBConnectionName { get; set; }
    }

    public class AllWidgetDropDowns
    {
        public List<ChartTypes> Charts { get; set; }
        public List<string> Users { get; set; }
        public List<SchedulerTypes> Schedulers { get; set; }
        public List<DBConnectionStrings> ConnectionStrings { get; set; }
    }
}
