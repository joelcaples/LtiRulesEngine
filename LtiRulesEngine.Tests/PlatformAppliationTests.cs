using LtiRulesEngine.models;
using LtiRulesEngine.services;
using Xunit.Abstractions;

namespace LtiRulesEngine.Tests {

    public class PlatformApplicationTests {

        private readonly ITestOutputHelper output;

        public PlatformApplicationTests(ITestOutputHelper output) {
            this.output = output;
        }

        [Fact]
        public async void ApplicationDeclinedReasonsWereIncluded() {
            var rulesService = new PlatformService();
            var result = await rulesService.Platform(new PlatformContext() {
                ContextType = "originations",
                IsDeclined = true,
                DeclineReasons = new List<string>() {
                    "The applicant had the most terrrible credit score ever."
                }
            });

            Assert.True(result.IsSuccess);

            output.WriteLine($"Validation Passed?: {result.IsSuccess}");
            result.Messages.ForEach(m => output.WriteLine(m));
        }

        [Fact]
        public async void ApplicationDeclinedReasonsWereNotIncluded() {
            var rulesService = new PlatformService();
            var result = await rulesService.Platform(new PlatformContext() {
                ContextType = "originations",
                IsDeclined = true,
                DeclineReasons = new List<string>() {
                }
            });

            Assert.False(result.IsSuccess);

            output.WriteLine($"Validation Passed?: {result.IsSuccess}");
            result.Messages.ForEach(m => output.WriteLine(m));
        }

        [Fact]
        public async void ApplicationAccepted() {
            var rulesService = new PlatformService();
            var result = await rulesService.Platform(new PlatformContext() {
                ContextType = "originations",
                IsDeclined = false,
                DeclineReasons = new List<string>() {
                }
            });

            Assert.True(result.IsSuccess);

            output.WriteLine($"Validation Passed?: {result.IsSuccess}");
            result.Messages.ForEach(m => output.WriteLine(m));
        }

    }
}
