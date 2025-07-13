namespace Tests.Configuration;

public static class TestConfiguration
{
    public static class Employee
    {
        public const string GetAll = "api/employee?PageIndex=1&PageSize=10";
        public const string GetById = "api/employee/{0}";
        public const string Create = "api/employee";
        public const string Update = "api/employee/{0}";
        public const string Delete = "api/employee/{0}";
    }
    
    public static class Attendance
    {
        public const string GetAll = "api/attendance?PageIndex=1&PageSize=10";
        public const string GetById = "api/attendance/{0}";
        public const string Create = "api/attendance";
        public const string Update = "api/attendance/{0}";
        public const string Delete = "api/attendance/{0}";
    }
    
    public static class Payroll
    {
        public const string GetAll = "api/payroll?PageIndex=1&PageSize=10";
        public const string GetById = "api/payroll/{0}";
        public const string Create = "api/payroll";
        public const string Update = "api/payroll/{0}";
        public const string Delete = "api/payroll/{0}";
    }
}