using LtiRulesEngine.dto;
using Microsoft.AspNetCore.Mvc;

namespace LtiRulesEngine.Controllers {
    [ApiController]
    [Route("rules")]
    public class RulesController : ControllerBase {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<RulesController> _logger;

        public RulesController(ILogger<RulesController> logger) {
            _logger = logger;
        }

        [HttpGet("ping")]
        public string Pint() {
            return "Pong";
        }

        [HttpGet("execute")]
        public async Task<string> Execute() {
            //return Enumerable.Range(1, 5).Select(index => new RulesService {
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //})
            //.ToArray();
            var response = await new RulesService().Execute();
            return response;
        }

        [HttpGet("colors")]
        public async Task<RulesEngineResponse> Colors(string data) {
            //return Enumerable.Range(1, 5).Select(index => new RulesService {
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //})
            //.ToArray();
            var response = await new RulesService().Colors(data);
            return response;
        }
    }
}