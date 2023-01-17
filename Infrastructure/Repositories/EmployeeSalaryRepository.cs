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
    public class EmployeeSalaryRepository : IEmployeeSalaryRepository
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
        public EmployeeSalaryRepository(ICrudOperationService crudOperation)
        {
            this._crudOperation = crudOperation;
        }
        public async Task<Response> EmployeeSalaryInsert(EmployeeSalaryInsert employeeSalaryInsert)
        {
            return await _crudOperation.InsertUpdateDelete<Response>(storedProcedureName: "[dbo].[uspSalaryInsert]", employeeSalaryInsert);
        }
        public async Task<Response<EmployeeGetFormPDF>> EmployeeSalaryGetId(EmployeeSalaryGetId employeeGetDetails)
        {
            return await _crudOperation.GetSingleRecord<EmployeeGetFormPDF>(storedProcedureName: "[dbo].[uspEmployeeSalaryGetDetails]", employeeGetDetails);
        }

        public async Task<Response<EmployeeGetFormPDF>> PDFGenerateSalary(EmployeeSalaryGetId employeeGetDetails)
        {
            return await _crudOperation.GetSingleRecord<EmployeeGetFormPDF>(storedProcedureName: "[dbo].[uspEmployeeSalaryGetDetails]",employeeGetDetails);
            
        }


        public async Task<Response<EmployeeGetFormPDF>> GetEmployeeSalaryPDF(EmployeeSalaryGetId employeeGetDetails)
        {
            return await _crudOperation.GetSingleRecord<EmployeeGetFormPDF>("[dbo].[uspEmployeeSalaryGetDetails]", employeeGetDetails);

        }

    }
}
