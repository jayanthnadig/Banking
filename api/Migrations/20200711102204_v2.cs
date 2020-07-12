using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamLease.CssService.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "add_permission",
                schema: "public",
                table: "users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "delete_permission",
                schema: "public",
                table: "users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "edit_permission",
                schema: "public",
                table: "users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "add_permission",
                schema: "public",
                table: "users");

            migrationBuilder.DropColumn(
                name: "delete_permission",
                schema: "public",
                table: "users");

            migrationBuilder.DropColumn(
                name: "edit_permission",
                schema: "public",
                table: "users");
        }
    }
}
