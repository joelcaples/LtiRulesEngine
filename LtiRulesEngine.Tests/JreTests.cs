using LtiRulesEngine.models;
using LtiRulesEngine.services;
using Xunit.Abstractions;

namespace LtiRulesEngine.Tests {

    public class JreTests {

        private readonly ITestOutputHelper output;

        public JreTests(ITestOutputHelper output) {
            this.output = output;
        }

        [Fact]
        public async void JreTestPass() {
            var rulesService = new JreService();
            var data = "{ \"data\" : \"bar\" }";
            var jreRules = new List<string>() { getJson("jre-rules.json") };
            var result = await rulesService.JreRules(data, jreRules);

            Assert.True(result.IsSuccess);

            output.WriteLine($"Validation Passed?: {result.IsSuccess}");
            result.Messages.ForEach(m => output.WriteLine(m));
        }

        [Fact]
        public async void JreTestFail() {
            var rulesService = new JreService();
            var data = "{ \"data\" : \"notbar\" }";
            var jreRules = new List<string>() { getJson("jre-rules.json") };
            var result = await rulesService.JreRules(data, jreRules);

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
    }
}
