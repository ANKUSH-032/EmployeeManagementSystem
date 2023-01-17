using Core.Comman;
using Core.Interface;
using Core.Model;
using CrudOperations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AllowanceRepository : IAllowanceRepository
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
        public AllowanceRepository(ICrudOperationService crudOperation)
        {
            this._crudOperation = crudOperation;
        }
        public async Task<Response> AllowancesInsert(AllowancesInsert allowancesInsert)
        {
           // TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
           // allowancesInsert.ClinicName = textInfo.ToTitleCase(allowancesInsert.);
            return await _crudOperation.InsertUpdateDelete<Response>(storedProcedureName: "[dbo].[uspAllowanceInsert]", allowancesInsert);
        }
        public async Task<Response<Allowances>> AllowancesGetDetails(AllowancesGetDetails allowancesGetDetails)
        {
            return await _crudOperation.GetSingleRecord<Allowances>(storedProcedureName: "[dbo].[uspAllowanceGetDetails]", allowancesGetDetails);
        }
        public async Task<Response> AllowancesUpdate(AllowancesUpdate allowancesUpdate)
        {
            return await _crudOperation.InsertUpdateDelete<Response>(storedProcedureName: "[dbo].[uspAllowanceUpdate]", allowancesUpdate);
        }
        public async Task<Response> AllowancesGetDelete(AllowancesGetDelete allowancesGetDelete)
        {
            return await _crudOperation.InsertUpdateDelete<Response>(storedProcedureName: "[dbo].[uspAllowanceDelete]", allowancesGetDelete);
        }
        public async Task<ResponseList<Allowances>> AllowancesGetList(string employeeId,  JqueryDataTable allowancesGetList)
        {
            return await _crudOperation.GetPaginatedList<Allowances>(storedProcedureName: "[dbo].[uspAllowanceList]",
                new
                {
                    employeeId,
                    allowancesGetList.SearchKey,
                    allowancesGetList.PageSize,
                    allowancesGetList.Start,
                    allowancesGetList.SortCol
                }).ConfigureAwait(false);
        }
    }
}
