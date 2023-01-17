using EmployeeGeneric.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Allowances
    {
        public string? AllowanceId { get; set; }
        public string? EmployeeId { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal HouseAllowance { get; set; }
        public decimal ConveyanceAllowance { get; set; }
        public decimal SpecialAllowance { get; set; }
        public decimal TotalAllowance { get; set; }
        [DenyHtmlInput]
        public string? MonthName { get; set; }
      
        public int Year { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsDeleted { get; set; }
    }
    public class AllowancesInsert
    {
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? EmployeeId { get; set; }
        [DenyHtmlInput]
        public decimal BasicSalary { get; set; }
        [DenyHtmlInput]
        public decimal HouseAllowance { get; set; }
        [DenyHtmlInput]
        public decimal ConveyanceAllowance { get; set; }
        [DenyHtmlInput]
        public decimal SpecialAllowance { get; set; }
        [DenyHtmlInput]
        public string? MonthName { get; set; }
        [DenyHtmlInput]
        public int Year { get; set; }
        [DenyHtmlInput]
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }

    }
    public class AllowancesUpdate
    {
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? AllowanceId { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? EmployeeId { get; set; }
        [DenyHtmlInput]
        public decimal BasicSalary { get; set; }
        [DenyHtmlInput]
        public decimal HouseAllowance { get; set; }
        [DenyHtmlInput]
        public decimal ConveyanceAllowance { get; set; }
        [DenyHtmlInput]
        public decimal SpecialAllowance { get; set; }
        [DenyHtmlInput]
        public string? MonthName { get; set; }
        [DenyHtmlInput]
        public int Year { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; }
    }
    public class AllowancesGetDetails
    {
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? AllowanceId { get; set; }
    }
    public class AllowancesGetDelete
    {
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? AllowanceId { get; set; }
        public string? DeletedBy { get; set; }
    }
}
