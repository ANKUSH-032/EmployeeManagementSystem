using EmployeeGeneric.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Deduction
    {
        public string? DeductionId { get; set; }
        public string? EmployeeId { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal EPF { get; set; }
        public decimal TRA { get; set; }
        public decimal HRA { get; set; }
        public decimal ProfessionalTax { get; set; }
        public decimal TDS { get; set; }
        public decimal TotalDeduction { get; set; }
        public string? MonthName { get; set; }
        public int Year { get; set; }
        
        public string? CreatedBy { get; set; }
        public string? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UpdatedOn { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool? IsDeleted { get; set; }
    }
    public class DeductionInsert
    {
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? EmployeeId { get; set; }
        [DenyHtmlInput]

        public decimal EPF { get; set; }
        [DenyHtmlInput]
        public decimal TRA { get; set; }
        [DenyHtmlInput]
        public decimal HRA { get; set; }
        [DenyHtmlInput]
        public decimal ProfessionalTax { get; set; }
        [DenyHtmlInput]
        public decimal GrossSalary { get; set; }
        [DenyHtmlInput]
        public string? MonthName { get; set; }
        [DenyHtmlInput]
        public int Year { get; set; }
        [DenyHtmlInput]
        public decimal TDS { get; set; }
        public bool? IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
    }
    public class DeductionUpdate 
    {
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? EmployeeId { get; set; }
        [DenyHtmlInput]

        public decimal EPF { get; set; }
        [DenyHtmlInput]
        public decimal TRA { get; set; }
        [DenyHtmlInput]
        public decimal HRA { get; set; }
        [DenyHtmlInput]
        public decimal GrossSalary { get; set; }
        [DenyHtmlInput]
        public string? MonthName { get; set; }
        [DenyHtmlInput]
        public int Year { get; set; }
        [DenyHtmlInput]
        public decimal ProfessionalTax { get; set; }
        [DenyHtmlInput]

        public decimal TDS { get; set; }
        public bool? IsDeleted { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? DeductionId { get; set; }
        [DenyHtmlInput]
        public string? UpdatedBy { get; set; }
    }
    public class DeductionGetDeatails
    {
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? DeductionId { get; set; }
    }
    public class DeductionDelete
    {
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? DeductionId { get; set; }
        public string? DeletedBy { get; set; }
    }
}
