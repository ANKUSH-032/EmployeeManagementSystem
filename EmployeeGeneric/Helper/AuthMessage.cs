using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeGeneric.Helper
{
    public class AuthMessage
    {
        public const string tokenRequiredLoggingOut = "Token is required for logging out";
        public const string tokenIsNotWellFormed = "Token is not well formed.";
        public const string tokenIsInvalid = "Token is invalid.";
        public const string userSuccessfullyLoggedOut = "User successfully logged out.";
        public const string newPasswordCanNotBeSameAsCurrentPassword = "New Password cannot be same as current password.";
        public const string invalidDetails = "Invalid details.";
        public const string linkIsExpired = "Link is expired, Please genrate the new link.";
        public const string passwordIsSuccessfullyReset = "Password is successfully reset.";
        public const string passwordUpdatedSuccessfully = "Password updated successfully !";
        public const string passwordIsSuccessfullyCreate = "Password is successfully create.";
        public const string resetPasswordRequest = "Reset Password request";
        public const string currentPasswordDoesNotMatch = "Current Password does not match..";
        public const string userNotRegister = "User not register";
        public const string createPasswordRequest ="Create Password request";
    }
}
