using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TeamLease.CssService.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "api_log",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    request_id = table.Column<string>(nullable: true),
                    application_name = table.Column<string>(nullable: true),
                    accessed_by = table.Column<string>(nullable: true),
                    machine = table.Column<string>(nullable: true),
                    request_ip = table.Column<string>(nullable: true),
                    request_content_type = table.Column<string>(nullable: true),
                    request_body = table.Column<string>(nullable: true),
                    request_uri = table.Column<string>(nullable: true),
                    request_method = table.Column<string>(nullable: true),
                    request_headers = table.Column<string>(nullable: true),
                    request_timestamp = table.Column<DateTime>(nullable: true),
                    response_content_type = table.Column<string>(nullable: true),
                    response_body = table.Column<string>(nullable: true),
                    response_status_code = table.Column<int>(nullable: true),
                    response_headers = table.Column<string>(nullable: true),
                    response_timestamp = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_api_log", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "app_events",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    event_date = table.Column<DateTime>(nullable: false),
                    parent_type = table.Column<string>(nullable: true),
                    parent_id = table.Column<string>(nullable: true),
                    event_name = table.Column<string>(nullable: true),
                    event_data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "app_log",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    request_id = table.Column<string>(nullable: true),
                    log_date = table.Column<DateTime>(nullable: false),
                    log_user = table.Column<string>(nullable: true),
                    log_level = table.Column<string>(nullable: true),
                    log_message = table.Column<string>(nullable: true),
                    exception = table.Column<string>(nullable: true),
                    admin_notified = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_log", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Chart_Type",
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
                    chart_id = table.Column<int>(nullable: false),
                    chart_name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chart_Type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "emails",
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
                    subject = table.Column<string>(nullable: true),
                    body = table.Column<string>(nullable: true),
                    to = table.Column<List<string>>(nullable: true),
                    cc = table.Column<List<string>>(nullable: true),
                    bcc = table.Column<List<string>>(nullable: true),
                    send_from = table.Column<string>(nullable: true),
                    attachment_name = table.Column<string>(nullable: true),
                    attachment_url = table.Column<string>(nullable: true),
                    attachment_data = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_emails", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Report_Config",
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
                    rc_userid = table.Column<string>(nullable: true),
                    rc_name = table.Column<string>(nullable: true),
                    rc_query = table.Column<string>(nullable: true),
                    rc_email = table.Column<string>(nullable: true),
                    rc_fileformat = table.Column<string>(nullable: true),
                    rc_schedule = table.Column<string>(nullable: true),
                    rc_modifiedon = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report_Config", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "settings",
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
                    key = table.Column<string>(nullable: true),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
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
                    branchname = table.Column<string>(nullable: true),
                    branchcode = table.Column<string>(nullable: true),
                    makerid = table.Column<string>(nullable: true),
                    assignedto = table.Column<string>(nullable: true),
                    postingdate = table.Column<DateTime>(nullable: true),
                    functionid = table.Column<string>(nullable: true),
                    transaction_status = table.Column<string>(nullable: true),
                    branch_type = table.Column<string>(nullable: true),
                    status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "upload_log",
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
                    client_id = table.Column<string>(nullable: true),
                    am_id = table.Column<string>(nullable: true),
                    associatename = table.Column<string>(nullable: true),
                    upload_type = table.Column<int>(nullable: false),
                    log_message = table.Column<string>(nullable: true),
                    upload_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_upload_log", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User_Dashboard",
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
                    ud_userid = table.Column<string>(nullable: true),
                    ud_charttype = table.Column<string>(nullable: true),
                    ud_widgetname = table.Column<string>(nullable: true),
                    ud_query = table.Column<string>(nullable: true),
                    ud_querylevel1 = table.Column<string>(nullable: true),
                    ud_querylevel2 = table.Column<string>(nullable: true),
                    ud_querylevel3 = table.Column<string>(nullable: true),
                    ud_modifiedon = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Dashboard", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_sessions",
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
                    user_id = table.Column<string>(nullable: true),
                    session_id = table.Column<string>(nullable: true),
                    last_api_call = table.Column<DateTime>(nullable: false),
                    active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_sessions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
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
                    last_logged_in = table.Column<DateTime>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    user_id = table.Column<string>(nullable: true),
                    user_type = table.Column<int>(nullable: false),
                    password = table.Column<string>(nullable: false),
                    user_status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_app_events_parent_type_parent_id",
                schema: "public",
                table: "app_events",
                columns: new[] { "parent_type", "parent_id" });

            migrationBuilder.CreateIndex(
                name: "IX_app_log_log_level_admin_notified",
                schema: "public",
                table: "app_log",
                columns: new[] { "log_level", "admin_notified" });

            migrationBuilder.CreateIndex(
                name: "IX_user_sessions_user_id_session_id_deleted",
                schema: "public",
                table: "user_sessions",
                columns: new[] { "user_id", "session_id", "deleted" });

            migrationBuilder.CreateIndex(
                name: "IX_users_user_id_email_deleted",
                schema: "public",
                table: "users",
                columns: new[] { "user_id", "email", "deleted" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "api_log",
                schema: "public");

            migrationBuilder.DropTable(
                name: "app_events",
                schema: "public");

            migrationBuilder.DropTable(
                name: "app_log",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Chart_Type",
                schema: "public");

            migrationBuilder.DropTable(
                name: "emails",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Report_Config",
                schema: "public");

            migrationBuilder.DropTable(
                name: "settings",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Transactions",
                schema: "public");

            migrationBuilder.DropTable(
                name: "upload_log",
                schema: "public");

            migrationBuilder.DropTable(
                name: "User_Dashboard",
                schema: "public");

            migrationBuilder.DropTable(
                name: "user_sessions",
                schema: "public");

            migrationBuilder.DropTable(
                name: "users",
                schema: "public");
        }
    }
}
