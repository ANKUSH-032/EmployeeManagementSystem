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
    public interface IUserRepositroy
    {
        Task<Response> UserInsert(UserInsert userInsert);
        Task<Response> UserUpdate(UserUpdate userUpdate);
        Task<ResponseList<UserGetDetails>> UserGetList(JqueryDataTable list);
       // Task<ResponseList<UserPermission>> UserPermissionsGetList(GetUser userGetList);
        Task<Response<UserInsert>> UserGetDetails(GetUser userGetDetails);
        Task<Response> UserDelete(DeleteUser userDelete);
       // Task<Response> UserPermissionsSave(List<UserPermission> PermissionsLst);
    }
}
