using Core.Comman;
using Core.Interface;
using Core.Model;
using CrudOperations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AttendanceReportRepository : IAttendanceReportRepository
    {
        private readonly ICrudOperationService _crudOperation;
        private static readonly string _con = string.Empty;
        public static IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_con);
            }
        }
        public AttendanceReportRepository(ICrudOperationService crudOperation)
        {
            this._crudOperation = crudOperation;
        }
        public async Task<Response> LogInOrLogOutAttendenceReport(LogInOrLogOutAttendenceReport logInOrLogOutAttendenceReport)
        {
            return await _crudOperation.InsertUpdateDelete<Response>(storedProcedureName: "[dbo].[uspLoginOrLogout]", logInOrLogOutAttendenceReport);
        }
        public async Task<Response> AttendenceDelete(AttendenceDelete attendenceDelete)
        {
            return await _crudOperation.InsertUpdateDelete<Response>(storedProcedureName: "[dbo].[uspLoginOrLogoutDelete]", attendenceDelete);
        }
        public async Task<ResponseList<AttendenceReport>> AttendenceGetList(string employeeId, JqueryDataTable list)
        {
            return await _crudOperation.GetPaginatedList<AttendenceReport>(storedProcedureName: "[dbo].[uspAttendenceReportList]",
                new
                {
                    employeeId,
                    list.SearchKey,
                    list.Start,
                    list.SortCol,
                    list.PageSize
                });
        }
        public async Task<Response<AttendenceReport>> AttendenceGetDetails(AttendenceGetDetails attendenceGetDetails)
        {
            return await _crudOperation.GetSingleRecord<AttendenceReport>(storedProcedureName: "[dbo].[uspLogInLogOutGetDetails]", attendenceGetDetails);
        }
    }
}
