using AspDotNetCoreApi.Models;
using AspDotNetCoreApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreApi.Orchestrator.Implementations
{
    public class AdventureWorksOrchestrator : IAdventureWorksOrchestrator
    {
        private readonly IAdventureWorksRepository _iAdventureWorksRepository;

        public AdventureWorksOrchestrator(IAdventureWorksRepository iAdventureWorksRepository)
        {
            _iAdventureWorksRepository = iAdventureWorksRepository;
        }

        public async Task<List<Department>> GetDepartments() =>
            await _iAdventureWorksRepository.GetDepartments(getDepartmentsSql);

        private static string getDepartmentsSql =>
            @"SELECT [DepartmentID],[Name],[GroupName],[ModifiedDate] 
              FROM [AdventureWorks2014].[HumanResources].[Department]";
    }
}
