using EmployeeGeneric.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmployeeGeneric.Helper.ValidationFilter;

namespace Core.Model
{
    public class Employee
    {
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        [NameInput]
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        [NameInput]
        public string? LastName { get; set; }
        public DateTime DayofBirth { get; set; }
        [Required, StringLength(150, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        [EmailInput]
        public string? EmailId { get; set; }
        public Int64 PhoneNumber { get; set; }
        public Int64 HomePhoneNumber { get; set; }
        public Int64 District { get; set; }
        public Int64 State { get; set; }
        public Int64 PinCode { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? Address { get; set; }
        public Int64 Qualification { get; set; }
        public Int64? CurrentExperience { get; set; }
        public DateTime JoinDate { get; set; }
        public Int64 DesignationName { get; set; }
        public string? Gender { get; set; }
        public string? MaritalStatus { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? CompanyName { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? CompanyAddress { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? FatherName { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? FatherOccupation { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? MotherName { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? MotherOcupation { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class EmployeeInsert : Employee
    {
        public string? Photopath { get; set; }
        [MaxFileSize(MaxSize = (int)(2 * 1024 * 1024))]
        [ValidFileType(Extensions = new string[] { ".JPEG", ".jpg",  ".png",  })]
        public IFormFile? AttachmentPhoto { get; set; }
        public string? SignaturePath { get; set; }
        [MaxFileSize(MaxSize = (int)(2 * 1024 * 1024))]
        [ValidFileType(Extensions = new string[] { ".JPEG", ".jpg", ".png", })]
        public IFormFile? AttachmentSignature { get; set; }
        public string? CreatedBy { get; set; }

       // public string? QRCodePath { get; set; }


    }
    public class EmployeeUpdate
    {
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        [NameInput]
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        [NameInput]
        public string? LastName { get; set; }
        public DateTime DayofBirth { get; set; }
        [Required, StringLength(150, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        [EmailInput]
        public string? EmailId { get; set; }
        public Int64 PhoneNumber { get; set; }
        public Int64 HomePhoneNumber { get; set; }
        public Int64 District { get; set; }
        public Int64 State { get; set; }
        public Int64 PinCode { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? Address { get; set; }
        public Int64 Qualification { get; set; }
        public Int64? CurrentExperience { get; set; }
        public DateTime JoinDate { get; set; }
        public Int64 DesignationName { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? Gender { get; set; }
        public string? MaritalStatus { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? CompanyName { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? CompanyAddress { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? FatherName { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? FatherOccupation { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? MotherName { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? MotherOcupation { get; set; }
        public string? UpdatedBy { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? EmployeeID { get; set; }
        public string? PhotoPath { get; set; }
        [MaxFileSize(MaxSize = (int)(2 * 1024 * 1024))]
        [ValidFileType(Extensions = new string[] { ".JPG", ".jpg", ".png" })]
        public IFormFile? AttachmentPhoto { get; set; }
        public string? SignaturePath { get; set; }
        [MaxFileSize(MaxSize = (int)(2 * 1024 * 1024))]
        [ValidFileType(Extensions = new string[] { ".JPG", ".jpg", ".png"})]
        public IFormFile? AttachmentSignature { get; set; }

    }
    public class EmployeeGetDetails
    {
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? EmployeeID { get; set; }

    }
    public class EmployeeDelete
    {
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? EmployeeID { get; set; }
        public string? DeletedBy { get; set; }

    }
    public class GetEmployeeFormPDF
    {
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
        public string? MaritalStatus  { get; set;}
        public string? FatherOccupation { get; set; }
        public string? MotherName { get; set; }
        public string? MotherOcupation { get; set; }

    }
}
