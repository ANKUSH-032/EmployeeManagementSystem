using Core.Comman;
using Core.Model;
using CrudOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IAllowanceRepository
    {
        Task<Response> AllowancesInsert(AllowancesInsert allowancesInsert);
        Task<Response<Allowances>> AllowancesGetDetails(AllowancesGetDetails allowancesGetDetails);
        Task<Response> AllowancesUpdate(AllowancesUpdate allowancesUpdate);
        Task<Response> AllowancesGetDelete(AllowancesGetDelete allowancesGetDelete);
        Task<ResponseList<Allowances>> AllowancesGetList(string employeeId, JqueryDataTable allowancesGetList);
    }
}
