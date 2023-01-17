using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Model
{
    public class Leave
    {

        public string? LeaveId { get; set; }
        public string? EmailId { get; set; }
        public Int64 PhoneNumber { get; set; }
        public string? EmployeeName { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string? EmployeeId { get; set; }
        public string? Reason { get; set; }
        public string? LeaveType { get; set; }
        public string? StatusType { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class LeaveInsert
    {
        [Required]
        public string? FromDate { get; set; }
        [Required]
        public string? ToDate { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? EmployeeId { get; set; }
        [Required, StringLength(1000, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? Reason { get; set; }
        [Required]
        public int? LeaveType { get; set; }
        public string? CreatedBy { get; set; }
    }
    public class LeaveUpdate
    {
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? LeaveId { get; set; }
        [Required]
        public string? FromDate { get; set; }
        [Required]
        public string? ToDate { get; set; }
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? EmployeeId { get; set; }
        [Required, StringLength(1000, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? Reason { get; set; }
        [Required]
        public int? LeaveType { get; set; }
        [Required]
        public int? StatusType { get; set; }
        public string? UpdatedBy { get; set; }
    }
    public class LeaveGetDetails
    {
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? LeaveId { get; set; }
    }
    public class LeaveDelete
    {
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? LeaveId { get; set; }
        public string? DeletedBy { get; set; }
    }
}
