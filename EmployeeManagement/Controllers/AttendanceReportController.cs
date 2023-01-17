using Core.Comman;
using Core.Interface;
using Core.Model;
using EmployeeGeneric.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers

{
    [Route("api/[Controller]"), Authorize, ActivityLog]
    [ApiController]
    public class AttendanceReportController : Controller
    {
        private readonly IAttendanceReportRepository _attendanceReportRepository;
        public AttendanceReportController(IAttendanceReportRepository employeeController)
        {
            _attendanceReportRepository = employeeController;
        }
        [HttpPost, Route("insert")]
        public async Task<IActionResult> LogInOrLogOutAttendenceReport([FromBody] LogInOrLogOutAttendenceReport logInOrLogOutAttendenceReport)
        {
            try
            {
                var res = await _attendanceReportRepository.LogInOrLogOutAttendenceReport(logInOrLogOutAttendenceReport);
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperations.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> AttendenceDelete([FromBody] AttendenceDelete attendenceDelete)
        {
            try
            {
                var res = await _attendanceReportRepository.AttendenceDelete(attendenceDelete);
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperations.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        [HttpGet("attendenceGetList")]
        public async Task<IActionResult> AttendenceGetList(string employeeId, [FromQuery] JqueryDataTable attendenceGetList)
        {
            try
            {
                var res = await _attendanceReportRepository.AttendenceGetList(employeeId, attendenceGetList).ConfigureAwait(false);
                if (res.Status && res.Data.Count == 0)
                {
                    res.RecordsFiltered = 0;
                    res.TotalRecords = 0;
                    return StatusCode(200, res);
                }
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                //Logger.AddErrorLog(ControllerContext.ActionDescriptor.ControllerName, "Clinic List", User.Identity.Name, ex);
                return StatusCode(CrudOperations.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }

        }
        [HttpGet]
        public async Task<IActionResult> AttendenceGetDetails([FromBody] AttendenceGetDetails attendenceGetDetails)
        {
            try
            {
                var res = await _attendanceReportRepository.AttendenceGetDetails(attendenceGetDetails);
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperations.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
