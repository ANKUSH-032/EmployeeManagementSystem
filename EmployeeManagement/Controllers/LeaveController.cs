using Core.Comman;
using Core.Interface;
using Core.Model;
using CORE.Interface;
using CORE.Model;
using CrudOperation;
using DocumentFormat.OpenXml.Wordprocessing;
using EmployeeGeneric.Helper;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using static EmployeeGeneric.Helper.Utility;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]"), Authorize, ActivityLog]
    [ApiController]
    public class LeaveController : Controller
    {
        private readonly IUserRepositroy _userRepository;
        private readonly ILeaveRepository _leaveRepository;
        private readonly IEmployeeRepository _employeeRepository;
        public LeaveController(ILeaveRepository leaveRepository, IEmployeeRepository employeeRepository, IUserRepositroy userRepository)
        {
            _leaveRepository = leaveRepository;
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
        }

        [HttpPost, Route("insert")]
        public async Task<IActionResult> LeaveInsert([FromBody] LeaveInsert leaveInsert)
            {
            try
            {
                EmployeeGetDetails employeeGetDetails = new()
                {
                    EmployeeID = leaveInsert.EmployeeId
                };
                //GetUser getUser = new()
                //{
                //    UserId = 
                //};
                var employeeDetails = await _employeeRepository.EmployeeGetDetails(employeeGetDetails).ConfigureAwait(false);
                var employeeData = employeeDetails.Data;
                //var userDetails = await _userRepository.UserGetDetails(userGetDetails);

                if (!employeeDetails.Status)
                {
                    return BadRequest(new
                    {
                        Status = false,
                        employeeDetails.Message
                    });
                }

                var res = await _leaveRepository.LeaveInsert(leaveInsert);
                if (res.Status)
                {
                    EmailData emailData = new()
                    {
                        EmailType = EmailType.LEAVEAPPLY,
                        User = new GetEmployeeFormPDF() { EmailId = employeeData!.EmailId, Name = employeeData!.FirstName + " " + employeeData!.LastName },
                        Reason = leaveInsert.Reason.ToString(),
                        FromDate = leaveInsert.FromDate,
                        ToDate = leaveInsert.ToDate,
                        JoinDate = employeeData!.JoinDate,  
                        Subject = "Apply the leave from" + employeeData!.FirstName + " " + employeeData!.LastName

                    };

                    await SendEmailAsync(emailData).ConfigureAwait(false);
                }
                return res.Status ? StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status201Created, res) : StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);

            }
        }

        [HttpGet]
        public async Task<IActionResult> LeaveGetDetails([FromBody] LeaveGetDetails leaveGetDetails)
        {
            try
            {
                var res = await _leaveRepository.LeaveGetDetails(leaveGetDetails);
                return res.Status ? StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status201Created, res) : StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        [HttpPost, Route("Update")]
        public async Task<IActionResult> LeaveUpdate([FromBody] LeaveUpdate leaveUpdate)
        {
            try
            {
                var res = await _leaveRepository.LeaveUpdate(leaveUpdate);
                return res.Status ? StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status201Created, res) : StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> LeaveDelete([FromBody] LeaveDelete leaveDelete)
        {
            try
            {
                var res = await _leaveRepository.LeaveDelete(leaveDelete);
                return res.Status ? StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status201Created, res) : StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> LeaveGetList(string employeeId, [FromQuery] JqueryDataTable leaveGetList)
        {
            try
            {
                var res = await _leaveRepository.LeaveGetList(employeeId, leaveGetList);
                if (res.Status && res.Data.Count == 0)
                {
                    res.RecordsFiltered = 0;
                    res.TotalRecords = 0;
                    return StatusCode(200, res);
                }
                return res.Status ? StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status201Created, res) : StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
