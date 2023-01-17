using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class EmployeeSalary
    {
        public string? SalaryId { get; set; }
        public string? EmployeeId { get; set; }
        public decimal TotalAllowances { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal NetSalary { get; set; }
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
    public class EmployeeSalaryInsert
    {
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? EmployeeId { get; set; }
        public decimal TotalAllowances { get; set; }
        public decimal TotalDeduction { get; set; }
        public string? MonthName { get; set; }
        public int Year { get; set; }
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
    }
    public class EmployeeSalaryGetId
    {
        public string? EmployeeId { get; set; }
        public string? MonthName { get; set; }  
        public int Year { get; set; }
    }
    public class EmployeeGetFormPDF
    {
        //      public string? EmployeeId { get; set; }
        ////public string? FirstName { get; set; }
        //      public string? Name { get; set; }
        //      //public string? MiddleName { get; set; }
        //      //public string? LastName { get; set; }
        //      public DateTime? DayofBirth { get; set; }
        //      public string? EmailId { get; set; }
        //      //public Int64 District { get; set; }
        //      // public string? State { get; set; }
        //      //public Int64 PinCode { get; set; }
        //      //public string? Address { get; set; }
        //      public string? FullAddress { get; set; }
        //      public string? DesignationName { get; set; }
        //      public string? Gender { get; set; }
        //      public string? CompanyName { get; set; }
        //      public string? CompanyAddress { get; set; }
        //      public string? FatherName { get; set; }
        //      public Int64 PhoneNumber { get; set; }
        //     // public Int64 HomePhoneNumber { get; set; }
        public string? EmployeeId { get; set; }
        public string? Name { get; set; }
        public DateTime? DayofBirth { get; set; }
        public string? EmailId { get; set; }
        public string? FullAddress { get; set; }
        public string? DesignationName { get; set; }
        public string? Gender { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyAddress { get; set; }
        public string? FatherName { get; set; }
        public Int64 PhoneNumber { get; set; }
        public Int64 HomePhoneNumber { get; set; }
        public string? Qualification { get; set; }
        public string? CurrentExperience { get; set; }
        public string? JoinDate { get; set; }
        public string? MaritalStatus { get; set; }
        public string? FatherOccupation { get; set; }
        public string? MotherName { get; set; }
        public string? MotherOcupation { get; set; }
        public string? Photopath { get; set; }
        public string? SignaturePath { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal HouseAllowance { get; set; }
        public decimal ConveyanceAllowance { get; set; }
        public decimal SpecialAllowance { get; set; }
        public decimal TotalAllowance { get; set; }
        public decimal EmployeesProvidentFund { get; set; }
        public decimal ToxicologicalRiskAssessments { get; set; }
        public decimal HouseRentAllowance { get; set; }
        public decimal PF { get; set; }
        public decimal TaxDeductedAtSource { get; set; }
        public decimal TotalDeduction { get; set; }
       
        public string? MonthName { get; set; }
        public int Year { get; set; }
        public decimal NetSalary { get; set; }
        public string? QRCodePath { get; set; }

    }
}
