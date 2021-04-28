using AspDotNetCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreApi.Repositories
{
    public interface IAdventureWorksRepository
    {
        Task<List<Department>> GetDepartments(string sql);
    }
}
