using LtiRulesEngine.models;
using LtiRulesEngine.services;
using Xunit.Abstractions;

namespace LtiRulesEngine.Tests {

    public class PlatformTests {

        private readonly ITestOutputHelper output;

        public PlatformTests(ITestOutputHelper output) {
            this.output = output;
        }

        [Fact]
        public async void PctOwnershipExceedsThresholdCreditReleaseTrue() {
            var rulesService = new PlatformService();
            var result = await rulesService.Platform(new PlatformContext() {
                ContextType = "originations",
                CreditRelease = true,
                PctOwnership = 30
            });

            Assert.True(result.IsSuccess);

            output.WriteLine($"Validation Passed?: {result.IsSuccess}");
            result.Messages.ForEach(m => output.WriteLine(m));
        }

        [Fact]
        public async void PctOwnershipExceedsThresholdCreditReleaseFalse() {
            var rulesService = new PlatformService();
            var result = await rulesService.Platform(new PlatformContext() {
                ContextType = "originations",
                CreditRelease = false,
                PctOwnership = 30
            });

            Assert.False(result.IsSuccess);

            output.WriteLine($"Validation Passed?: {result.IsSuccess}");
            result.Messages.ForEach(m => output.WriteLine(m));
        }

        [Fact]
        public async void PctOwnershipDoesNotExceedThreshold() {
            var rulesService = new PlatformService();
            var result = await rulesService.Platform(new PlatformContext() {
                ContextType = "originations",
                CreditRelease = false,
                PctOwnership = 20
            });

            Assert.True(result.IsSuccess);

            output.WriteLine($"Validation Passed?: {result.IsSuccess}");
            result.Messages.ForEach(m => output.WriteLine(m));
        }

    }
}
