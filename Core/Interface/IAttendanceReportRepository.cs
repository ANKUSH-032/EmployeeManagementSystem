using Core.Comman;
using Core.Model;
using CrudOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IAttendanceReportRepository
    {
        Task<Response> LogInOrLogOutAttendenceReport(LogInOrLogOutAttendenceReport logInOrLogOutAttendenceReport);
        Task<Response> AttendenceDelete(AttendenceDelete attendenceDelete);
        Task<ResponseList<AttendenceReport>> AttendenceGetList(string employeeId, JqueryDataTable list);
        Task<Response<AttendenceReport>> AttendenceGetDetails(AttendenceGetDetails attendenceGetDetails);
    }
}
