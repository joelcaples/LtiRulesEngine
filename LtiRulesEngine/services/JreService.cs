using LtiRulesEngine.models;
using LtiRulesEngine.util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RulesEngine.Models;
using System.Dynamic;
using System.Text;

namespace LtiRulesEngine.services {
    public class JreService {

        public async Task<RulesEngineResponse> JreRules(string context, List<string> jreRules) {

            try {

                var rulesEngine = GetRulesEngine(jreRules.Select(r => FromJson(r)));

                List<RuleResultTree> resultList = await rulesEngine.ExecuteAllRulesAsync("JRERULES", getDataObject(context));

                var response = new RulesEngineResponse() {
                    IsSuccess = resultList.TrueForAll(r => r.IsSuccess),
                };

                response.Messages.AddRange(resultList
                    .Where(r => !r.IsSuccess && !response.Messages.Contains(r.ExceptionMessage))
                    .Select(r => r.ExceptionMessage));

                return response;

            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        private dynamic getDataObject(string data) {
            var converter = new ExpandoObjectConverter();
            var expando = JsonConvert.DeserializeObject<ExpandoObject>(data, converter);
            if (expando == null)
                throw new Exception("Invalid Data");

            dynamic colorData = expando;

            var obj = new dynamic[] {
                colorData
            };

            return obj;
        }

        public static RulesEngine.RulesEngine GetRulesEngine(IEnumerable<JreRule> jreRules) {

            var rules = new List<Rule>();
            var workflows = new List<Workflow>() { };

            foreach (var jreRule in jreRules) {

                if (jreRule.Conditions?.All == null)
                    continue;

                var sb = new StringBuilder();

                foreach (var a in jreRule.Conditions.All) {
                    if (rules.Count > 0)
                        sb.Append(" AND ");

                    switch (a.Operator) {
                        case "equal-ignore-case":
                        default:
                            sb.Append($"{a.Fact}.ToLower() == \"{a.Path?.ToLower()}\"");
                            break;
                    }
                }

                workflows.Add(new Workflow() {
                    WorkflowName = "JRERULES",
                    Rules = new List<Rule>() {
                        new Rule() {
                            RuleName = jreRule.Name,
                            //SuccessEvent = ,
                            //ErrorMessage = "",
                            Expression = sb.ToString(),
                            RuleExpressionType = RuleExpressionType.LambdaExpression
                        }
                    }
                });
            }

            return new RulesEngine.RulesEngine(workflows.ToArray(), null);
        }

        private JreRule FromJson(string data) {
            var converter = new ExpandoObjectConverter();
            var jreRule = JsonConvert.DeserializeObject<JreRule>(data, converter);
            if (jreRule == null)
                throw new Exception("Invalid Data");
            return jreRule;
        }
    }
}
