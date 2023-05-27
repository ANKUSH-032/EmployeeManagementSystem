using Core.Comman;
using CORE.Interface;
using CORE.Model;
using CrudOperation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class LeaveRepository : ILeaveRepository
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
        public LeaveRepository(ICrudOperationService crudOperation)
        {
            this._crudOperation = crudOperation;
        }
        public async Task<Response> LeaveInsert(LeaveInsert leaveInsert)
        {
            return await _crudOperation.InsertUpdateDelete<Response>(storedProcedureName: "[dbo].[uspLeaveInsert]", leaveInsert);
        }
        public async Task<Response<Leave>> LeaveGetDetails(LeaveGetDetails leaveGetDetails)
        {
            return await _crudOperation.GetSingleRecord<Leave>(storedProcedureName: "[dbo].[uspLeaveGetDetails]", leaveGetDetails);
        }
        public async Task<Response> LeaveUpdate(LeaveUpdate leaveUpdate)
        {
            return await _crudOperation.InsertUpdateDelete<Response>(storedProcedureName: "[dbo].[uspLeaveUpdate]", leaveUpdate);
        }
        public async Task<Response> LeaveDelete(LeaveDelete leaveDelete)
        {
            return await _crudOperation.InsertUpdateDelete<Response>(storedProcedureName: "[dbo].[uspLeaveDelete]", leaveDelete);
        }
        public async Task<ResponseList<Leave>> LeaveGetList(string employeeId, JqueryDataTable list)
        {
            return await _crudOperation.GetPaginatedList<Leave>(storedProcedureName: "[dbo].[uspLeaveList]", new
            {
                employeeId,
                list.SearchKey,
                list.PageSize,
                list.SortCol,
                list.Start
            });
        }
    }
}
