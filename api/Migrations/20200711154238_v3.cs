using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TeamLease.CssService.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Chart_Type",
                schema: "public",
                table: "Chart_Type");

            migrationBuilder.DropColumn(
                name: "id",
                schema: "public",
                table: "Chart_Type");

            migrationBuilder.DropColumn(
                name: "created_by",
                schema: "public",
                table: "Chart_Type");

            migrationBuilder.DropColumn(
                name: "created_date",
                schema: "public",
                table: "Chart_Type");

            migrationBuilder.DropColumn(
                name: "deleted",
                schema: "public",
                table: "Chart_Type");

            migrationBuilder.DropColumn(
                name: "modified_by",
                schema: "public",
                table: "Chart_Type");

            migrationBuilder.DropColumn(
                name: "modified_date",
                schema: "public",
                table: "Chart_Type");

            migrationBuilder.RenameColumn(
                name: "ud_query",
                schema: "public",
                table: "User_Dashboard",
                newName: "ud_widgetquery");

            migrationBuilder.AddColumn<string>(
                name: "ud_querylevel4",
                schema: "public",
                table: "User_Dashboard",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ud_dashboardconnectionstring",
                schema: "public",
                table: "User_Dashboard",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ud_l1connectionstring",
                schema: "public",
                table: "User_Dashboard",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ud_l2connectionstring",
                schema: "public",
                table: "User_Dashboard",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ud_l3connectionstring",
                schema: "public",
                table: "User_Dashboard",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ud_l4connectionstring",
                schema: "public",
                table: "User_Dashboard",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "chart_id",
                schema: "public",
                table: "Chart_Type",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chart_Type",
                schema: "public",
                table: "Chart_Type",
                column: "chart_id");

            migrationBuilder.CreateTable(
                name: "DBConnections",
                schema: "public",
                columns: table => new
                {
                    dbconnection_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    dbconnection_name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBConnections", x => x.dbconnection_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DBConnections",
                schema: "public");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chart_Type",
                schema: "public",
                table: "Chart_Type");

            migrationBuilder.DropColumn(
                name: "ud_querylevel4",
                schema: "public",
                table: "User_Dashboard");

            migrationBuilder.DropColumn(
                name: "ud_dashboardconnectionstring",
                schema: "public",
                table: "User_Dashboard");

            migrationBuilder.DropColumn(
                name: "ud_l1connectionstring",
                schema: "public",
                table: "User_Dashboard");

            migrationBuilder.DropColumn(
                name: "ud_l2connectionstring",
                schema: "public",
                table: "User_Dashboard");

            migrationBuilder.DropColumn(
                name: "ud_l3connectionstring",
                schema: "public",
                table: "User_Dashboard");

            migrationBuilder.DropColumn(
                name: "ud_l4connectionstring",
                schema: "public",
                table: "User_Dashboard");

            migrationBuilder.RenameColumn(
                name: "ud_widgetquery",
                schema: "public",
                table: "User_Dashboard",
                newName: "ud_query");

            migrationBuilder.AlterColumn<int>(
                name: "chart_id",
                schema: "public",
                table: "Chart_Type",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<int>(
                name: "id",
                schema: "public",
                table: "Chart_Type",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                schema: "public",
                table: "Chart_Type",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                schema: "public",
                table: "Chart_Type",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "deleted",
                schema: "public",
                table: "Chart_Type",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "modified_by",
                schema: "public",
                table: "Chart_Type",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "modified_date",
                schema: "public",
                table: "Chart_Type",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chart_Type",
                schema: "public",
                table: "Chart_Type",
                column: "id");
        }
    }
}
