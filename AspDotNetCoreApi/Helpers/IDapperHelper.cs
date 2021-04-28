using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreApi.Helpers
{
    public interface IDapperHelper
    {
        Task<List<T>> QueryAsync<T>(string sql, object parameters = null);
    }
}
