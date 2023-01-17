using Core.Model;
using CrudOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IEmployeeSalaryRepository
    {
        Task<Response> EmployeeSalaryInsert(EmployeeSalaryInsert employeeSalaryInsert);
        Task<Response<EmployeeGetFormPDF>> EmployeeSalaryGetId(EmployeeSalaryGetId employeeGetDetails);
        Task<Response<EmployeeGetFormPDF>> PDFGenerateSalary(EmployeeSalaryGetId employeeGetDetails);
        Task<Response<EmployeeGetFormPDF>> GetEmployeeSalaryPDF(EmployeeSalaryGetId employeeGetDetails);
    }
}
