using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamLease.CssService.Migrations
{
    public partial class v5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "rc_SMSmsg",
                schema: "public",
                table: "Report_Config",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rc_deliveryoption",
                schema: "public",
                table: "Report_Config",
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
                name: "rc_SMSphno",
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

            migrationBuilder.AddColumn<string>(
                name: "rc_workstarttime",
                schema: "public",
                table: "Report_Config",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rc_SMSmsg",
                schema: "public",
                table: "Report_Config");

            migrationBuilder.DropColumn(
                name: "rc_deliveryoption",
                schema: "public",
                table: "Report_Config");

            migrationBuilder.DropColumn(
                name: "rc_frequency",
                schema: "public",
                table: "Report_Config");

            migrationBuilder.DropColumn(
                name: "rc_frequencyvalue",
                schema: "public",
                table: "Report_Config");

            migrationBuilder.DropColumn(
                name: "rc_SMSphno",
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

            migrationBuilder.DropColumn(
                name: "rc_workstarttime",
                schema: "public",
                table: "Report_Config");
        }
    }
}
