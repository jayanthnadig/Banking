using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamLease.CssService.Migrations
{
    public partial class v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "rc_connectionstring",
                schema: "public",
                table: "Report_Config",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rc_connectionstring",
                schema: "public",
                table: "Report_Config");
        }
    }
}
