using Dapper;
using Eob_Web.Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eob_Web.Core.Services
{
    public class Repository : IRepository
    {
        private readonly IConfiguration _configuration;
        private string _connection_String;

        public Repository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection_String = _configuration.GetConnectionString("Data");
        }

        /// <summary>
        /// Read data
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <typeparam name="U">Parameters</typeparam>
        /// <param name="procedure">The SQL procedure to execute</param>
        /// <param name="parameters2">Data</param>
        /// <returns>List of Models</returns>
        public async Task<IEnumerable<T>> Load<T, U>(string procedure, U parameters)
        {
            using IDbConnection connection = new SqlConnection(_connection_String);
            return await connection.QueryAsync<T>(procedure, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Read data from 2 joined tables
        /// </summary>
        /// <typeparam name="T1">Model 1</typeparam>
        /// <typeparam name="T2">Model 2</typeparam>
        /// <typeparam name="TReturn">Model to return</typeparam>
        /// <typeparam name="U">Parameters</typeparam>
        /// <param name="procedure">The SQL procedure to execute</param>
        /// <param name="parameters">Data</param>
        /// <param name="mapping">delegate function on how to map the objects</param>
        /// <returns>List of Models</returns>
        public async Task<IEnumerable<TReturn>> Load_Multi<T1, T2, TReturn, U>(string procedure, U parameters, Func<T1, T2, TReturn> mapping)
        {
            using IDbConnection connection = new SqlConnection(_connection_String);
            return await connection.QueryAsync(
                sql: procedure,
                map: mapping,
                param: parameters,
                commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Read data from 3 joined tables
        /// </summary>
        /// <typeparam name="T1">Model 1</typeparam>
        /// <typeparam name="T2">Model 2</typeparam>
        /// <typeparam name="T2">Model 3</typeparam>
        /// <typeparam name="TReturn">Model to return</typeparam>
        /// <typeparam name="U">Parameters</typeparam>
        /// <param name="procedure">The SQL procedure to execute</param>
        /// <param name="parameters">Data</param>
        /// <param name="mapping">delegate function on how to map the objects</param>
        /// <returns>List of Models</returns>
        public async Task<IEnumerable<TReturn>> Load_Multi<T1, T2, T3, TReturn, U>(string procedure, U parameters, Func<T1, T2, T3, TReturn> mapping)
        {
            using IDbConnection connection = new SqlConnection(_connection_String);
            return await connection.QueryAsync(
                sql: procedure,
                map: mapping,
                param: parameters,
                commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Read data from 4 joined tables
        /// </summary>
        /// <typeparam name="T1">Model 1</typeparam>
        /// <typeparam name="T2">Model 2</typeparam>
        /// <typeparam name="T3">Model 3</typeparam>
        /// <typeparam name="T4">Model 4</typeparam>
        /// <typeparam name="TReturn">Model to return</typeparam>
        /// <typeparam name="U">Parameters</typeparam>
        /// <param name="procedure">The SQL procedure to execute</param>
        /// <param name="parameters">Data</param>
        /// <param name="mapping">delegate function on how to map the objects</param>
        /// <returns>List of Models</returns>
        public async Task<IEnumerable<TReturn>> Load_Multi<T1, T2, T3, T4, TReturn, U>(string procedure, U parameters, Func<T1, T2, T3, T4, TReturn> mapping)
        {
            using IDbConnection connection = new SqlConnection(_connection_String);
            return await connection.QueryAsync(
                sql: procedure,
                map: mapping,
                param: parameters,
                commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Create, Update or Delete data
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="procedure">The SQL procedure to execute</param>
        /// <param name="parameters">Data</param>
        /// <returns>Inserted/Deleted Id</returns>
        public async Task<int> Modify<T>(string procedure, T parameters)
        {
            using IDbConnection connection = new SqlConnection(_connection_String);
            return await connection.ExecuteScalarAsync<int>(
                sql: procedure,
                param: parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}
