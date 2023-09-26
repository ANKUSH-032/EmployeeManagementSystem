using Core.Interface;
using Core.Model;
using CrudOperation;
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
    public  class EmployeeRepository : IEmployeeRepository
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
        public EmployeeRepository(ICrudOperationService crudOperation)
        {
            this._crudOperation = crudOperation;
        }

        public async Task<Response> EmployeeInsert(EmployeeInsert employeeInsert)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            employeeInsert.FirstName = textInfo.ToTitleCase(employeeInsert.FirstName);
          //  employeeInsert.MiddleName = textInfo.ToTitleCase(employeeInsert.MiddleName);
            employeeInsert.LastName = textInfo.ToTitleCase(employeeInsert.LastName);
            employeeInsert.CompanyName = textInfo.ToTitleCase(employeeInsert.CompanyName);
            employeeInsert.FatherName = textInfo.ToTitleCase(employeeInsert.FatherName);
            employeeInsert.MotherName = textInfo.ToTitleCase(employeeInsert.MotherName);

            return await _crudOperation.InsertUpdateDelete<Response>(storedProcedureName: "[dbo].[uspEmployeeInsert]", new
            {
                employeeInsert.FirstName,
                employeeInsert.MiddleName,
                employeeInsert.LastName,
                employeeInsert.Address,
                employeeInsert.DayofBirth,
                employeeInsert.Gender,
                employeeInsert.MaritalStatus,
                employeeInsert.Qualification,
                employeeInsert.State,
                employeeInsert.PhoneNumber,
                employeeInsert.PinCode,
                employeeInsert.HomePhoneNumber,
                employeeInsert.District,
                employeeInsert.EmailId,
                employeeInsert.CompanyName,
                employeeInsert.CompanyAddress,
                employeeInsert.FatherName,
                employeeInsert.FatherOccupation,
                MontherName = employeeInsert.MotherName,
                MontherOcupation =employeeInsert.MotherOcupation,
                employeeInsert.CurrentExperience,
                employeeInsert.JoinDate,
                employeeInsert.DesignationName,
                employeeInsert.SignaturePath,
                employeeInsert.Photopath,
                employeeInsert.CreatedBy,
                employeeInsert.IsDeleted,
            });
        }
        public async Task<Response> EmployeeUpdate(EmployeeUpdate employeeUpdate)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            employeeUpdate.CompanyName = textInfo.ToTitleCase(employeeUpdate.CompanyName);
            employeeUpdate.FatherName = textInfo.ToTitleCase(employeeUpdate.FatherName);
            employeeUpdate.FirstName = textInfo.ToTitleCase(employeeUpdate.FirstName);
            employeeUpdate.MiddleName = textInfo.ToTitleCase(employeeUpdate.MiddleName);
            employeeUpdate.LastName = textInfo.ToTitleCase(employeeUpdate.LastName);
            return await _crudOperation.InsertUpdateDelete<Response>(storedProcedureName: "[dbo].[uspEmployeeUpdate]", new
            {
                employeeUpdate.FirstName,
                employeeUpdate.MiddleName,
                employeeUpdate.LastName,
                employeeUpdate.Address,
                employeeUpdate.DayofBirth,
                employeeUpdate.Gender,
                employeeUpdate.MaritalStatus,
                employeeUpdate.Qualification,
                employeeUpdate.State,
                employeeUpdate.PhoneNumber,
                employeeUpdate.PinCode,
                employeeUpdate.HomePhoneNumber,
                employeeUpdate.District,
                employeeUpdate.EmailId,
                employeeUpdate.CompanyName,
                employeeUpdate.CompanyAddress,
                employeeUpdate.FatherName,
                employeeUpdate.FatherOccupation,
                employeeUpdate.MotherName,
                employeeUpdate.MotherOcupation,
                employeeUpdate.CurrentExperience,
                employeeUpdate.JoinDate,
                employeeUpdate.DesignationName,
                employeeUpdate.SignaturePath,
                employeeUpdate.PhotoPath,
                employeeUpdate.UpdatedBy,
                employeeUpdate.EmployeeID,
            });
        }
        public async Task<Response<Employee>> EmployeeGetDetails(EmployeeGetDetails employeeGetDetails)
        {
            return await _crudOperation.GetSingleRecord<Employee>(storedProcedureName: "[dbo].[uspEmployeeGetDetails]", new { employeeGetDetails.EmployeeID });
        }
        public async Task<Response> EmployeeDelete(EmployeeDelete employeeDelete)
        {
            return await _crudOperation.InsertUpdateDelete<Response>(storedProcedureName: "[dbo].[uspEmployeeDelete]", employeeDelete);
        }
        public async Task<ResponseList<Employee>> EmployeeList(Core.Comman.JqueryDataTable list)
        {
            return await _crudOperation.GetPaginatedList<Employee>(storedProcedureName: "[dbo].[uspEmployeeList]", list);
        }

        public async Task<Response<EmployeeGetFormPDF>> GetEmployeeFormPDF(string employeeId)
        {
            return await _crudOperation.GetSingleRecord<EmployeeGetFormPDF>("[dbo].[uspEmployeeFormGet]", new { employeeId });

        }
    }
}
