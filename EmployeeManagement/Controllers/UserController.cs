

using Core.Comman;
using Core.Interface;
using Core.Interface.Helper;
using Core.Model;
using EmployeeGeneric.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepositroy _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IDataProtectionRepository _dataProtection;
       
        public UserController(IUserRepositroy userRepository, IConfiguration configuration, IDataProtectionRepository dataProtectionRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _dataProtection = dataProtectionRepository;
          
        }
        [HttpPost, Route("insert")]
        public async Task<IActionResult> UserInsert([FromBody] UserInsert userInsert)
        {
            try
            {
                string Password = Utility.CreateRandomPassword();
                 Utility.CreatePasswordHash(Password, out byte[] passwordHash, out byte[] passwordSalt);
                userInsert.PasswordHash = passwordHash;
                userInsert.PasswordSalt = passwordSalt;


                var res = await _userRepository.UserInsert(userInsert);

                //CommonFunctionsInput cfi = new();
                //List<UserPermission> ups = new();
                //UserPermission up = new();
                //cfi.type = "module";
                //dynamic respornse = _commonFunctionsRepository.DDLgetlist(cfi);
                //foreach (var item in respornse.Result)
                //{
                //    up = new UserPermission();
                //    up.ModuleId = Convert.ToInt64(item.Id);
                //    up.IsView = true;
                //    up.UpdatedBy = user.CreatedBy;
                //    up.UserId = res.Data;
                //    up.IsAdd = false;
                //    up.IsEdit = false;
                //    up.IsDelete = false;
                //    ups.Add(up);
                //}

                // await _userRepository.UserPermissionsSave(ups);

                //Utilities.SendCreatPasswordMail(user.Email, user.FirstName, Password);
                string otp = Utility.GenerateOtp();
                Authenticates.CreatePasswordCheckerAdd(userInsert.Email, otp);
                string link = _configuration.GetValue<string>("CreateUrl").
                 Replace("{{Email}}", _dataProtection.Encrypt(userInsert.Email), StringComparison.InvariantCulture).
                 Replace("{{token}}", _dataProtection.Encrypt(otp), StringComparison.InvariantCulture);
                Utility.SendCreatePassword(userInsert.Email, userInsert.FirstName, link);

                return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                Logger.AddErrorLog(ControllerContext.ActionDescriptor.ControllerName, "User Insert", User?.Identity?.Name, ex);
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR,MessageHelper.message);
            }
        }
        [SwaggerOperation(
       Summary = "User Update", Description = "This API will allow to update user into system.", OperationId = "")]
        [HttpPost, Route("update")]
        public async Task<IActionResult> UserUpdate([FromBody] UserUpdate userUpdate)
        {
            try
            {
                userUpdate.UpdatedBy = User?.Identity?.Name;
                var res = await _userRepository.UserUpdate(userUpdate);
                return res.Status ? StatusCode(StatusCodes.Status200OK, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                Logger.AddErrorLog(ControllerContext.ActionDescriptor.ControllerName, "User Update", User?.Identity?.Name, ex);
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, MessageHelper.message);
            }
        }
        [SwaggerOperation(
        Summary = "Get User List", Description = "This API will allow to get list of user in system.", OperationId = "")]
        [HttpPost, Route("usergetlist")]
        public async Task<IActionResult> UserGetList([FromBody] JqueryDataTable listingParams)
            {
            try
            {
                var res = await _userRepository.UserGetList(listingParams);
                if (res.Status && res.Data.Count == 0)
                {
                    res.RecordsFiltered = 0;
                    res.TotalRecords = 0;
                    return StatusCode(200, res);
                }
                return res.Status ? StatusCode(StatusCodes.Status200OK, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                Logger.AddErrorLog(ControllerContext.ActionDescriptor.ControllerName, "Provider Get List", User?.Identity?.Name, ex);
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, MessageHelper.message);
            }
        }

        //[SwaggerOperation(
        //Summary = "Get User Permissions List", Description = "This API will allow to get list of users Permissions in system.", OperationId = "")]
        //[HttpGet, Route("userpermissionsgetlist")]
        //public async Task<IActionResult> UserPermissionsGetList([FromQuery] GetUser userPermissionGetList)
        //{
        //    try
        //    {
        //        var res = await _userRepository.UserPermissionsGetList(userPermissionGetList);
        //        return res.Status ? StatusCode(StatusCodes.Status200OK, res) : StatusCode(StatusCodes.Status409Conflict, res);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddErrorLog(ControllerContext.ActionDescriptor.ControllerName, "Provider Get List", User.Identity.Name, ex);
        //        return StatusCode(CrudOperations.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, MessageHelper.message);
        //    }
        //}

        [SwaggerOperation(
        Summary = "Get User Details", Description = "This API will allow to get user info from system.", OperationId = "")]
        [HttpGet, Route("usergetdetails")]
        public async Task<IActionResult> UserGetDetails([FromQuery] GetUser userGetDetails)
        {
            try
            {
                var res = await _userRepository.UserGetDetails(userGetDetails);
                return res.Status ? StatusCode(StatusCodes.Status200OK, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                Logger.AddErrorLog(ControllerContext.ActionDescriptor.ControllerName, "Provider Get Details", User?.Identity?.Name, ex);
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, MessageHelper.message);
            }
        }

        [SwaggerOperation(
        Summary = "User Delete", Description = "This API will allow to delete user into system.", OperationId = "")]
        [HttpGet, Route("userdelete")]
        public async Task<IActionResult> UserDelete([FromQuery] DeleteUser userDelete)
        {
            try
            {
                var res = await _userRepository.UserDelete(userDelete);
                return res.Status ? StatusCode(StatusCodes.Status200OK, res) : StatusCode(StatusCodes.Status409Conflict, res);
            }
            catch (Exception ex)
            {
                Logger.AddErrorLog(ControllerContext.ActionDescriptor.ControllerName, "Provider Delete", User?.Identity?.Name, ex);
                return StatusCode(CrudOperation.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, MessageHelper.message);
            }
        }

        //[SwaggerOperation(
        //Summary = "Permissions Update", Description = "This API will allow to update user permissions into system.", OperationId = "")]
        //[HttpPost, Route("permissionssave")]
        //public async Task<IActionResult> UserPermissionsSave([FromBody] List<UserPermission> userPermissionsSave)
        //{
        //    try
        //    {

        //        var res = await _userRepository.UserPermissionsSave(userPermissionsSave);
        //        return res.Status ? StatusCode(StatusCodes.Status201Created, res) : StatusCode(StatusCodes.Status409Conflict, res);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.AddErrorLog(ControllerContext.ActionDescriptor.ControllerName, "Provider Insert", User.Identity.Name, ex);
        //        return StatusCode(CrudOperations.StatusCodes.HTTP_INTERNAL_SERVER_ERROR, MessageHelper.message);
        //    }
       // }
    }
}
