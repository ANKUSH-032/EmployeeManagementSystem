using CORE.Comman;
using CORE.Model;
using CrudOperation;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Helper
{
    public class CommanDDLRepository : ICommanDDLRepository
    {
        private static IConfigurationRoot _iconfiguration;
        private readonly ICrudOperationService _crudOperation;
        private static string _con = string.Empty;
        private static string _secretKey = string.Empty;

        public CommanDDLRepository(ICrudOperationService crudOperation)
        {
            this._crudOperation = crudOperation;
            var builder = new ConfigurationBuilder()
                 .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                 .AddJsonFile("appsettings.json");
            _iconfiguration = builder.Build();

            _con = _iconfiguration["ConnectionStrings:DataAccessConnection"];
            _secretKey = _iconfiguration["AppSettings:Secret"];
        }

        public static IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_con);
            }
        }
        public Task<List<DDList>> DDLgetlist(CommanDDLFuntion commonInput)
        {
            //return _crudOperation.GetPaginatedList<DDList>(storedProcedureName: "[dbo].[uspCommonGetDDL]", commonInput);
            List<DDList> response;

            using (IDbConnection db = Connection)
            {
                var res = db.QueryMultiple("[dbo].[uspCommonGetDDL]", new
                {
                    commonInput.Type,
                    commonInput.Id
                }, commandType: CommandType.StoredProcedure);

                response = res.Read<DDList>().ToList();
            }

            return Task.FromResult(response);
        }
    }
}
