using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CrudOperation
{
#pragma warning disable
    public class CrudOperationDataAccess : ICrudOperationService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public CrudOperationDataAccess(IConfiguration configuration, string connectionString)
        {
            _configuration = configuration;
            _connectionString = connectionString;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public string GetConnectionString(string connectionName)
        {
            return this._configuration.GetConnectionString(connectionName);
        }

        public IConfigurationSection GetConfigurationSection(string key)
        {
            return this._configuration.GetSection(key);
        }

        public string AppSettingsKeys(string nodeName, string key)
        {
            return this._configuration["" + nodeName + ":" + key + ""];
        }

        public Task<T> Insert<T>(string storedProcedureName, DynamicParameters parameters)
        {
            T response;
            using (var connections = Connection)
            {
                try
                {
                    response = connections.QueryFirstOrDefault<T>(
                        sql: storedProcedureName,
                        param: parameters,
                        commandTimeout: null,
                        commandType: CommandType.StoredProcedure
                        );
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (connections.State == ConnectionState.Open)
                        connections.Close();
                }
                return Task.FromResult(response);
            }
        }

        public async Task<Response<T>> InsertAndGet<T>(string storedProcedureName, object parametersPocoObj)
        {
            using var connections = Connection;
            try
            {
                var result = await connections.QueryMultipleAsync(
                    sql: storedProcedureName,
                    param: GenricsDynamicParamterMapper(parametersPocoObj),
                    commandTimeout: null,
                    commandType: CommandType.StoredProcedure
                    );

                Response<T> response = result.Read<Response<T>>().FirstOrDefault();
                response.Data = response.Status ? result.Read<T>().FirstOrDefault() : default;
                return response;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connections.State == ConnectionState.Open)
                    connections.Close();
            }
        }

        public Task<T> Update<T>(string storedProcedureName, DynamicParameters parameters)
        {
            T response;
            using (var connections = Connection)
            {
                try
                {
                    response = connections.QueryFirstOrDefault<T>(
                        sql: storedProcedureName,
                        param: parameters,
                        commandTimeout: null,
                        commandType: CommandType.StoredProcedure
                        );
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (connections.State == ConnectionState.Open)
                        connections.Close();
                }
                return Task.FromResult(response);
            }
        }

        public Task<T> Delete<T>(string storedProcedureName, DynamicParameters parameters)
        {
            T response;
            using (var connections = Connection)
            {
                try
                {
                    response = connections.QueryFirstOrDefault<T>(
                        sql: storedProcedureName,
                        param: parameters,
                        commandTimeout: null,
                        commandType: CommandType.StoredProcedure
                        );
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (connections.State == ConnectionState.Open)
                        connections.Close();
                }
                return Task.FromResult(response);
            }
        }

        public Task<T> InsertUpdateDelete<T>(string storedProcedureName, object spParamsPocoMapper)
        {
            T response;
            using (var connections = Connection)
            {
                try
                {
                    response = connections.QueryFirstOrDefault<T>(
                        sql: storedProcedureName,
                        param: GenricsDynamicParamterMapper(spParamsPocoMapper),
                        commandTimeout: null,
                        commandType: CommandType.StoredProcedure
                        );
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (connections.State == ConnectionState.Open)
                        connections.Close();
                }
                return Task.FromResult(response);
            }
        }

        //public Task<T> GetSingleRecord<T>(string storedProcedureName, DynamicParameters parameters)
        //{
        //    T response;
        //    using (var connections = Connection)
        //    {
        //        try
        //        {
        //            //using (var multi = connections.QueryMultipleAsync(storedProcedureName, new
        //            //{
        //            //    parameters
        //            //}).Result)
        //            //{
        //            //    response = multi.Read<T>().FirstOrDefault();
        //            //}

        //            response = (T)connections.QueryFirstOrDefault<T>(
        //                sql: storedProcedureName,
        //                param: parameters,
        //                commandTimeout: null,
        //                commandType: CommandType.StoredProcedure
        //                );
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //        finally
        //        {
        //            if (connections.State == ConnectionState.Open)
        //                connections.Close();
        //        }
        //        return Task.FromResult(response);
        //    }
        //}

        //public Tuple<Model, Response> GetSingleRecord<Model, Response>(string storedProcedureName, DynamicParameters parameters)
        //{
        //    using (var connections = Connection)
        //    {
        //        try
        //        {
        //            var result = connections.QueryMultipleAsync(
        //                sql: storedProcedureName,
        //                param: parameters,
        //                commandTimeout: null,
        //                commandType: CommandType.StoredProcedure
        //                ).Result;
        //            return Tuple.Create(result.Read<Model>().FirstOrDefault(), result.Read<Response>().FirstOrDefault());

        //            //var abc = ValueTuple.Create(result.Read<T>().FirstOrDefault(), result.Read<R>().FirstOrDefault());
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //        finally
        //        {
        //            if (connections.State == ConnectionState.Open)
        //                connections.Close();
        //        }

        //    }
        //}

        public async Task<Response<T>> GetSingleRecord<T>(string storedProcedureName, DynamicParameters parameters)
        {
            using (var connections = Connection)
            {
                try
                {
                    var result = await connections.QueryMultipleAsync(
                        sql: storedProcedureName,
                        param: parameters,
                        commandTimeout: null,
                        commandType: CommandType.StoredProcedure
                        );

                    Response<T> response = result.Read<Response<T>>().FirstOrDefault();
                    response.Data = response.Status ? result.Read<T>().FirstOrDefault() : default;
                    return response;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (connections.State == ConnectionState.Open)
                        connections.Close();
                }
            }
        }

        public async Task<Response<T>> GetSingleRecord<T>(string storedProcedureName, object spParamsPocoMapper)
        {
            using (var connections = Connection)
            {
                try
                {
                    var result = await connections.QueryMultipleAsync(
                        sql: storedProcedureName,
                        param: GenricsDynamicParamterMapper(spParamsPocoMapper),
                        commandTimeout: null,
                        commandType: CommandType.StoredProcedure
                        );

                    Response<T> response = result.Read<Response<T>>().FirstOrDefault();
                    response.Data = response.Status ? result.Read<T>().FirstOrDefault() : default;
                    return response;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (connections.State == ConnectionState.Open)
                        connections.Close();
                }
            }
        }

        public async Task<Response<T>> GetSingleRecord<T>(string storedProcedureName)
        {
            using (var connections = Connection)
            {
                try
                {
                    var result = await connections.QueryMultipleAsync(
                        sql: storedProcedureName,
                        commandTimeout: null,
                        commandType: CommandType.StoredProcedure
                        );

                    Response<T> response = result.Read<Response<T>>().FirstOrDefault();
                    response.Data = response.Status ? result.Read<T>().FirstOrDefault() : default;
                    return response;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (connections.State == ConnectionState.Open)
                        connections.Close();
                }
            }
        }

        public async Task<ResponseList<T>> GetPaginatedList<T>(string storedProcedureName, DynamicParameters parameters)
        {
            using (var connections = Connection)
            {
                try
                {
                    var result = await connections.QueryMultipleAsync(
                        sql: storedProcedureName,
                        param: parameters,
                        commandTimeout: null,
                        commandType: CommandType.StoredProcedure
                        );

                    ResponseList<T> response = result.Read<ResponseList<T>>().FirstOrDefault();
                    response.Data = response.Status ? result.Read<T>().ToList() : default;
                    response.TotalRecords = response.RecordsFiltered = response.Status ? result.Read<int>().SingleOrDefault() : 0;
                    return response;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (connections.State == ConnectionState.Open)
                        connections.Close();
                }
            }
        }

        public async Task<Response<Tuple<T1, List<T2>>>> GetSingleRecord<T1, T2>(string storedProcedureName, DynamicParameters parameters)
        {
            using (var connections = Connection)
            {
                try
                {
                    var result = await connections.QueryMultipleAsync(
                        sql: storedProcedureName,
                        param: parameters,
                        commandTimeout: null,
                        commandType: CommandType.StoredProcedure
                        );

                    Response<Tuple<T1, List<T2>>> response = result.Read<Response<Tuple<T1, List<T2>>>>().FirstOrDefault();
                    var firstObject = response.Status ? result.Read<T1>().FirstOrDefault() : default;
                    var listObject = response.Status ? result.Read<T2>()?.ToList() : default;
                    response.Data = new Tuple<T1, List<T2>>(firstObject, listObject);

                    return response;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (connections.State == ConnectionState.Open)
                        connections.Close();
                }
            }
        }

        public async Task<Response<Tuple<T1, List<T2>>>> GetSingleRecord<T1, T2>(string storedProcedureName, object parameters)
        {
            using (var connections = Connection)
            {
                try
                {
                    var result = await connections.QueryMultipleAsync(
                        sql: storedProcedureName,
                        param: GenricsDynamicParamterMapper(parameters),
                        commandTimeout: null,
                        commandType: CommandType.StoredProcedure
                        );

                    Response<Tuple<T1, List<T2>>> response = result.Read<Response<Tuple<T1, List<T2>>>>().FirstOrDefault();
                    var firstObject = response.Status ? result.Read<T1>().FirstOrDefault() : default;
                    var listObject = response.Status ? result.Read<T2>()?.ToList() : default;
                    response.Data = new Tuple<T1, List<T2>>(firstObject, listObject);

                    return response;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (connections.State == ConnectionState.Open)
                        connections.Close();
                }
            }
        }

        public async Task<ResponseList<T>> GetList<T>(string storedProcedureName, DynamicParameters parameters)
        {
            using var connections = Connection;
            try
            {
                var result = await connections.QueryMultipleAsync(
                    sql: storedProcedureName,
                    param: parameters,
                    commandTimeout: null,
                    commandType: CommandType.StoredProcedure
                    );

                ResponseList<T> response = result.Read<ResponseList<T>>().FirstOrDefault();
                response.Data = response.Status ? result.Read<T>().ToList() : default;
                return response;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connections.State == ConnectionState.Open)
                    connections.Close();
            }
        }

        public async Task<ResponseList<T>> GetList<T>(string storedProcedureName, object spParamsPocoMapper)
        {
            using var connections = Connection;
            try
            {
                var result = await connections.QueryMultipleAsync(
                    sql: storedProcedureName,
                    param: GenricsDynamicParamterMapper(spParamsPocoMapper),
                    commandTimeout: null,
                    commandType: CommandType.StoredProcedure
                    );

                ResponseList<T> response = result.Read<ResponseList<T>>().FirstOrDefault();
                response.Data = response.Status ? result.Read<T>().ToList() : default;
                return response;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connections.State == ConnectionState.Open)
                    connections.Close();
            }
        }

        public async Task<ResponseList<T>> GetPaginatedList<T>(string storedProcedureName, object spParamsPocoMapper)
        {
            using (var connections = Connection)
            {
                try
                {
                    var result = await connections.QueryMultipleAsync(
                        sql: storedProcedureName,
                        param: GenricsDynamicParamterMapper(spParamsPocoMapper),
                        commandTimeout: null,
                        commandType: CommandType.StoredProcedure
                        );

                    ResponseList<T> response = result.Read<ResponseList<T>>().FirstOrDefault();
                    response.Data = response.Status ? result.Read<T>().ToList() : default;
                    response.TotalRecords = response.RecordsFiltered = response.Status ? result.Read<int>().SingleOrDefault() : 0;
                    return response;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (connections.State == ConnectionState.Open)
                        connections.Close();
                }
            }
        }

        //public Tuple<List<Model>,Response> GetPaginatedList<Model, Response>(string storedProcedureName, DynamicParameters parameters)
        //{
        //    using (var connections = Connection)
        //    {
        //        try
        //        {
        //            var result = connections.QueryMultipleAsync(
        //                sql: storedProcedureName,
        //                param: parameters,
        //                commandTimeout: null,
        //                commandType: CommandType.StoredProcedure
        //                ).Result;

        //            return Tuple.Create(result.Read<Model>().ToList(),
        //                                result.Read<Response>().FirstOrDefault());
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //        finally
        //        {
        //            if (connections.State == ConnectionState.Open)
        //                connections.Close();
        //        }

        //    }
        //}

        private static DynamicParameters GenricsDynamicParamterMapper(object tmodelObj)
        {
            var parameter = new DynamicParameters();

            Type tModelType = tmodelObj.GetType();

            //We will be defining a PropertyInfo Object which contains details about the class property
            PropertyInfo[] arrayPropertyInfos = tModelType.GetProperties();

            //Now we will loop in all properties one by one to get value
            foreach (PropertyInfo property in arrayPropertyInfos)
            {
                if (!property.CustomAttributes.Any(x => x.AttributeType.Name.Contains("Ignore")))
                {
                    if (property.PropertyType == typeof(DataTable))
                    {
                        parameter.Add(string.Concat("@", property.Name), ((DataTable)property.GetValue(tmodelObj)).AsTableValuedParameter());
                    }
                    else
                    {
                        parameter.Add(string.Concat("@", property.Name), property.GetValue(tmodelObj));
                    }
                }
            }

            return parameter;
        }
    }
}
