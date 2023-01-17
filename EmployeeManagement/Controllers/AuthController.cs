


using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using System.Globalization;

using StatusCodes = CrudOperations.StatusCodes;
using Core.Interface.Helper;
using EmployeeGeneric.Helper;
using Core.Comman;
using System.IdentityModel.Tokens.Jwt;
using Core.Model;
using Swashbuckle.AspNetCore.Annotations;

namespace EmployeeManagement.Controllers
{
    [ActivityLog]
    [ Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IDataProtectionRepository _dataProtection;

        public AuthController(IConfiguration configuration, IDataProtectionRepository dataProtectionRepository)
        {
            _configuration = configuration;
            _dataProtection = dataProtectionRepository;
        }

        
        [AllowAnonymous, HttpPost("authenticate")]
        public IActionResult Login([FromBody] AuthenticationRequest loginCredentials)
        {
            try
            {
                
                loginCredentials.Email = loginCredentials.Email.ToLower();
                LoginUser user = Authenticates.Login<LoginUser>(loginCredentials);

                if (user == null)
                {
                    ClsResponse rep = new()
                    {

                        Message = MessageHelper.invalidCredentials,
                        Data = null
                    };
                    return StatusCode(StatusCodes.HTTP_UNAUTHORIZED, rep);

                }

                else
                {
                    ClsResponse rep = new();
                    if (user.Name.ToUpper(CultureInfo.CurrentCulture).Equals("DELETED"))
                    {
                        rep.Message = MessageHelper.userNotFoundForEmail;
                        rep.Data = null;
                        return StatusCode(StatusCodes.HTTP_FORBIDDEN, rep);
                    }


                    if (!string.IsNullOrWhiteSpace(user.Name) && user.Name.ToUpper(CultureInfo.CurrentCulture).Equals("USERNOTREGISTER"))
                    {
                        rep.Message = MessageHelper.userNotRegister;
                        rep.Data = null;
                        return StatusCode(StatusCodes.HTTP_FORBIDDEN, rep);
                    }

                }

                return Ok(new { Status = true, Message = "Success", Userdetails = user });
            }
            catch (Exception ex)
            {
                Logger.AddErrorLog(ControllerContext.ActionDescriptor.ControllerName, "Login", User.Identity?.Name, ex);
                return StatusCode(StatusCodes.HTTP_INTERNAL_SERVER_ERROR, MessageHelper.message);
            }
        }




        [HttpGet, Route("logout")]
        public IActionResult Logout()
        {
            try
            {
                var tokenstring = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

                ClsResponse rep = new()
                {
                    Data = null
                };
                if (string.IsNullOrWhiteSpace(tokenstring))
                {
                    rep.Message = AuthMessage.tokenRequiredLoggingOut;
                    return StatusCode(StatusCodes.HTTP_UNAUTHORIZED, rep);
                }

                var handler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken;
                try
                {
                    jwtToken = handler.ReadToken(tokenstring.Split(" ")[1]) as JwtSecurityToken;
                }
                catch (Exception)
                {
                    rep.Message = AuthMessage.tokenIsNotWellFormed;
                    return StatusCode(StatusCodes.HTTP_UNAUTHORIZED, rep);
                }

                if (jwtToken?.Claims?.FirstOrDefault() == null || string.IsNullOrWhiteSpace(jwtToken.Claims.FirstOrDefault()?.Value))
                {
                    rep.Message = AuthMessage.tokenIsInvalid;
                    return StatusCode(StatusCodes.HTTP_UNAUTHORIZED, rep);
                }

                string CurrentLoggedInUserEmail = jwtToken.Claims.Skip(2).FirstOrDefault().Value;

                return Ok(new
                {
                    Message = string.Format(CultureInfo.CurrentCulture, AuthMessage.userSuccessfullyLoggedOut),
                    Status = true
                });
            }
            catch (Exception ex)
            {
                //Logger.AddErrorLog(ControllerContext.ActionDescriptor.ControllerName, "Logout", User.Identity.Name, ex);
                return StatusCode(StatusCodes.HTTP_INTERNAL_SERVER_ERROR, MessageHelper.message);
            }
        }


        [SwaggerOperation(
        Summary = "Forgot Password",
        Description = "This API will send a reset password link in case user forgot his password and user intend to reset his password.",
        OperationId = "")]
        [AllowAnonymous, HttpPost, Route("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword forgotPassword)
        {
            try
            {
                string otp = Utility.GenerateOtp();
                string response = Authenticates.ForgotPasswordCheckerAdd(forgotPassword.Email, otp);
                if (response == "False") //not valid user
                {
                    ClsResponse rep = new()
                    {
                        Status = false,
                        Data = "",
                        Message = MessageHelper.userNotFoundForEmail
                    };
                    return StatusCode(StatusCodes.HTTP_NOT_FOUND, rep);
                }
                string link = _configuration.GetValue<string>("ForgetUrl").
                    Replace("{{Email}}", _dataProtection.Encrypt(forgotPassword.Email), StringComparison.InvariantCulture).
                    Replace("{{token}}", _dataProtection.Encrypt(otp), StringComparison.InvariantCulture);

                dynamic responses = await Authenticates.ForgotPassword<ClsResponse<User>>(forgotPassword.Email, link, otp, true);
                return Ok(new
                {
                    Status = true,
                    Message = MessageHelper.resetPasswordlin
                });
            }
            catch (Exception ex)
            {
                Logger.AddErrorLog(ControllerContext.ActionDescriptor.ControllerName, "ForgotPassword", User.Identity.Name, ex);
                return StatusCode(StatusCodes.HTTP_INTERNAL_SERVER_ERROR, MessageHelper.message);
            }
        }


        [SwaggerOperation(
        Summary = "Forgot Password",
        Description = "This API will reset the password for user.",
        OperationId = "")]
        [AllowAnonymous, HttpPost, Route("resetpassword")]
        public IActionResult ResetPassword([FromBody] ResetPassword resetPassword)
        {
            try
            {
                resetPassword.Email = _dataProtection.Decrypt(resetPassword.Email);
                resetPassword.EmailToken = !string.IsNullOrEmpty(resetPassword.EmailToken) ? _dataProtection.Decrypt(resetPassword.EmailToken) : "";
                resetPassword.UpdatedBy = User.Identity.Name;

                ClsResponse rep = new()
                {
                    Status = false,
                    Data = ""
                };
                dynamic checkResponse = Authenticates.ResetPassword<dynamic>(resetPassword);

                if (checkResponse == "samepass")
                {
                    rep.Message = AuthMessage.newPasswordCanNotBeSameAsCurrentPassword;
                    return StatusCode(StatusCodes.HTTP_NOT_FOUND, rep);
                }
                if (checkResponse == "invaliddetails")
                {
                    rep.Message = AuthMessage.invalidDetails;
                    return StatusCode(StatusCodes.HTTP_NOT_FOUND, rep);
                }
                else if (checkResponse == "tokenexpired")
                {
                    //return StatusCode(StatusCodes.HTTP_INTERNAL_SERVER_ERROR, "Link is expired, Please genrate the new link.");
                    rep.Message = AuthMessage.linkIsExpired;
                    return StatusCode(StatusCodes.HTTP_INTERNAL_SERVER_ERROR, rep);
                }
                else if (checkResponse.ToUpper(CultureInfo.CurrentCulture).Equals("DELETED"))
                {
                    //return StatusCode(StatusCodes.HTTP_NOT_FOUND, MessageHelper.UserDeleted);
                    rep.Message = MessageHelper.userDeleted;
                    return StatusCode(StatusCodes.HTTP_NOT_FOUND, rep);
                }
                else if (!string.IsNullOrWhiteSpace(checkResponse) && checkResponse.ToUpper(CultureInfo.CurrentCulture).Equals("USERNOTREGISTER"))
                {
                    //return StatusCode(StatusCodes.HTTP_NOT_FOUND, MessageHelper.UserNotRegister);
                    rep.Message = MessageHelper.userNotRegister;
                    return StatusCode(StatusCodes.HTTP_NOT_FOUND, rep);
                }
                if (checkResponse == "UNF")
                {
                    rep.Message = MessageHelper.userNotRegister;
                    return StatusCode(StatusCodes.HTTP_NOT_FOUND, rep);
                }
                if (checkResponse == "DELETED")
                {
                    rep.Message = MessageHelper.userDeleted;
                    return StatusCode(StatusCodes.HTTP_NOT_FOUND, rep);
                }
                ClsResponse response = new()
                {
                    Status = true,
                    Message = string.Format(CultureInfo.CurrentCulture,AuthMessage.passwordIsSuccessfullyReset)
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.AddErrorLog(ControllerContext.ActionDescriptor.ControllerName, "ResetPassword", User.Identity.Name, ex);
                return StatusCode(StatusCodes.HTTP_INTERNAL_SERVER_ERROR, MessageHelper.message);
            }
        }


        [SwaggerOperation(
        Summary = "Change | Password",
        Description = "This API will allow user to update password into system",
        OperationId = "")]
        [Authorize,HttpPost, Route("changepassword")]
        public IActionResult ChangePassword([FromBody] ChangePassword changePassword)
        {
            try
            {
                dynamic response = Authenticates.ChangePassword<ChangePassword>(changePassword.Email, changePassword.CurrentPassword, changePassword.Password);

                try
                {
                    if (response.Status)
                    {
                        ClsResponse responseCls = new()
                        {
                            Status = response.Status,
                            Message = string.Format(CultureInfo.CurrentCulture, AuthMessage.passwordUpdatedSuccessfully)
                        };
                        return Ok(responseCls);
                    }
                }
                catch
                {
                    return BadRequest(new
                    {
                        Status = false,
                        Message = response
                    });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.AddErrorLog(ControllerContext.ActionDescriptor.ControllerName, "ChangePassword", User.Identity.Name, ex);
                return StatusCode(500, new { Status = false, Message = String.Concat(ex.Message, MessageHelper.genericException) });
            }
        }

        [SwaggerOperation(
        Summary = "Create Password",
        Description = "This API will Create the password for user.",
        OperationId = "")]
        [AllowAnonymous, HttpPost, Route("createpassword")]
        public IActionResult CreatePassword([FromBody] CreatePassword createPassword)
        {
            try
            {
                createPassword.Email = _dataProtection.Decrypt(createPassword.Email);
                createPassword.EmailToken = !string.IsNullOrEmpty(createPassword.EmailToken) ? _dataProtection.Decrypt(createPassword.EmailToken) : "";
                createPassword.UpdatedBy = User.Identity.Name;

                ClsResponse rep = new()
                {
                    Status = false,
                    Data = ""
                };
                dynamic checkResponse = Authenticates.CreatePassword<dynamic>(createPassword);

                if (checkResponse == "samepass")
                {
                    rep.Message = AuthMessage.newPasswordCanNotBeSameAsCurrentPassword;
                    return StatusCode(StatusCodes.HTTP_NOT_FOUND, rep);
                }
                if (checkResponse == "invaliddetails")
                {
                    rep.Message = "Invalid details.";
                    return StatusCode(StatusCodes.HTTP_NOT_FOUND, rep);
                }
                else if (checkResponse == "tokenexpired")
                {
                    //return StatusCode(StatusCodes.HTTP_INTERNAL_SERVER_ERROR, "Link is expired, Please genrate the new link.");
                    rep.Message = AuthMessage.linkIsExpired;
                    return StatusCode(StatusCodes.HTTP_INTERNAL_SERVER_ERROR, rep);
                }
                else if (checkResponse.ToUpper(CultureInfo.CurrentCulture).Equals("DELETED"))
                {
                    //return StatusCode(StatusCodes.HTTP_NOT_FOUND, MessageHelper.UserDeleted);
                    rep.Message = MessageHelper.userDeleted;
                    return StatusCode(StatusCodes.HTTP_NOT_FOUND, rep);
                }
                else if (!string.IsNullOrWhiteSpace(checkResponse) && checkResponse.ToUpper(CultureInfo.CurrentCulture).Equals("USERNOTREGISTER"))
                {
                    //return StatusCode(StatusCodes.HTTP_NOT_FOUND, MessageHelper.UserNotRegister);
                    rep.Message = MessageHelper.userNotRegister;
                    return StatusCode(StatusCodes.HTTP_NOT_FOUND, rep);
                }
                if (checkResponse == "UNF")
                {
                    rep.Message = MessageHelper.userNotRegister;
                    return StatusCode(StatusCodes.HTTP_NOT_FOUND, rep);
                }
                if (checkResponse == "DELETED")
                {
                    rep.Message = MessageHelper.userDeleted;
                    return StatusCode(StatusCodes.HTTP_NOT_FOUND, rep);
                }
                ClsResponse response = new()
                {
                    Status = true,
                    Message = string.Format(CultureInfo.CurrentCulture, AuthMessage.passwordIsSuccessfullyCreate)
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.AddErrorLog(ControllerContext.ActionDescriptor.ControllerName, "CreatePassword", User.Identity.Name, ex);
                return StatusCode(StatusCodes.HTTP_INTERNAL_SERVER_ERROR, MessageHelper.message);
            }
        }
    }
}
