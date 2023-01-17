using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Comman
{
    public class ClsResponse
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public string? Data { get; set; }
    }
    public class ClsResponse<T>
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public long? TotalRecords { get; set; } = 0;
        public long RecordsFiltered { get; set; }
       // public long? TotalAttendance { get; set; } = 0;
    }
    public class JqueryDataTable
    {
        public string? SearchKey { get; set; } //= string.Empty;
        public int? Start { get; set; }
        public int? PageSize { get; set; }
        public string? SortCol { get; set; }// = string.Empty;
    }
    public class AttendenceGetList : JqueryDataTable
    {
        public string? EmployeeId { get; set; }
    }
    public class DeductionGetList : JqueryDataTable
    {
        public string? EmployeeId { get; set; }
    }
    public class AllowancesGetList : JqueryDataTable
    {
        public string? EmployeeId { get; set; }
    }
}
