namespace LtiRulesEngine.dto {
    public class OriginationsContext {
        public string ContextType { get; set; } = "Undefined";
        public bool CreditRelease { get; set; } = false;
        public int? PctOwnership { get; set; } = null;
    }
}
