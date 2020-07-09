using System;

namespace ASNRTech.CoreService.Enums
{
    public enum EnLogLevel
    {
        ALL = 1,
        DEBUG = 2,
        INFO = 3,
        WARN = 4,
        ERROR = 5,
        FATAL = 6,
        OFF = 7
    }

    public enum DatabaseType
    {
        Postgres = 10,
        SqlServer = 20
    }

    [Flags]
    public enum AccessType
    {
        Open = 0,
        Admin = 1,
        Client = 2,
        None = 3
    }

    public enum ReportColumnDataType
    {
        StringType = 1,
        IntType = 2,
        DoubleType = 3,
        DateTimeType = 4,
        MoneyType = 5
    }

    [Flags]
    public enum SecurityCheckType
    {
        None = 0,
        Client = 1
    }

    public enum JwtTokenValidationStatus
    {
        Valid = 1,
        NoToken = 2,
        Expired = 3,
        Invalid = 4
    }

    public enum RouteAccessStatus
    {
        Ok = 1,
        UnAuthorized = 2,
        NotFound = 3
    }

    public enum UserStatus
    {
        Active = 1,
        InActive = 2,
    }

    public enum UserType
    {
        Admin = 1,
        Client = 2
    }

    public enum Charts
    {
        BarChart = 1,
        PieChart = 2
    }

    //public enum PasswordResetType
    //{
    //    Email = 1,
    //    Phone = 2
    //}

    public enum LoginStatus
    {
        LoggedIn = 1,
        InValid = 2,
        NoAccess = 3,
        ContractExpired = 4,
        DeActivated = 5
    }
}
