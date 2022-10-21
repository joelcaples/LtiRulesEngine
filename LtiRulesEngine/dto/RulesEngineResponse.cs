namespace LtiRulesEngine.dto {
    public class RulesEngineResponse {
        public bool IsSuccess = false;
        public List<string> Messages = new List<string>();
        public string ExceptionMessage = string.Empty;
    }
}
