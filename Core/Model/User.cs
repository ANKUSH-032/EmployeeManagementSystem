using EmployeeGeneric.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class User
    {
        #region Entity Properties


        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        [NameInput]
        public string? FirstName { get; set; }


        [StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? MiddleName { get; set; }

        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        [NameInput]
        public string? LastName { get; set; }

        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? ContactNo { get; set; }

        [Required, StringLength(150, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        [EmailInput]
        public string? Email { get; set; }

        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? RoleID { get; set; }


        #endregion Entity Properties

        #region Helper Properties
        #endregion Helper Properties
    }
    public class UserInsert : User
    {
        public string? CreatedBy { get; set; }

        [DataType(DataType.Password)]
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }

    }

    public class UserUpdate : User
    {
        [StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? UserId { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class UserLogin : User
    {
        [StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? UserId { get; set; }
        public string? Token { get; set; }
    }
    public class UserGetDetails : User
    {
        [StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? UserId { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? RoleName { get; set; }
    }


    public class DeleteUser
    {
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? UserId { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? DeletedBy { get; set; }

    }
    public class GetUser
    {
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? UserId { get; set; }
    }

    public class UserPermission
    {
        public Int64 MappingId { get; set; }
        [Required]
        [NameInput]
        public string? ModuleName { get; set; }
        [Required]
        public string? UserId { get; set; }
        [Required]
        public Int64 ModuleId { get; set; }
        public bool IsView { get; set; }
        public bool IsAdd { get; set; }
        public bool IsEdit { get; set; }
        public string? UserName { get; set; }
        public bool IsDelete { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? UpdatedBy { get; set; }
    }
}
