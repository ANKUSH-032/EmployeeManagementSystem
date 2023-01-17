using Core.Model;
using CrudOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Interface
{
    public interface IQRGaneraterRepository
    {
        Task<Response<EmployeeGetFormPDF>> QRCodeGenerator(EmployeeSalaryGetId EmployeeSalaryGetId);
    }
}
