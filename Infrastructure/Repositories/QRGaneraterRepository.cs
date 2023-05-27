using Core.Model;
using CORE.Interface;
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
    public class QRGaneraterRepository : IQRGaneraterRepository
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
        public QRGaneraterRepository(ICrudOperationService crudOperation)
        {
            this._crudOperation = crudOperation;
        }
        public async Task<Response<EmployeeGetFormPDF>> QRCodeGenerator(EmployeeSalaryGetId EmployeeSalaryGetId)
        {
            return await _crudOperation.GetSingleRecord<EmployeeGetFormPDF>(storedProcedureName: "[dbo].[uspEmployeeSalaryGetDetails]", EmployeeSalaryGetId);
        }
    }
}
