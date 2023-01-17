using Core.Model;
using CrudOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IEmployeeRepository
    {
        Task<Response> EmployeeInsert(EmployeeInsert employeeInsert);
        Task<Response> EmployeeUpdate(EmployeeUpdate employeeUpdate);
        Task<Response<Employee>> EmployeeGetDetails(EmployeeGetDetails employeeGetDetails);
        Task<Response> EmployeeDelete(EmployeeDelete employeeDelete);
        Task<ResponseList<Employee>> EmployeeList(Core.Comman.JqueryDataTable list);
        Task<Response<EmployeeGetFormPDF>> GetEmployeeFormPDF(string employeeId);

    }
}
