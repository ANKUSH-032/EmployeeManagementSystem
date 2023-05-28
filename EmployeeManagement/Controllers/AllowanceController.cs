using Core.Comman;
using Core.Interface;
using Core.Model;
using EmployeeGeneric.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]"), Authorize,ActivityLog]
    [ApiController]
    public class AllowanceController : Controller
    {
        private readonly IAllowanceRepository _allowanceRepository;
        public AllowanceController(IAllowanceRepository allowanceRepository)
        {
            _allowanceRepository = allowanceRepository;
        }

        [HttpPost, Route("insert")]
        public async Task<IActionResult> AllowancesInsert([FromBody] AllowancesInsert allowancesInsert)
        {
            try
            {
                var res = await _allowanceRepository.AllowancesInsert(allowancesInsert);
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);

            }
        }

        [HttpGet("get")]
        public async Task<IActionResult> AllowancesGetDetails([FromBody] AllowancesGetDetails allowancesGetDetails)
        {
            try
            {
                var res = await _allowanceRepository.AllowancesGetDetails(allowancesGetDetails);
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        [HttpPost, Route("Update")]
        public async Task<IActionResult> AllowancesUpdate([FromBody] AllowancesUpdate allowancesUpdate)
        {
            try
            {
                var res = await _allowanceRepository.AllowancesUpdate(allowancesUpdate);
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> AllowancesGetDelete([FromBody] AllowancesGetDelete allowancesGetDelete)
        {
            try
            {
                var res = await _allowanceRepository.AllowancesGetDelete(allowancesGetDelete);
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        [HttpGet("allowancesGetList")]
        public async Task<IActionResult> AllowancesGetList(string? employeeId, [FromQuery] JqueryDataTable allowancesGetList)
        {
            try
            {

                var res = await _allowanceRepository.AllowancesGetList(

                    employeeId,
                    allowancesGetList
                //allowancesGetList.PageSize,
                //allowancesGetList.Start,
                //allowancesGetList.SortCol,
                //allowancesGetList.SearchKey
                ).ConfigureAwait(false);
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
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

    }
}
