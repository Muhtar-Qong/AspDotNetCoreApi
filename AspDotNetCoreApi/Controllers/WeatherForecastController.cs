using AspDotNetCoreApi.Models;
using AspDotNetCoreApi.Orchestrator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IAdventureWorksOrchestrator _iAdventureWorksOrchestrator;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IAdventureWorksOrchestrator iAdventureWorksOrchestrator)
        {
            _logger = logger;
            _iAdventureWorksOrchestrator = iAdventureWorksOrchestrator;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [Authorize]       
        [HttpGet("Departments")]
        public async Task<List<Department>> GetDepartments() =>
            await _iAdventureWorksOrchestrator.GetDepartments();
    }
}
