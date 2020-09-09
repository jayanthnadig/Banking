using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TeamLease.CssService.Migrations
{
    public partial class v8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ud_modifiedon",
                schema: "public",
                table: "User_Dashboard");

            migrationBuilder.DropColumn(
                name: "rc_frequency",
                schema: "public",
                table: "Report_Config");

            migrationBuilder.DropColumn(
                name: "rc_frequencyvalue",
                schema: "public",
                table: "Report_Config");

            migrationBuilder.DropColumn(
                name: "rc_sendtime",
                schema: "public",
                table: "Report_Config");

            migrationBuilder.DropColumn(
                name: "rc_workendtime",
                schema: "public",
                table: "Report_Config");

            migrationBuilder.RenameColumn(
                name: "ud_dashboardconnectionstring",
                schema: "public",
                table: "User_Dashboard",
                newName: "ud_widgetschedulertype");

            migrationBuilder.RenameColumn(
                name: "rc_workstarttime",
                schema: "public",
                table: "Report_Config",
                newName: "rc_schedulername");

            migrationBuilder.AddColumn<string>(
                name: "ud_emailformat",
                schema: "public",
                table: "User_Dashboard",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ud_userpermission",
                schema: "public",
                table: "User_Dashboard",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ud_l1scheduleremailids",
                schema: "public",
                table: "User_Dashboard",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ud_l2scheduleremailids",
                schema: "public",
                table: "User_Dashboard",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ud_l3scheduleremailids",
                schema: "public",
                table: "User_Dashboard",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ud_l4scheduleremailids",
                schema: "public",
                table: "User_Dashboard",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ud_l1schedulertype",
                schema: "public",
                table: "User_Dashboard",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ud_l2schedulertype",
                schema: "public",
                table: "User_Dashboard",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ud_l3schedulertype",
                schema: "public",
                table: "User_Dashboard",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ud_l4schedulertype",
                schema: "public",
                table: "User_Dashboard",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ud_widgetconnectionstring",
                schema: "public",
                table: "User_Dashboard",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ud_widgetscheduleremailids",
                schema: "public",
                table: "User_Dashboard",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ud_sendemail",
                schema: "public",
                table: "User_Dashboard",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Scheduler",
                schema: "public",
                columns: table => new
                {
                    created_by = table.Column<string>(nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    deleted = table.Column<bool>(nullable: false),
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    modified_by = table.Column<string>(nullable: true),
                    modified_date = table.Column<DateTime>(nullable: true),
                    sd_name = table.Column<string>(nullable: true),
                    sd_workstarttime = table.Column<string>(nullable: true),
                    sd_workendtime = table.Column<string>(nullable: true),
                    sd_frequency = table.Column<string>(nullable: true),
                    sd_sendtime = table.Column<string>(nullable: true),
                    sd_frequencyvalue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scheduler", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Scheduler",
                schema: "public");

            migrationBuilder.DropColumn(
                name: "ud_emailformat",
                schema: "public",
                table: "User_Dashboard");

            migrationBuilder.DropColumn(
                name: "ud_userpermission",
                schema: "public",
                table: "User_Dashboard");

            migrationBuilder.DropColumn(
                name: "ud_l1scheduleremailids",
                schema: "public",
                table: "User_Dashboard");

            migrationBuilder.DropColumn(
                name: "ud_l2scheduleremailids",
                schema: "public",
                table: "User_Dashboard");

            migrationBuilder.DropColumn(
                name: "ud_l3scheduleremailids",
                schema: "public",
                table: "User_Dashboard");

            migrationBuilder.DropColumn(
                name: "ud_l4scheduleremailids",
                schema: "public",
                table: "User_Dashboard");

            migrationBuilder.DropColumn(
                name: "ud_l1schedulertype",
                schema: "public",
                table: "User_Dashboard");

            migrationBuilder.DropColumn(
                name: "ud_l2schedulertype",
                schema: "public",
                table: "User_Dashboard");

            migrationBuilder.DropColumn(
                name: "ud_l3schedulertype",
                schema: "public",
                table: "User_Dashboard");

            migrationBuilder.DropColumn(
                name: "ud_l4schedulertype",
                schema: "public",
                table: "User_Dashboard");

            migrationBuilder.DropColumn(
                name: "ud_widgetconnectionstring",
                schema: "public",
                table: "User_Dashboard");

            migrationBuilder.DropColumn(
                name: "ud_widgetscheduleremailids",
                schema: "public",
                table: "User_Dashboard");

            migrationBuilder.DropColumn(
                name: "ud_sendemail",
                schema: "public",
                table: "User_Dashboard");

            migrationBuilder.RenameColumn(
                name: "ud_widgetschedulertype",
                schema: "public",
                table: "User_Dashboard",
                newName: "ud_dashboardconnectionstring");

            migrationBuilder.RenameColumn(
                name: "rc_schedulername",
                schema: "public",
                table: "Report_Config",
                newName: "rc_workstarttime");

            migrationBuilder.AddColumn<DateTime>(
                name: "ud_modifiedon",
                schema: "public",
                table: "User_Dashboard",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rc_frequency",
                schema: "public",
                table: "Report_Config",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rc_frequencyvalue",
                schema: "public",
                table: "Report_Config",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rc_sendtime",
                schema: "public",
                table: "Report_Config",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rc_workendtime",
                schema: "public",
                table: "Report_Config",
                nullable: true);
        }
    }
}
