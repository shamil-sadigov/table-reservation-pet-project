using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Domain.BusinessRule;
using BuildingBlocks.Domain.BusinessRule.AsyncVersion;
using FluentAssertions;
using Xunit;

namespace BuildingBlocks.Domain.Tests.BusinessRule
{
    public class CompositeRuleTest
    {
        [Fact]
        public async Task Composite_rule_succeed()
        {
            var businessRule =
                new TestBusinessRuleAsync(isSuccessfulRule: true, errorMessage: "Error message")
                    .And(new TestBusinessRuleAsync(isSuccessfulRule: true, errorMessage: "Error message2"))
                    .Or(new TestBusinessRuleAsync(isSuccessfulRule: false, errorMessage: "Error message3"))
                    .And(new TestBusinessRuleAsync(isSuccessfulRule: true, errorMessage: "Error message4"))
                    .Or(new TestBusinessRuleAsync(isSuccessfulRule: true, errorMessage: "Error message3"))
                    .Or(new TestBusinessRuleAsync(isSuccessfulRule: false, errorMessage: "Error message3"))
                    .And(new TestBusinessRuleAsync(isSuccessfulRule: true, errorMessage: "Error message6"));

            var checkResult = await businessRule.Check();

            checkResult.ShouldSucceed();
        }

        
        [Fact]
        public async Task Composite_rule_fails()
        {
            var businessRule =
                new TestBusinessRuleAsync(isSuccessfulRule: true, errorMessage: "Error message")
                    .And(new TestBusinessRuleAsync(isSuccessfulRule: false, errorMessage: "Error message2"))
                    .Or(new TestBusinessRuleAsync(isSuccessfulRule: true, errorMessage: "Error message3"))
                    .And(new TestBusinessRuleAsync(isSuccessfulRule: true, errorMessage: "Error message4"))
                    .Or(new TestBusinessRuleAsync(isSuccessfulRule: true, errorMessage: "Error message3"))
                    .Or(new TestBusinessRuleAsync(isSuccessfulRule: false, errorMessage: "Error message3"))
                    .And(new TestBusinessRuleAsync(isSuccessfulRule: false, errorMessage: "Error message6"));

            var checkResult = await businessRule.Check();

            checkResult.ShouldFail();

            checkResult.Errors.Select(x=> x.Message)
                .Should().HaveCount(1)
                .And
                .Contain("Error message6");
        }
    }
}