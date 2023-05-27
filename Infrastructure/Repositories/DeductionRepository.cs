using Core.Comman;
using Core.Interface;
using Core.Model;
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
    public class DeductionRepository : IDeductionRepository
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
        public DeductionRepository(ICrudOperationService crudOperation)
        {
            this._crudOperation = crudOperation;
        }
        public async Task<Response> DeductionInsert(DeductionInsert deductionInsert)
        {
            return await _crudOperation.InsertUpdateDelete<Response>(storedProcedureName: "[dbo].[uspDeductionInsert]", deductionInsert);
        }
        public async Task<Response<Deduction>> DeductionGetDeatails(DeductionGetDeatails deductionGetDeatails)
        {
            return await _crudOperation.GetSingleRecord<Deduction>(storedProcedureName: "[dbo].[uspDeducationGetDetails]", deductionGetDeatails);
        }
        public async Task<Response> DeductionUpdate(DeductionUpdate deductionUpdate)
        {
            return await _crudOperation.InsertUpdateDelete<Response>(storedProcedureName: "[dbo].[uspDeductionUpdate]", deductionUpdate);
        }
        public async Task<Response> DeductionDelete(DeductionDelete deductionDelete)
        {
            return await _crudOperation.InsertUpdateDelete<Response>(storedProcedureName: "[dbo].[uspDeductionDelete]", deductionDelete);
        }
        public async Task<ResponseList<Deduction>> DeductionList(string employeeId, JqueryDataTable list)
        {
            return await _crudOperation.GetPaginatedList<Deduction>(storedProcedureName: "[dbo].[uspDeductionList]", new
            {
                employeeId,
                list.SearchKey,
                list.PageSize,
                list.Start,
                list.SortCol

            });
        }
    }
}
