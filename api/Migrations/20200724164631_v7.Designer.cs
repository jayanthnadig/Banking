﻿// <auto-generated />
using System;
using System.Collections.Generic;
using ASNRTech.CoreService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TeamLease.CssService.Migrations
{
    [DbContext(typeof(TeamDbContext))]
    [Migration("20200724164631_v7")]
    partial class v7
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("PropertyAccessMode", PropertyAccessMode.Property)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("ASNRTech.CoreService.Core.Models.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("CreatedBy")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("created_date");

                    b.Property<bool>("Deleted")
                        .HasColumnName("deleted");

                    b.Property<string>("Key")
                        .HasColumnName("key");

                    b.Property<string>("ModifiedBy")
                        .HasColumnName("modified_by");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnName("modified_date");

                    b.Property<string>("Value")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.ToTable("settings","public");
                });

            modelBuilder.Entity("ASNRTech.CoreService.Dashboard.ChartType", b =>
                {
                    b.Property<int>("ChartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("chart_id");

                    b.Property<string>("ChartName")
                        .HasColumnName("chart_name");

                    b.HasKey("ChartId");

                    b.ToTable("Chart_Type","public");
                });

            modelBuilder.Entity("ASNRTech.CoreService.Dashboard.DBConnection", b =>
                {
                    b.Property<int>("DBConnectionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("dbconnection_id");

                    b.Property<string>("DBConnectionName")
                        .HasColumnName("dbconnection_name");

                    b.HasKey("DBConnectionId");

                    b.ToTable("DBConnections","public");
                });

            modelBuilder.Entity("ASNRTech.CoreService.Dashboard.Transactions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("AssignedTo")
                        .HasColumnName("assignedto");

                    b.Property<string>("BranchCode")
                        .HasColumnName("branchcode");

                    b.Property<string>("BranchName")
                        .HasColumnName("branchname");

                    b.Property<string>("BranchType")
                        .HasColumnName("branch_type");

                    b.Property<string>("CreatedBy")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("created_date");

                    b.Property<bool>("Deleted")
                        .HasColumnName("deleted");

                    b.Property<string>("FunctionId")
                        .HasColumnName("functionid");

                    b.Property<string>("MakerId")
                        .HasColumnName("makerid");

                    b.Property<string>("ModifiedBy")
                        .HasColumnName("modified_by");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnName("modified_date");

                    b.Property<DateTime?>("PostingDate")
                        .HasColumnName("postingdate");

                    b.Property<string>("Status")
                        .HasColumnName("status");

                    b.Property<int>("TransactionId")
                        .HasColumnName("id");

                    b.Property<string>("TransactionStatus")
                        .HasColumnName("transaction_status");

                    b.Property<string>("UserEmail")
                        .HasColumnName("email");

                    b.HasKey("Id");

                    b.ToTable("Transactions","public");
                });

            modelBuilder.Entity("ASNRTech.CoreService.Dashboard.UserDashboard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("CreatedBy")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("created_date");

                    b.Property<DateTime?>("DashbaordModifiedOn")
                        .HasColumnName("ud_modifiedon");

                    b.Property<string>("DashbaordQuery")
                        .HasColumnName("ud_widgetquery");

                    b.Property<string>("DashbaordQueryL1")
                        .HasColumnName("ud_querylevel1");

                    b.Property<string>("DashbaordQueryL2")
                        .HasColumnName("ud_querylevel2");

                    b.Property<string>("DashbaordQueryL3")
                        .HasColumnName("ud_querylevel3");

                    b.Property<string>("DashbaordQueryL4")
                        .HasColumnName("ud_querylevel4");

                    b.Property<string>("DashboardChartType")
                        .HasColumnName("ud_charttype");

                    b.Property<string>("DashboardConnectionString")
                        .HasColumnName("ud_dashboardconnectionstring");

                    b.Property<string>("DashboardUserId")
                        .HasColumnName("ud_userid");

                    b.Property<string>("DashboardWidgetName")
                        .HasColumnName("ud_widgetname");

                    b.Property<bool>("Deleted")
                        .HasColumnName("deleted");

                    b.Property<string>("Level1ConnectionString")
                        .HasColumnName("ud_l1connectionstring");

                    b.Property<string>("Level2ConnectionString")
                        .HasColumnName("ud_l2connectionstring");

                    b.Property<string>("Level3ConnectionString")
                        .HasColumnName("ud_l3connectionstring");

                    b.Property<string>("Level4ConnectionString")
                        .HasColumnName("ud_l4connectionstring");

                    b.Property<string>("ModifiedBy")
                        .HasColumnName("modified_by");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnName("modified_date");

                    b.HasKey("Id");

                    b.ToTable("User_Dashboard","public");
                });

            modelBuilder.Entity("ASNRTech.CoreService.Email.EmailDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<byte[]>("AttachmentData")
                        .HasColumnName("attachment_data");

                    b.Property<string>("AttachmentName")
                        .HasColumnName("attachment_name");

                    b.Property<string>("AttachmentS3Url")
                        .HasColumnName("attachment_url");

                    b.Property<List<string>>("Bcc")
                        .HasColumnName("bcc");

                    b.Property<string>("Body")
                        .HasColumnName("body");

                    b.Property<List<string>>("Cc")
                        .HasColumnName("cc");

                    b.Property<string>("CreatedBy")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("created_date");

                    b.Property<bool>("Deleted")
                        .HasColumnName("deleted");

                    b.Property<string>("ModifiedBy")
                        .HasColumnName("modified_by");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnName("modified_date");

                    b.Property<string>("SendFrom")
                        .HasColumnName("send_from");

                    b.Property<string>("Subject")
                        .HasColumnName("subject");

                    b.Property<List<string>>("To")
                        .HasColumnName("to");

                    b.HasKey("Id");

                    b.ToTable("emails","public");
                });

            modelBuilder.Entity("ASNRTech.CoreService.Logging.ApiLogEntry", b =>
                {
                    b.Property<long>("ApiLogEntryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Application")
                        .HasColumnName("application_name");

                    b.Property<string>("Machine")
                        .HasColumnName("machine");

                    b.Property<string>("RequestContentBody")
                        .HasColumnName("request_body");

                    b.Property<string>("RequestContentType")
                        .HasColumnName("request_content_type");

                    b.Property<string>("RequestHeaders")
                        .HasColumnName("request_headers");

                    b.Property<string>("RequestId")
                        .HasColumnName("request_id");

                    b.Property<string>("RequestIpAddress")
                        .HasColumnName("request_ip");

                    b.Property<string>("RequestMethod")
                        .HasColumnName("request_method");

                    b.Property<DateTime?>("RequestTimestamp")
                        .HasColumnName("request_timestamp");

                    b.Property<string>("RequestUri")
                        .HasColumnName("request_uri");

                    b.Property<string>("ResponseContentBody")
                        .HasColumnName("response_body");

                    b.Property<string>("ResponseContentType")
                        .HasColumnName("response_content_type");

                    b.Property<string>("ResponseHeaders")
                        .HasColumnName("response_headers");

                    b.Property<int?>("ResponseStatusCode")
                        .HasColumnName("response_status_code");

                    b.Property<DateTime?>("ResponseTimestamp")
                        .HasColumnName("response_timestamp");

                    b.Property<string>("User")
                        .HasColumnName("accessed_by");

                    b.HasKey("ApiLogEntryId");

                    b.ToTable("api_log","public");
                });

            modelBuilder.Entity("ASNRTech.CoreService.Logging.AppEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Data")
                        .HasColumnName("event_data");

                    b.Property<DateTime>("Date")
                        .HasColumnName("event_date");

                    b.Property<string>("Name")
                        .HasColumnName("event_name");

                    b.Property<string>("ParentId")
                        .HasColumnName("parent_id");

                    b.Property<string>("ParentType")
                        .HasColumnName("parent_type");

                    b.HasKey("Id");

                    b.HasIndex("ParentType", "ParentId");

                    b.ToTable("app_events","public");
                });

            modelBuilder.Entity("ASNRTech.CoreService.Logging.AppLogEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<bool>("AdminNotified")
                        .HasColumnName("admin_notified");

                    b.Property<DateTime>("Date")
                        .HasColumnName("log_date");

                    b.Property<string>("ExceptionMessage")
                        .HasColumnName("exception");

                    b.Property<string>("Level")
                        .HasColumnName("log_level");

                    b.Property<string>("Message")
                        .HasColumnName("log_message");

                    b.Property<string>("RequestId")
                        .HasColumnName("request_id");

                    b.Property<string>("User")
                        .HasColumnName("log_user");

                    b.HasKey("Id");

                    b.HasIndex("Level", "AdminNotified");

                    b.ToTable("app_log","public");
                });

            modelBuilder.Entity("ASNRTech.CoreService.Reports.ReportConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("CreatedBy")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("created_date");

                    b.Property<bool>("Deleted")
                        .HasColumnName("deleted");

                    b.Property<string>("ModifiedBy")
                        .HasColumnName("modified_by");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnName("modified_date");

                    b.Property<string>("ReportConnectionString")
                        .HasColumnName("rc_connectionstring");

                    b.Property<string>("ReportDefaultSMSMsg")
                        .HasColumnName("rc_SMSmsg");

                    b.Property<string>("ReportDeliveryOption")
                        .HasColumnName("rc_deliveryoption");

                    b.Property<string>("ReportEmail")
                        .HasColumnName("rc_email");

                    b.Property<string>("ReportFileFormat")
                        .HasColumnName("rc_fileformat");

                    b.Property<string>("ReportFrequecy")
                        .HasColumnName("rc_frequency");

                    b.Property<string>("ReportFrequecyValue")
                        .HasColumnName("rc_frequencyvalue");

                    b.Property<DateTime?>("ReportModifiedOn")
                        .HasColumnName("rc_modifiedon");

                    b.Property<string>("ReportName")
                        .HasColumnName("rc_name");

                    b.Property<string>("ReportQuery")
                        .HasColumnName("rc_query");

                    b.Property<string>("ReportReqUserId")
                        .HasColumnName("rc_userid");

                    b.Property<string>("ReportSMSPhNo")
                        .HasColumnName("rc_SMSphno");

                    b.Property<string>("ReportSendTime")
                        .HasColumnName("rc_sendtime");

                    b.Property<string>("ReportWorkEndTime")
                        .HasColumnName("rc_workendtime");

                    b.Property<string>("ReportWorkStartTime")
                        .HasColumnName("rc_workstarttime");

                    b.HasKey("Id");

                    b.ToTable("Report_Config","public");
                });

            modelBuilder.Entity("ASNRTech.CoreService.Security.UploadLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("AmId")
                        .HasColumnName("am_id");

                    b.Property<string>("ClientId")
                        .HasColumnName("client_id");

                    b.Property<string>("CreatedBy")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("created_date");

                    b.Property<bool>("Deleted")
                        .HasColumnName("deleted");

                    b.Property<string>("ErrorMessage")
                        .HasColumnName("log_message");

                    b.Property<string>("ModifiedBy")
                        .HasColumnName("modified_by");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnName("modified_date");

                    b.Property<string>("associateName")
                        .HasColumnName("associatename");

                    b.Property<int>("jobID")
                        .HasColumnName("upload_id");

                    b.Property<int>("type")
                        .HasColumnName("upload_type");

                    b.HasKey("Id");

                    b.ToTable("upload_log","public");
                });

            modelBuilder.Entity("ASNRTech.CoreService.Security.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("CreatedBy")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("created_date");

                    b.Property<bool>("Deleted")
                        .HasColumnName("deleted");

                    b.Property<string>("Email")
                        .HasColumnName("email");

                    b.Property<bool>("IsAdd")
                        .HasColumnName("add_permission");

                    b.Property<bool>("IsDelete")
                        .HasColumnName("delete_permission");

                    b.Property<bool>("IsEdit")
                        .HasColumnName("edit_permission");

                    b.Property<DateTime?>("LastLoggedIn")
                        .HasColumnName("last_logged_in");

                    b.Property<string>("ModifiedBy")
                        .HasColumnName("modified_by");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnName("modified_date");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("password");

                    b.Property<int>("Status")
                        .HasColumnName("user_status");

                    b.Property<string>("UserId")
                        .HasColumnName("user_id");

                    b.Property<int>("UserType")
                        .HasColumnName("user_type");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "Email", "Deleted");

                    b.ToTable("users","public");
                });

            modelBuilder.Entity("ASNRTech.CoreService.Security.UserSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<bool>("Active")
                        .HasColumnName("active");

                    b.Property<string>("CreatedBy")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnName("created_date");

                    b.Property<bool>("Deleted")
                        .HasColumnName("deleted");

                    b.Property<DateTime>("LastApiCall")
                        .HasColumnName("last_api_call");

                    b.Property<string>("ModifiedBy")
                        .HasColumnName("modified_by");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnName("modified_date");

                    b.Property<string>("SessionId")
                        .HasColumnName("session_id");

                    b.Property<string>("UserId")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "SessionId", "Deleted");

                    b.ToTable("user_sessions","public");
                });
#pragma warning restore 612, 618
        }
    }
}
