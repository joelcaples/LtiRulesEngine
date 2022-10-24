using LtiRulesEngine.dto;
using Xunit.Abstractions;

namespace LtiRulesEngine.Tests {
    public class ColorTests {

        private readonly ITestOutputHelper output;

        public ColorTests(ITestOutputHelper output) {
            this.output = output;
        }

        [Fact]
        public async void InvalidIngredientsFromJsonTest() {
            var rulesService = new RulesService();
            var result = await rulesService.Colors(getJson("test-01.json"));

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Messages, msg => msg.Contains("Red is required for Orange"));
            Assert.Contains(result.Messages, msg => msg.Contains("Yellow is required for Orange"));
            Assert.Contains(result.Messages, msg => msg.Contains("Blue is not allowed for Orange"));

            output.WriteLine($"Validation Passed?: {result.IsSuccess}");
            result.Messages.ForEach(m => output.WriteLine(m));
        }

        [Fact]
        public async void InvalidIngredientsTest() {
            var rulesService = new RulesService();
            var result = await rulesService.Colors(new ColorRecipe() {
                Recipe = "orange",
                Ingredients = new List<string>() {
                    "blue",
                    "green"
                }
            });

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Messages, msg => msg.Contains("Red is required for Orange"));
            Assert.Contains(result.Messages, msg => msg.Contains("Yellow is required for Orange"));
            Assert.Contains(result.Messages, msg => msg.Contains("Blue is not allowed for Orange"));

            output.WriteLine($"Validation Passed?: {result.IsSuccess}");
            result.Messages.ForEach(m => output.WriteLine(m));
        }

        private string getJson(string dataFileName) {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), dataFileName, SearchOption.AllDirectories);
            if (files == null || files.Length == 0)
                throw new Exception("Rules not found.");
            var fileData = File.ReadAllText(files[0]);
            return fileData;
        }
    }
}