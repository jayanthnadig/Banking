using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamLease.CssService.Migrations
{
    public partial class v9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ud_userid",
                schema: "public",
                table: "User_Dashboard");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ud_userid",
                schema: "public",
                table: "User_Dashboard",
                nullable: true);
        }
    }
}
