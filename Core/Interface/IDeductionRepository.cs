using Core.Comman;
using Core.Model;
using CrudOperation;


namespace Core.Interface
{
    public interface IDeductionRepository
    {
        Task<Response> DeductionInsert(DeductionInsert deductionInsert);
        Task<Response<Deduction>> DeductionGetDeatails(DeductionGetDeatails deductionGetDeatails);
        Task<Response> DeductionUpdate(DeductionUpdate deductionUpdate);
        Task<Response> DeductionDelete(DeductionDelete deductionDelete);
        Task<ResponseList<Deduction>> DeductionList(string employeeId, JqueryDataTable list);
    }
}
