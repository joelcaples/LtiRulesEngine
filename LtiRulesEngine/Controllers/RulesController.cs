using Microsoft.AspNetCore.Mvc;

namespace LtiRulesEngine.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class RulesController : ControllerBase {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<RulesController> _logger;

        public RulesController(ILogger<RulesController> logger) {
            _logger = logger;
        }

        [HttpGet(Name = "execute")]
        public void Execute() {
            //return Enumerable.Range(1, 5).Select(index => new RulesService {
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //})
            //.ToArray();
            new RulesService().Execute();
        }
    }
}