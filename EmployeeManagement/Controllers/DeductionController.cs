using Core.Comman;
using Core.Interface;
using Core.Model;
using EmployeeGeneric.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]"), Authorize, ActivityLog]
    [ApiController]
    public class DeductionController : Controller
    {


        private readonly IDeductionRepository _deductionRepository;
        public DeductionController(IDeductionRepository deductionRepository)
        {
            _deductionRepository = deductionRepository;
        }
        [HttpPost, Route("insert")]
        public async Task<IActionResult> DeductionInsert([FromBody] DeductionInsert deductionInsert)
        {
            try
            {
                var res = await _deductionRepository.DeductionInsert(deductionInsert);
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeductionGetDeatails([FromBody] DeductionGetDeatails deductionGetDeatails)
        {
            try
            {
                var res = await _deductionRepository.DeductionGetDeatails(deductionGetDeatails);
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        [HttpPost, Route("Update")]
        public async Task<IActionResult> DeductionUpdate([FromBody] DeductionUpdate deductionUpdate)
        {
            try
            {
                var res = await _deductionRepository.DeductionUpdate(deductionUpdate);
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeductionDelete([FromBody] DeductionDelete deductionDelete)
        {
            try
            {
                var res = await _deductionRepository.DeductionDelete(deductionDelete);
                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        [HttpGet("deductionList")]
        public async Task<IActionResult> DeductionList(string employeeId, [FromQuery] JqueryDataTable deductionList)
        {
            try
            {
                var res = await _deductionRepository.DeductionList(employeeId,deductionList);
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
