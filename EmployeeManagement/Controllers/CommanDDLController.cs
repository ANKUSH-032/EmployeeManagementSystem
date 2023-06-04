using CORE.Comman;
using CORE.Model;
using EmployeeGeneric.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommanDDLController : ControllerBase
    {
        private readonly ICommanDDLRepository _commonFunctionsRepository;

        public CommanDDLController(ICommanDDLRepository commonFunctionsRepository)
        {
            _commonFunctionsRepository = commonFunctionsRepository;
        }

        [SwaggerOperation(
        Summary = "Get DDL List", Description = "This API will allow to get list of diffrenttypes provided in system.", OperationId = "")]
        [HttpGet, Route("DDLgetlist")]
        public async Task<IActionResult> DDLgetlist([FromQuery] CommanDDLFuntion commonInput)
        {
            try
            {
                var res = await _commonFunctionsRepository.DDLgetlist(commonInput);
                return StatusCode(StatusCodes.Status200OK, res);//: StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                Logger.AddErrorLog(ControllerContext.ActionDescriptor.ControllerName, "Provider Get List", User.Identity.Name, ex);
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, MessageHelper.message);
            }
        }
    }
}
