using Core.Interface;
using Core.Model;
using EmployeeGeneric.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]"), Authorize, ActivityLog]
    [ApiController]
    public class EmployeeSalaryController : Controller
    {
        private readonly IEmployeeSalaryRepository _employeeSalaryRepository;
        public EmployeeSalaryController(IEmployeeSalaryRepository employeeSalaryRepository)
        {
            _employeeSalaryRepository = employeeSalaryRepository;
        }
        [HttpPost, Route("insert")]
        public async Task<IActionResult> EmployeeSalaryInsert([FromBody] EmployeeSalaryInsert employeeSalaryInsert)
        {
            try
            {
                var res = await _employeeSalaryRepository.EmployeeSalaryInsert(employeeSalaryInsert);
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> EmployeeSalaryGetId([FromBody] EmployeeSalaryGetId employeeSalaryGetId)
        {
            try
            {
                var res = await _employeeSalaryRepository.EmployeeSalaryGetId(employeeSalaryGetId);
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
