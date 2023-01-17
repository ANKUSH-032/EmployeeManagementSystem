using Core.Comman;
using CORE.Model;
using CrudOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Interface
{
    public interface ILeaveRepository
    {
        Task<Response> LeaveInsert(LeaveInsert leaveInsert);
        Task<Response<Leave>> LeaveGetDetails(LeaveGetDetails leaveGetDetails);
        Task<Response> LeaveUpdate(LeaveUpdate leaveUpdate);
        Task<Response> LeaveDelete(LeaveDelete leaveDelete);
        Task<ResponseList<Leave>> LeaveGetList(string employeeId, JqueryDataTable list);
    }
}
