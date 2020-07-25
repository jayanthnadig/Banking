using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamLease.CssService.Migrations
{
    public partial class v6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rc_schedule",
                schema: "public",
                table: "Report_Config");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "rc_schedule",
                schema: "public",
                table: "Report_Config",
                nullable: true);
        }
    }
}
