using AspDotNetCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreApi.Orchestrator
{
    public interface IAdventureWorksOrchestrator
    {
        Task<List<Department>> GetDepartments();
    }
}
