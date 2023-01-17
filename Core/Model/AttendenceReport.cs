using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class AttendenceReport
    {
       public string? AttendenceId { get; set; }
       public string? EmployeeId { get; set; }
       public DateTime? FromTime { get; set; }
       public DateTime? ToTime { get; set; }
       public string? CreatedBy { get; set; }
       public DateTime? CreatedOn { get; set; }
       public string? UpdateBy { get; set; }
       public DateTime? UpdatedOn { get; set; }
       
       public DateTime? DeletedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? AttendenceStatus { get; set; }
    }
    public class LogInOrLogOutAttendenceReport
    {
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? EmployeeId { get; set; }
        public string? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        //public string? UpdatedBy { get; set; }
        public bool? AttendenceStatus { get; set; }
        public string? EmailId { get; set; }
        //public string? Password { get; set; }
    }
    public class AttendenceGetDetails
    {
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? AttendenceId { get; set; }
    }
    public class AttendenceDelete
    {
        [Required, StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        public string? AttendenceId { get; set; }
        public string? DeletedBy { get; set; }
    }
}
