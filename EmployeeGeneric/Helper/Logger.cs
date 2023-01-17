using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace EmployeeGeneric.Helper
{
    public static class Logger
    {
        private static readonly IConfigurationRoot _iconfiguration;
        private static readonly string? con = string.Empty;
        static Logger()
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                 .AddJsonFile("appsettings.json");
            _iconfiguration = builder.Build();

            con = _iconfiguration["ConnectionStrings:DataAccessConnection"];
        }

        public static IDbConnection Connection
        {
            get
            {
                return new SqlConnection(con);
            }
        }

        public static void AddErrorLog(string? controller, string? action, string? createdBy, Exception exception, string? requestBody = null)
        {
            if (exception is SqlException exception1)
            {
                using IDbConnection db = Connection;

                db.ExecuteScalar("[dbo].[uspErrorLogInsert]", new
                {
                    controller,
                    action,
                    Message = exception.Message + " error at " + exception1.Procedure,
                    Source = exception1.Procedure,
                    exception.StackTrace,
                    LineNo = new StackTrace(exception, true).GetFrame(0).GetFileLineNumber(),
                    createdBy,
                    requestBody
                }, commandType: CommandType.StoredProcedure);

            }
            else
            {
                using IDbConnection db = Connection;

                db.ExecuteScalar("[dbo].[uspErrorLogInsert]", new
                {
                    controller,
                    action,
                    exception.Message,
                    exception.Source,
                    exception.StackTrace,
                    LineNo = new StackTrace(exception, true).GetFrame(0).GetFileLineNumber(),
                    createdBy,
                    requestBody
                }, commandType: CommandType.StoredProcedure);

            }
        }

        public static void SaveLog(UserActivityLog userActivityLog)
        {
            using IDbConnection db = Connection;

            db.Execute("[dbo].[uspUserActivityLogInsert]", new
            {
                userActivityLog.UserID,
                userActivityLog.IpAddress,
                userActivityLog.AreaAccessed,
                userActivityLog.TimeStamp,
                userActivityLog.Body,
                userActivityLog.StatusCode,
                userActivityLog.Method,
            }, commandType: CommandType.StoredProcedure);

        }
    }
    public class UserActivityLog
    {
        public string? UserID { get; set; }
        public string? IpAddress { get; set; }
        public string? AreaAccessed { get; set; }
        public DateTime TimeStamp { get; set; }
        public string? Body { get; set; }
        public int StatusCode { get; set; }
        public string? Method { get; set; }
    }
}

