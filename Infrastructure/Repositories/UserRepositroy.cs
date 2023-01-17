using Core.Comman;
using Core.Interface;
using Core.Model;
using CrudOperations;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepositroy : IUserRepositroy
    {
        private readonly ICrudOperationService _crudOperation;
        private static string _con = string.Empty;
        private static IConfigurationRoot _iconfiguration;
        public UserRepositroy(ICrudOperationService crudOperation)
        {
            this._crudOperation = crudOperation;
            var builder = new ConfigurationBuilder()
                 .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                 .AddJsonFile("appsettings.json");
            _iconfiguration = builder.Build();
            _con = _iconfiguration["ConnectionStrings:DataAccessConnection"];
        }
        public static IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_con);
            }
        }
        public async Task<Response> UserInsert(UserInsert userInsert)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            userInsert.FirstName = textInfo.ToTitleCase(userInsert.FirstName);
            userInsert.MiddleName = textInfo.ToTitleCase(userInsert.MiddleName);
            userInsert.LastName = textInfo.ToTitleCase(userInsert.LastName);
            return await _crudOperation.InsertUpdateDelete<Response>(storedProcedureName: "[dbo].[uspUserInsert]", new
            {
                userInsert.FirstName,
                userInsert.MiddleName,
                userInsert.LastName,
                userInsert.Email,
                userInsert.ContactNo,
                userInsert.RoleID,
                userInsert.PasswordHash,
                userInsert.PasswordSalt,
                userInsert.CreatedBy,
                
            });
        }
        public async Task<Response> UserUpdate(UserUpdate userUpdate)
        {
            return await _crudOperation.InsertUpdateDelete<Response>(storedProcedureName: "[dbo].[uspUserUpdate]", userUpdate);
        }

        public async Task<ResponseList<UserGetDetails>> UserGetList(JqueryDataTable list)
        {
            return await _crudOperation.GetPaginatedList<UserGetDetails>(storedProcedureName: "[dbo].[uspUserGetList]", list);
        }

        //public async Task<ResponseList<UserPermission>> UserPermissionsGetList(GetUser userGetList)
        //{
        //    return await _crudOperation.GetPaginatedList<UserPermission>(storedProcedureName: "[dbo].[uspUserPermissionsGet]", userGetList);
        //}

        public async Task<Response<UserInsert>> UserGetDetails(GetUser userGetDetails)
        {
            return await _crudOperation.GetSingleRecord<UserInsert>(storedProcedureName: "[dbo].[uspUserGetDetails]", userGetDetails);
        }

        public async Task<Response> UserDelete(DeleteUser userDelete)
        {
            return await _crudOperation.InsertUpdateDelete<Response>(storedProcedureName: "[dbo].[uspUserDelete]", userDelete);
        }

        //public async Task<Response> UserPermissionsSave(List<UserPermission> PermissionsLst)
        //{
        //    dynamic response = null;
        //    foreach (var item in PermissionsLst)
        //    {

        //        response = await _crudOperation.InsertUpdateDelete<Response>(storedProcedureName: "[dbo].[uspUserPermissionsSave]", new
        //        {
        //            item.MappingId,
        //            item.UserId,
        //            item.ModuleId,
        //            item.IsView,
        //            item.IsAdd,
        //            item.IsEdit,
        //            item.IsDelete,
        //            item.UpdatedBy,
        //        });
        //    }
        //    return response;
        //}
    }
}
