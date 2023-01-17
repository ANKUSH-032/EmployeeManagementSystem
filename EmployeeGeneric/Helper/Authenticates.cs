using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static EmployeeGeneric.Helper.Utility;

namespace EmployeeGeneric.Helper
{
    public class Authenticates
    {
        private static readonly IConfigurationRoot _iconfiguration;
        private static readonly string? _con = string.Empty;
        private static readonly string? _secretKey = string.Empty;

        static Authenticates()
        {

            var builder = new ConfigurationBuilder()
                 .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                 .AddJsonFile("appsettings.json");
            _iconfiguration = builder.Build();

            _con = _iconfiguration["ConnectionStrings:DataAccessConnection"];
            _secretKey = _iconfiguration["AppSettings:Secret"];
        }

        public static IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_con);
            }
        }

        public static dynamic Login<T>(AuthenticationRequest loginCredentials)
        {
            return Authentication<T>(loginCredentials);
        }

        private static LoginUser Authentication<T>(AuthenticationRequest loginCredentials)
        {
            LoginUser loginUserDetails = new();

            dynamic response = GetUserDetails<T>(Email: loginCredentials.Email);

            if (response == null)
                return null;
            else if (response.Name.ToUpper().Equals("USERNOTREGISTER") || response.Name.ToUpper().Equals("DELETED"))
                return response;
            else
            {
                //var emailId = Convert.ToString(typeof(T).GetProperty("Email").GetValue(response, null));
                //byte[] PasswordHash = (byte[])typeof(T).GetProperty("PasswordHash").GetValue(response, null);
                //byte[] PasswordSalt = (byte[])typeof(T).GetProperty("PasswordSalt").GetValue(response, null);

                //var userId = Convert.ToString(typeof(T).GetProperty("UserID").GetValue(response, null));
                //var role = Convert.ToString(typeof(T).GetProperty("Role").GetValue(response, null));

                if (!Utility.VerifyPasswordHash(loginCredentials.Password, response.PasswordHash, response.PasswordSalt))
                    return null;

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                          new Claim(ClaimTypes.Name , response.UserId ), new Claim(ClaimTypes.Role, response.Role), new Claim(ClaimTypes.Email, response.Email)
                    }
                    ),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                loginUserDetails.UserId = response.UserId;
                loginUserDetails.Email = response.Email;
                loginUserDetails.Name = response.Name;
                loginUserDetails.Token = tokenHandler.WriteToken(token);
                loginUserDetails.Role = response.Role;

                return loginUserDetails;
            }
        }

        public static dynamic GetUserDetails<T>(string? UserId = null, string? Email = null)
        {
            dynamic? response;

            using (IDbConnection db = Connection)
            {
                response = db.QueryFirstOrDefault<T>("[dbo].[uspUserDetailsGet]", new
                {
                    UserId,
                    Email
                }, commandType: CommandType.StoredProcedure);
            }

            return response;
        }

        public static string ForgotPasswordCheckerAdd(string Email, string EmailToken)
        {
            string? response = string.Empty;
            using (IDbConnection dbContext = Connection)
            {
                response = dbContext.Query<string>("[dbo].[uspUserForgetValidatorAdd]", new
                {
                    Email,
                    EmailToken
                }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }

            return response;

        }

        public static string CreatePasswordCheckerAdd(string Email, string EmailToken)
        {
            string? response = string.Empty;
            using (IDbConnection dbContext = Connection)
            {
                response = dbContext.Query<string>("[dbo].[uspUserCreatePassValidatorAdd]", new
                {
                    Email,
                    EmailToken
                }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }

            return response;

        }

        public static async Task<dynamic> ForgotPassword<T>(string Email, string link, string otp, bool sendEmail)
        {
            using IDbConnection db = Connection;

            dynamic response = db.QueryFirstOrDefault<string>("[dbo].[uspCheckValidEmail]", new { Email }, commandType: CommandType.StoredProcedure);
            if (response != "NVU")
            {
                if (sendEmail)
                {
                    EmailData emailData = new()
                    {
                        EmailType = EmailType.FORGOTPASSWORD,
                        User = new { Email, Name = response },
                        Subject = AuthMessage.resetPasswordRequest,
                        ResetPwdLink = link,
                        Otp = otp
                    };

                    await SendEmailAsync(emailData);

                    response = "EmailSent";
                }
            }

            return response;

        }

        public static string ForgotPasswordChecker(string Email, string EmailToken)
        {
            string? response;
            using IDbConnection dbContext = Connection;
            
                response = dbContext.Query<string>("[dbo].[uspUserForgetValidatorChecker]", new
                {
                    Email,
                    EmailToken
                }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                return response;
            
        }

        public static string CreatePasswordChecker(string Email, string EmailToken)
        {
            string? response;
            using IDbConnection dbContext = Connection;

            response = dbContext.Query<string>("[dbo].[uspUsertblCreatePassValidatorChecker]", new
            {
                Email,
                EmailToken
            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            return response;
        }

        public static dynamic ResetPassword<T>(ResetPassword resetPassword)
        {
            dynamic? response = string.Empty;

            if (!string.IsNullOrEmpty(resetPassword.Email) && !string.IsNullOrEmpty(resetPassword.EmailToken))
            {
                AuthenticationRequest authenticationRequestNew = new()
                {
                    Email = resetPassword.Email,
                    Password = resetPassword.Password
                };

                dynamic validatePassword = Authentication<T>(authenticationRequestNew);
                if (validatePassword != null)
                    return "samepass";

                string checkResult = ForgotPasswordChecker(resetPassword.Email, resetPassword.EmailToken);
                if (!string.IsNullOrEmpty(checkResult))
                {
                    if (checkResult != "valid")
                    {
                        response = checkResult;
                        return response;
                    }
                    else
                    {
                        dynamic user = GetUserDetails<T>(Email: resetPassword.Email);
                        if (user != null)
                        {
                            if (!string.IsNullOrWhiteSpace(user.Name) && user.Name.ToUpper(CultureInfo.CurrentCulture).Equals("USERNOTREGISTER") || user.Name.ToUpper(CultureInfo.CurrentCulture).Equals("DELETED"))
                            {
                                response = user.Name;
                                return response;
                            }
                            else
                            {

                                CreatePasswordHash(resetPassword.Password, out byte[] passwordHash, out byte[] passwordSalt);
                                resetPassword.PasswordHash = passwordHash;
                                resetPassword.PasswordSalt = passwordSalt;

                                using IDbConnection dbContext = Connection;

                                response = dbContext.Query<string>("[dbo].[uspUserResetPassword]", new
                                {
                                    resetPassword.Email,
                                    resetPassword.PasswordHash,
                                    resetPassword.PasswordSalt
                                }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                                return response;

                            }
                        }
                    }
                }
            }

            return response;
        }

        public static dynamic ChangePassword<T>(string email, string currentpassword, string newPassword)
        {
            using IDbConnection db = Connection;

            AuthenticationRequest authenticationRequest = new()
            {
                Email = email,
                Password = currentpassword
            };

            dynamic? response = Authentication<T>(authenticationRequest);
            if (response == null)
            {
                return AuthMessage.currentPasswordDoesNotMatch;
            }
            if (response.Name.ToUpper(CultureInfo.CurrentCulture).Equals("USERNOTREGISTER"))
            {
                return AuthMessage.userNotRegister;
            }

            AuthenticationRequest authenticationRequestNew = new()
            {
                Email = email,
                Password = newPassword
            };

            dynamic validatePassword = Authentication<T>(authenticationRequestNew);
            if (validatePassword != null)
                return AuthMessage.newPasswordCanNotBeSameAsCurrentPassword; 
            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
            var PasswordHash = passwordHash;
            var PasswordSalt = passwordSalt;

            response = db.Query<dynamic>("[dbo].[uspUserResetPassword]", new
            {
                email,
                PasswordHash,
                PasswordSalt,
            }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            return response;

        }
        public static async Task<dynamic> CreatePassword<T>(string Email, string link, string otp, bool sendEmail)
        {
            using IDbConnection db = Connection;

            dynamic response = db.QueryFirstOrDefault<string>("[dbo].[uspUserCreatePassword]", new { Email }, commandType: CommandType.StoredProcedure);
            if (response != "NVU")
            {
                if (sendEmail)
                {
                    EmailData emailData = new()
                    {
                        EmailType = EmailType.CREATEPASSWORD,
                        User = new { Email, Name = response },
                        Subject = AuthMessage.createPasswordRequest,
                        ResetPwdLink = link,
                        Otp = otp
                    };

                    await SendEmailAsync(emailData);

                    response = "EmailSent";
                }
            }
            return response;
        }

        public static dynamic CreatePassword<T>(CreatePassword createPassword)
        {
            dynamic? response = string.Empty;

            if (!string.IsNullOrEmpty(createPassword.Email) && !string.IsNullOrEmpty(createPassword.EmailToken))
            {
                string checkResult = CreatePasswordChecker(createPassword.Email, createPassword.EmailToken);
                if (!string.IsNullOrEmpty(checkResult))
                {
                    if (checkResult != "valid")
                    {
                        response = checkResult;
                        return response;
                    }
                    else
                    {
                        dynamic user = GetUserDetails<T>(Email: createPassword.Email);
                        if (user != null)
                        {
                            if (!string.IsNullOrWhiteSpace(user.Name) && user.Name.ToUpper(CultureInfo.CurrentCulture).Equals("USERNOTREGISTER") || user.Name.ToUpper(CultureInfo.CurrentCulture).Equals("DELETED"))
                            {
                                response = user.Name;
                                return response;
                            }
                            else
                            {

                                CreatePasswordHash(createPassword.Password, out byte[] passwordHash, out byte[] passwordSalt);
                                createPassword.PasswordHash = passwordHash;
                                createPassword.PasswordSalt = passwordSalt;

                                using IDbConnection dbContext = Connection;

                                response = dbContext.Query<string>("[dbo].[uspUserCreatePassword]", new
                                {
                                    createPassword.Email,
                                    createPassword.PasswordHash,
                                    createPassword.PasswordSalt
                                }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                                return response;

                            }
                        }
                    }
                }
            }

            return response;
        }
    }
}
