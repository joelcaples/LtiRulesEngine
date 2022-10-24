namespace LtiRulesEngine.dto {
    public class ColorRecipe {
        public string Recipe { get; set; } = string.Empty;
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    }
}
