using Xunit.Abstractions;

namespace LtiRulesEngine.Tests {
    public class ColorTests {

        private readonly ITestOutputHelper output;

        public ColorTests(ITestOutputHelper output) {
            this.output = output;
        }

        [Fact]
        public async void Test1() {
            var rulesService = new RulesService();
            var result = await rulesService.Colors(getJson("test-01.json"));

            Assert.False(result.IsSuccess);
            Assert.Collection(result.Messages, msg => msg.Contains("Red was not added"));

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