using System;
using System.Dynamic;
using System.Text.Json;

using LtiRulesEngine.dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RulesEngine.Extensions;
using RulesEngine.Models;

namespace LtiRulesEngine {
    public class RulesService {
        //public DateTime Date { get; set; }

        //public int TemperatureC { get; set; }

        //public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        //public string? Summary { get; set; }

        /*public async Task<string> Execute() {

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

        }*/

        public async Task<RulesEngineResponse> Colors(string data) {
            var result = await Colors(getDataObject(data));
            return result;
        }

        public async Task<RulesEngineResponse> Colors(ColorRecipe recipe) {

            try {

                var rulesEngine = GetRulesEngine();

                List<RuleResultTree> resultList = await rulesEngine.ExecuteAllRulesAsync("ColorRecipe", recipe);

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

        private RulesEngine.RulesEngine GetRulesEngine() {

            var workflowFiles = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "../../../../"), "color-workflow.json", SearchOption.AllDirectories);

            if (workflowFiles == null || workflowFiles.Length == 0)
                throw new Exception("Rules not found.");
            var workflowData = File.ReadAllText(workflowFiles[0]);

            var workflows = JsonConvert.DeserializeObject<List<Workflow>>(workflowData);
            if (workflows == null || workflows.Count == 0)
                throw new Exception("Workflows not found.");

            return new RulesEngine.RulesEngine(workflows.ToArray(), null);
        }

        private ColorRecipe getDataObject(string data) {
            var converter = new ExpandoObjectConverter();
            var recipe = JsonConvert.DeserializeObject<ColorRecipe>(data, converter);
            if (recipe == null)
                throw new Exception("Invalid Data");
            return recipe;
        }

        // EXAMPLE USING GENERIC OBJECT
        //
        //private dynamic getDataObject(string data) {
        //    var converter = new ExpandoObjectConverter();
        //    var expando = JsonConvert.DeserializeObject<ExpandoObject>(data, converter);
        //    if (expando == null)
        //        throw new Exception("Invalid Data");
        //    //return new RulesEngineResponse() {
        //    //    ExceptionMessage = "Invalid Data"
        //    //};
        //
        //    dynamic colorData = expando;
        //
        //    var obj = new dynamic[] {
        //        colorData
        //    };
        //
        //    return obj;
        //}

        // EXAMPLE CREATING WORKFLOW IN MEMORY:
        //
        //var workflows = new List<Workflow>() {
        //    new Workflow() {
        //        WorkflowName = "Color Rules",
        //        Rules = new List<Rule>() {
        //            new Rule() {
        //                RuleName = "If Orange then Red is required",
        //                SuccessEvent = "Red was added.",
        //                ErrorMessage = "Red was not added.",
        //                Expression = "colors.Any(c => c == \"red\")",
        //                RuleExpressionType = RuleExpressionType.LambdaExpression
        //            }
        //        }
        //    }
        //};

    }
}