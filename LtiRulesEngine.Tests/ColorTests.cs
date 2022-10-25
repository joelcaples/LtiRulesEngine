using LtiRulesEngine.models;
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
                Ingredients = new List<Ingredient>() {
                    new Ingredient() { Color = "blue", Pct = 50 },
                    new Ingredient() { Color = "green", Pct = 50 }
                }
            });

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Messages, msg => msg.Contains("Red is required for Orange"));
            Assert.Contains(result.Messages, msg => msg.Contains("Yellow is required for Orange"));
            Assert.Contains(result.Messages, msg => msg.Contains("Blue is not allowed for Orange"));

            output.WriteLine($"Validation Passed?: {result.IsSuccess}");
            result.Messages.ForEach(m => output.WriteLine(m));
        }

        [Fact]
        public async void OrangeTest() {
            var rulesService = new RulesService();
            var result = await rulesService.Colors(new ColorRecipe() {
                Recipe = "orange",
                Ingredients = new List<Ingredient>() {
                    new Ingredient() { Color = "red", Pct = 50 },
                    new Ingredient() { Color = "yellow", Pct = 50 }
                }
            });

            output.WriteLine($"Validation Passed?: {result.IsSuccess}");
            result.Messages.ForEach(m => output.WriteLine(m));

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void MinimumPctTest() {
            var rulesService = new RulesService();
            var result = await rulesService.Colors(new ColorRecipe() {
                Recipe = "pink",
                Ingredients = new List<Ingredient>() {
                    new Ingredient() { Color = "red", Pct = 80 },
                    new Ingredient() { Color = "white", Pct = 20 }
                }
            });

            Assert.True(result.IsSuccess);

            output.WriteLine($"Validation Passed?: {result.IsSuccess}");
            result.Messages.ForEach(m => output.WriteLine(m));
        }

        [Fact]
        public async void MinimumPctFailTest() {
            var rulesService = new RulesService();
            var result = await rulesService.Colors(new ColorRecipe() {
                Recipe = "pink",
                Ingredients = new List<Ingredient>() {
                    new Ingredient() { Color = "red", Pct = 90 },
                    new Ingredient() { Color = "white", Pct = 10 }
                }
            });

            Assert.False(result.IsSuccess);

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

        [Fact]
        public async void MustTotal100PctTest() {
            var rulesService = new RulesService();
            var result = await rulesService.Colors(new ColorRecipe() {
                Recipe = "orange",
                Ingredients = new List<Ingredient>() {
                    new Ingredient() { Color = "blue", Pct = 10 },
                    new Ingredient() { Color = "green", Pct = 250 }
                }
            });

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Messages, msg => msg.Contains("Percentages did not total 100%"));

            output.WriteLine($"Validation Passed?: {result.IsSuccess}");
            result.Messages.ForEach(m => output.WriteLine(m));
        }
    }
}