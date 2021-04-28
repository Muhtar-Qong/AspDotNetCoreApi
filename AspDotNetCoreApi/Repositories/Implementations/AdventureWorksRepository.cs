using AspDotNetCoreApi.Helpers;
using AspDotNetCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreApi.Repositories.Implementations
{
    public class AdventureWorksRepository : IAdventureWorksRepository
    {
        private readonly IDapperHelper _iDapperHelper;

        public AdventureWorksRepository(IDapperHelper iDapperHelper)
        {
            _iDapperHelper = iDapperHelper;
        }

        public async Task<List<Department>> GetDepartments(string sql) =>
           await _iDapperHelper.QueryAsync<Department>(sql);
    }
}
