using System;
using LtiRulesEngine.models;
using LtiRulesEngine.util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RulesEngine.Models;

namespace LtiRulesEngine.services
{

    public class ColorService
    {
        public async Task<RulesEngineResponse> Colors(string data)
        {
            var result = await Colors(FromJson(data));
            return result;
        }

        public async Task<RulesEngineResponse> Colors(ColorRecipe recipe)
        {
            try
            {

                var rulesEngine = RulesEngineUtil.GetRulesEngine("color");

                List<RuleResultTree> resultList = await rulesEngine.ExecuteAllRulesAsync("Color", recipe);

                var response = new RulesEngineResponse()
                {
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

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        private ColorRecipe FromJson(string data)
        {
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