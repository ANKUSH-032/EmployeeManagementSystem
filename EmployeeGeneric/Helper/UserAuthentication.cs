using EmployeeGeneric.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeGeneric.Helper
{
    public class UserAuthentication
    {
    }
    public class AuthenticationRequest
    {
        [Required, DenyHtmlInput]
        [EmailInput]
        public string? Email { get; set; }

        [DenyHtmlInput]
        [Required, DataType(DataType.Password)]
        public string? Password { get; set; }
    }

    public class ForgotPassword
    {
        [Required]
        [EmailInput]
        public string? Email { get; set; }
    }

    public class ResetPassword
    {
        [Required, DataType(DataType.Password)]
        [PasswordInput]
        public string? Password { get; set; }
        [DataType(DataType.Password), Compare("Password")]
        public string? ConfirmPassword { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string? UpdatedBy { get; set; }
        [Required]
        public string? Email { get; set; }
        public string? EmailToken { get; set; }
        public string? OldPassword { get; set; }
    }

    public class ChangePassword
    {
        public string? UserId { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required, DataType(DataType.Password)]
        [PasswordInput]
        public string? CurrentPassword { get; set; }
        public string? Role { get; set; }
        [Required, DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d.*)(?=.*\W.*)[a-zA-Z0-9\S]{8,}$", ErrorMessage = "Password must be 8 Characters long and must contain at least one upper case character, one lowercase character, one number and one special character.")]
        public string? Password { get; set; }
        [DataType(DataType.Password), Compare("Password")]
        public string? ConfirmPassword { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string? UpdatedBy { get; set; }
        public string? Name { get; set; }
        public string? EmailToken { get; set; }
    }

    public class AuthenticationResponse
    {
        public string? UserID { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public string? Role { get; set; }
    }

    public class LoginUser
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public string? Role { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
    }
    public class CreatePassword
    {
        [Required, DataType(DataType.Password)]
        [PasswordInput]
        public string? Password { get; set; }
        [DataType(DataType.Password), Compare("Password")]
        public string? ConfirmPassword { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string? UpdatedBy { get; set; }
        [Required]
        public string? Email { get; set; }
        public string? EmailToken { get; set; }
    }
}
