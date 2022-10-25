using LtiRulesEngine.models;
using Newtonsoft.Json;
using RulesEngine.Models;
using System.Text;

namespace LtiRulesEngine.util {
    public class RulesEngineUtil {

        public static RulesEngine.RulesEngine GetRulesEngine(string workflowName) {

            var workflowFiles = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "../../../../"), $"{workflowName}-workflow.json", SearchOption.AllDirectories);

            if (workflowFiles == null || workflowFiles.Length == 0)
                throw new Exception("Rules not found.");
            var workflowData = File.ReadAllText(workflowFiles[0]);

            var workflows = JsonConvert.DeserializeObject<List<Workflow>>(workflowData);
            if (workflows == null || workflows.Count == 0)
                throw new Exception("Workflows not found.");

            return new RulesEngine.RulesEngine(workflows.ToArray(), null);
        }
    }
}
