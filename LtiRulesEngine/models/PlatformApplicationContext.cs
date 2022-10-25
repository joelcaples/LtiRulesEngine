namespace LtiRulesEngine.models {
    public partial class PlatformContext {
        public bool IsDeclined { get; set; } = false;
        public List<string> DeclineReasons { get; set; } = new List<string>();
    }
}
