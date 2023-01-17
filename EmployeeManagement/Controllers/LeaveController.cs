using Core.Comman;
using CORE.Interface;
using CORE.Model;
using EmployeeGeneric.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]"), Authorize, ActivityLog]
    [ApiController]
    public class LeaveController : Controller
    {
        private readonly ILeaveRepository _leaveRepository;
        public LeaveController(ILeaveRepository leaveRepository)
        {
            _leaveRepository = leaveRepository;
        }

        [HttpPost, Route("insert")]
        public async Task<IActionResult> LeaveInsert([FromBody] LeaveInsert leaveInsert)
        {
            try
            {
                var res = await _leaveRepository.LeaveInsert(leaveInsert);
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperations.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);

            }
        }

        [HttpGet]
        public async Task<IActionResult> LeaveGetDetails([FromBody] LeaveGetDetails leaveGetDetails)
        {
            try
            {
                var res = await _leaveRepository.LeaveGetDetails(leaveGetDetails);
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperations.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        [HttpPost, Route("Update")]
        public async Task<IActionResult> LeaveUpdate([FromBody] LeaveUpdate leaveUpdate)
        {
            try
            {
                var res = await _leaveRepository.LeaveUpdate(leaveUpdate);
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperations.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> LeaveDelete([FromBody] LeaveDelete leaveDelete)
        {
            try
            {
                var res = await _leaveRepository.LeaveDelete(leaveDelete);
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperations.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> LeaveGetList(string employeeId, [FromQuery] JqueryDataTable leaveGetList)
        {
            try
            {
                var res = await _leaveRepository.LeaveGetList(employeeId,leaveGetList);
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
                return StatusCode(CrudOperations.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
