using LtiRulesEngine.models;
using RulesEngine.Models;
using LtiRulesEngine.util;

namespace LtiRulesEngine.services {
    public class PlatformService {


        public async Task<RulesEngineResponse> Platform(PlatformContext context) {

            try {

                var rulesEngine = RulesEngineUtil.GetRulesEngine("platform");

                List<RuleResultTree> resultList = await rulesEngine.ExecuteAllRulesAsync("Platform", context);

                var response = new RulesEngineResponse() {
                    IsSuccess = resultList.TrueForAll(r => r.IsSuccess),
                };

                response.Messages.AddRange(resultList
                    .Where(r => !r.IsSuccess && !response.Messages.Contains(r.ExceptionMessage))
                    .Select(r => r.ExceptionMessage));

                //resultList.OnSuccess((eventName) => {
                //    Console.WriteLine($"Result '{eventName}' is as expected.");
                //});

                //resultList.OnFail(() => {
                //    response.Messages.AddRange(resultList.Select(r => r.Rule.ErrorMessage));
                //});

                return response;

            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        //private RulesEngine.RulesEngine GetRulesEngine() {

        //    var workflowFiles = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "../../../../"), "platform-workflow.json", SearchOption.AllDirectories);

        //    if (workflowFiles == null || workflowFiles.Length == 0)
        //        throw new Exception("Rules not found.");
        //    var workflowData = File.ReadAllText(workflowFiles[0]);

        //    var workflows = JsonConvert.DeserializeObject<List<Workflow>>(workflowData);
        //    if (workflows == null || workflows.Count == 0)
        //        throw new Exception("Workflows not found.");

        //    return new RulesEngine.RulesEngine(workflows.ToArray(), null);
        //}

        //private ColorRecipe getDataObject(string data) {
        //    var converter = new ExpandoObjectConverter();
        //    var recipe = JsonConvert.DeserializeObject<ColorRecipe>(data, converter);
        //    if (recipe == null)
        //        throw new Exception("Invalid Data");
        //    return recipe;

        //}
    }
}