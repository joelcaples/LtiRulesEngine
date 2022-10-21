namespace LtiRulesEngine.Tests {
    public class ColorTests {
        [Fact]
        public async void Test1() {
            var rulesService = new RulesService();
            var result = await rulesService.Colors(getJson("test-01.json"));
            Console.WriteLine(result);
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