using LtiRulesEngine.models;
using LtiRulesEngine.services;
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

        [HttpGet("colors")]
        public async Task<RulesEngineResponse> Colors(string data) {
            //return Enumerable.Range(1, 5).Select(index => new RulesService {
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //})
            //.ToArray();
            var response = await new ColorService().Colors(data);
            return response;
        }

        [HttpGet("jre")]
        public async Task<RulesEngineResponse> Jre(string data) {
            //return Enumerable.Range(1, 5).Select(index => new RulesService {
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //})
            //.ToArray();
            var jreRules = new List<string>() { getJson("jre-rules.json") };
            var response = await new JreService().JreRules(data, jreRules);
            return response;
        }

        private string getJson(string dataFileName) {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), dataFileName, SearchOption.AllDirectories);
            if (files == null || files.Length == 0)
                throw new Exception("Rules not found.");
            var fileData = System.IO.File.ReadAllText(files[0]);
            return fileData;
        }
    }
}