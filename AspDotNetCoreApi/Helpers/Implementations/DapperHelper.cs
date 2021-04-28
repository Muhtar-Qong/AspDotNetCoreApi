using AspDotNetCoreApi.Models;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreApi.Helpers.Implementations
{
    public class DapperHelper : IDapperHelper
    {
        private static Settings _setting;

        public DapperHelper(IOptions<Settings> settings)
        {
            _setting = settings.Value;
        }

        public async Task<List<T>> QueryAsync<T>(string sql, object parameters = null)
        {
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentException("Sql quesry can not be empty");

            using (var connection = new SqlConnection(_setting.ConnectionString))
            {
                var result = await connection.QueryAsync<T>(sql, parameters);
                return result?.ToList();
            }
        }
    }
}
