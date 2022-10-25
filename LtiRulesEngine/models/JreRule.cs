namespace LtiRulesEngine.models {
    public class JreRule {
        public string? Name { get; set; }
        public Conditions? Conditions { get; set; }
        public Action? Action { get; set; }
    }

    public class All {
        public string? Fact { get; set; }
        public string? Operator { get; set; }
        public string? Value { get; set; }
        public string? Path { get; set; }
    }

    public class Conditions {
        public List<All>? All { get; set; }
    }

    public class Action {
        public string? Type { get; set; }
    }


}
