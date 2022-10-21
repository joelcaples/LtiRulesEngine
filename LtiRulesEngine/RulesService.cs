//using LtiRulesEngine.dto;
using RulesEngine.Extensions;
using RulesEngine.Models;
//using System.Data;
using System.Dynamic;
using System.Text.Json;

namespace LtiRulesEngine {
    public class RulesService {
        //public DateTime Date { get; set; }

        //public int TemperatureC { get; set; }

        //public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        //public string? Summary { get; set; }

        public async void Execute() {

            //string jsonData = File.ReadAllText("data/workflow-01.json");
            //var workflow = JsonSerializer.Deserialize<WorkflowDto>(jsonData);

            //var workflowRules = jsonData;   //Get list of workflow rules declared in the json



            //var re = new RulesEngine.RulesEngine(jsonData);

            //// Declare input1,input2,input3 
            //var input1 = new { };
            //var input2 = new { };
            //var input3 = new { };
            //var resultList = await re.ExecuteAllRulesAsync("Discount", input1, input2, input3);

            ////Check success for rule
            //foreach (var result in resultList) {
            //    Console.WriteLine($"Rule - {result.Rule.RuleName}, IsSuccess - {result.IsSuccess}");
            //}

            var workflows = new List<Workflow>() {
                new Workflow() {
                    WorkflowName = "Test Workflow Rule 1",
                    Rules = new List<Rule>() {
                        new Rule() {
                            RuleName = "Test Rule",
                            SuccessEvent = "Count is within tolerance.",
                            ErrorMessage = "Over expected.",
                            Expression = "count < 3",
                            RuleExpressionType = RuleExpressionType.LambdaExpression
                        }
                    }
                } 
            };

            var rulesEngine = new RulesEngine.RulesEngine(workflows.ToArray(), null);

            dynamic datas = new ExpandoObject();
            datas.count = 1;
            var inputs = new dynamic[] {
                datas
            };

            List<RuleResultTree> resultList = await rulesEngine.ExecuteAllRulesAsync("Test Workflow Rule 1", inputs);

            bool outcome = false;

            //Different ways to show test results:
            outcome = resultList.TrueForAll(r => r.IsSuccess);

            resultList.OnSuccess((eventName) => {
                Console.WriteLine($"Result '{eventName}' is as expected.");
                outcome = true;
            });

            resultList.OnFail(() => {
                outcome = false;
            });

            Console.WriteLine($"Test outcome: {outcome}.");

        }
    }
}