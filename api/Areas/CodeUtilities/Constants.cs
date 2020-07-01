using System;
using System.Xml;

namespace ASNRTech.CoreService.Utilities {
    public static class Constants {
        internal const string DB_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        internal const string SYSTEM_USER_ID = "system";
        internal static DateTime DefaultDate = XmlConvert.ToDateTime("1900-01-01", "yyyy-MM-dd");
        internal static DateTime MIN_DATE = new DateTime(1990, 1, 1);
        internal static DateTime MAX_DATE = new DateTime(2100, 1, 1);
        internal const string INDIA_DATE_FORMAT = "dd/MM/yyyy";
        internal const string INDIA_DATE_TIME_FORMAT = "dd/MM/yyyy HH:mm";
        internal const string DEFAULT_DATE_FORMAT = "dd MMM yyyy";
        internal static int DEFAULT_PAGE_SIZE = 10;
        internal static int DEFAULT_MAX_LIMIT = 100000000;

        internal const string REGEX_MOBILE = @"[6789]\d{9}";
        internal const string REGEX_PIN_CODE = @"\d{6}";
        internal const string REGEX_PASSWORD = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@,#,$,%,^,&,*,_])[A-Za-z\d@,#,$,%,^,&,*,_]{8,15}$";
        internal const string CONTEXT_CORRELATION_ID = "correlationId";
        internal const string CONTEXT_USER = "user";
        internal const string CONTEXT_CLIENT_ID = "clientId";
        internal const string CONTEXT_INVOICE_ID = "invoiceId";

        internal const string HEADER_CONTENT_TYPE = "Content-Type";
        internal const string HEADER_SENT_AT = "sent_at";
        internal const string HEADER_API_KEY = "x-api-key";
        internal const string HEADER_SIGNATURE = "x-signature";
        internal const string HEADER_JWT_CLIENT_TOKEN = "x-client-token";

        internal const string SORT_BY_ID = "id";

        internal const string HTTP_CODE_DUPLICATE = "409";
        internal const string HTTP_CODE_BAD_REQUEST = "400";
        internal const string DUMMY_FILE = "DUMMY_FILE_DATA";

        internal const string ROUTE_PARAM_CLIENT_ID = "clientId";
        internal const string ROUTE_PARAM_INVOICE_ID = "invoiceId";
        internal const string ROUTE_PARAM_ASSOCIATE_ID = "empId";
        internal const string QUERY_PARAM_JWT = "access_token";
        internal const string CLAIM_USER_ID = "user-id";
        internal const string CLAIM_SESSION_ID = "session-id";

        internal const string CACHE_SESSION_ID = "session-id:";
        internal const string CACHE_USER = "user:";
        internal const string CACHE_INVOICE_IDS = "invoice-ids:";
        internal const string CACHE_ASSOCIATE_IDS = "emp-ids:";

        internal const string SETTING_USERS_LAST_SYNCED_AT = "users_last_synced_at";
        internal const string PARENT_TYPE_ASSOCIATE = "associate";
        internal const string PARENT_TYPE_CLIENT = "client";
        internal const string DOCUMENT_TYPE_PROFILE_IMAGE = "Profile";

        internal const string REPORT_CUSTOM_ALL_COLUMNS = "ALL COLUMNS FOR CUSTOM REPORT";
        internal const string REPORT_STOP_PAYMENT = "Stop payment";
        internal const string REPORT_CONTRACT_EXTENSION = "Contract Extension";
        internal const string REPORT_INDUCTION = "Induction";
        internal const string REPORT_SALARY_REVISION = "Salary Revision";
        internal const string REPORT_SALARY_REGISTER = "Salary Register";
        internal const string REPORT_ASSOCIATE_MASTER_CTC_ACTIVE_ASSOCIATES = "Associate Master CTC Active Associates";
        internal const string REPORT_CLIENT_SPECIFIC_EMP_MASTER = "Employee master client-specific";
        internal const string REPORT_BILLING_SALARY_REGISTER = "Billing Salary register";
        internal const string REPORT_OPS_CONSOLIDATED_STOP_PAY = "Ops Consolidated Stop Pay";
        internal const string REPORT_OPS = "Ops Report";

        internal const string PROC_REPORT_ASSOCIATE_MASTER_CTC = "PROC_ALCS_REPORT_ASSOCIATE_MASTER_CTC";
        internal const string PROC_CONTRACT_EXTENSION = "Proc_rpt_Contract_Extension_Rpt_PeriodWise";
        internal const string PROC_INDUCTION = "PROC_ALCS_REPORT_Induction_details";
        internal const string PROC_SALARY_REGISTER = "Proc_rpt_Salary_register";
        internal const string PROC_BILLING_SALARY_REGISTER = "Proc_rpt_Salary_Register_Annexure";
        internal const string PROC_SALARY_REVISION = "PROC_ALCS_REPORT_SAL_REV_ALL_CLIENT";
        internal const string PROC_STOP_PAYMENT_DETAILS = "Proc_rpt_MonthlyStoppaymentDetails";
        internal const string PROC_CSS_GET_CUSTOM_REPORT_DETAILS = "css_get_custom_report_records";
        internal const string PROC_RPT_EMPLOYEEMASTER_CLIENTSPECIFIC = "PROC_RPT_EMPLOYEEMASTER_CLIENTSPECIFIC";
        internal const string PROC_OPS_STOP_PAY = "css_get_ops_stoppay";
        internal const string PROC_OPS_REPORT = "css_get_ops_details";

        internal const string PROC_OPS_STOP_PAY_COUNT = "css_get_ops_stoppay_count";
        internal const string PROC_OPS_REPORT_COUNT = "css_get_ops_report_count";

        internal const string PROC_CHECK_APPROVAL_PENDING = "PROC_ALCS_CHECK_APPROVALS_PENDING";

        internal const string DEFAULT_ASSC_REPORT_TYPE = "ACTIVE";
        internal const string ALCS_SYNC_USER_ID = "css-user";
        internal const int INVOICE_GENERATION_MAX_ATTEMPTS = 2;

        internal const string REPORT_HEADER_STYLE = "style='font-size:22px;font-weight:600;color:#0a1f46;font-family:Ubuntu,sans-serif'";
        internal const string TABLE_STYLE = "<style> table {background-color:#ffffff;border-collapse:collapse;font-family:'Ubuntu'}th {font-weight:600;color:#34404e;text-align: left;}td {font-weight:400;font-size:15px;}tr:nth-child(odd){background-color: #f5f6fb;}tr:nth-child(even){background-color: #fff;}</style >";
    }
}
