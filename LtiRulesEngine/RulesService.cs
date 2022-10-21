//using LtiRulesEngine.dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using RulesEngine.Extensions;
using RulesEngine.Models;
using System;
//using System.Data;
using System.Dynamic;
using System.Text.Json;

namespace LtiRulesEngine {
    public class RulesService {
        //public DateTime Date { get; set; }

        //public int TemperatureC { get; set; }

        //public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        //public string? Summary { get; set; }

        public async Task<string> Execute() {

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
            return $"Test outcome: {outcome}.";

        }


        public async Task<string> Colors(string data) {

            var workflows = new List<Workflow>() {
                new Workflow() {
                    WorkflowName = "Color Rules",
                    Rules = new List<Rule>() {
                        new Rule() {
                            RuleName = "If Orange then Red is required",
                            SuccessEvent = "Red was added.",
                            ErrorMessage = "Red was not added.",
                            Expression = "colors.Any(c => c == \"red\")",
                            RuleExpressionType = RuleExpressionType.LambdaExpression
                        }
                    }
                }
            };

            var rulesEngine = new RulesEngine.RulesEngine(workflows.ToArray(), null);

            //dynamic datas = new ExpandoObject();
            //datas.colors = new List<string>() { "red" };

            //string json = @"
            //{
            //    ""colors"": [
            //        ""blue"",
            //        ""green""
            //    ]
            //}";


            var converter = new ExpandoObjectConverter();
            var expando = JsonConvert.DeserializeObject<ExpandoObject>(data, converter);
            if (expando == null)
                return "Invalid data";

            dynamic colorData = expando;

            //JObject datas = JObject.Parse(json);

            var inputs = new dynamic[] {
                colorData
            };

            List<RuleResultTree> resultList = await rulesEngine.ExecuteAllRulesAsync("Color Rules", inputs);

            bool outcome = false;

            //Different ways to show test results:
            outcome = resultList.TrueForAll(r => r.IsSuccess);

            resultList.OnSuccess((eventName) => {
                Console.WriteLine($"Result '{eventName}' is as expected.");
                outcome = true;
            });

            var msg = System.Environment.NewLine;
            resultList.OnFail(() => {
                outcome = false;
                msg += string.Join(System.Environment.NewLine, resultList.Select(r => r.Rule.ErrorMessage));
            });

            Console.WriteLine($"Test outcome: {outcome}." + msg);
            return $"Test outcome: {outcome}." + msg;

        }

    }
}