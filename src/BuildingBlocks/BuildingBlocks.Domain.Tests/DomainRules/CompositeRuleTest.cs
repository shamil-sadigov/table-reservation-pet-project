#region

using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Domain.DomainRules.AsyncVersion;
using BuildingBlocks.Tests.Shared;
using FluentAssertions;
using Xunit;

#endregion

namespace BuildingBlocks.Domain.Tests.DomainRules
{
    public class CompositeRuleTest
    {
        [Fact]
        public async Task Composite_rule_succeed()
        {
            var domainRule =
                new TestDomainRuleAsync(isSuccessfulRule: true, errorMessage: "Error message")
                    .And(new TestDomainRuleAsync(isSuccessfulRule: true, errorMessage: "Error message2"))
                    .Or(new TestDomainRuleAsync(isSuccessfulRule: false, errorMessage: "Error message3"))
                    .And(new TestDomainRuleAsync(isSuccessfulRule: true, errorMessage: "Error message4"))
                    .Or(new TestDomainRuleAsync(isSuccessfulRule: true, errorMessage: "Error message3"))
                    .Or(new TestDomainRuleAsync(isSuccessfulRule: false, errorMessage: "Error message3"))
                    .And(new TestDomainRuleAsync(isSuccessfulRule: true, errorMessage: "Error message6"));

            var checkResult = await domainRule.CheckAsync();

            checkResult.ShouldSucceed();
        }


        [Fact]
        public async Task Composite_rule_fails()
        {
            var domainRule =
                new TestDomainRuleAsync(isSuccessfulRule: true, errorMessage: "Error message")
                    .And(new TestDomainRuleAsync(isSuccessfulRule: false, errorMessage: "Error message2"))
                    .Or(new TestDomainRuleAsync(isSuccessfulRule: true, errorMessage: "Error message3"))
                    .And(new TestDomainRuleAsync(isSuccessfulRule: true, errorMessage: "Error message4"))
                    .Or(new TestDomainRuleAsync(isSuccessfulRule: true, errorMessage: "Error message3"))
                    .Or(new TestDomainRuleAsync(isSuccessfulRule: false, errorMessage: "Error message3"))
                    .And(new TestDomainRuleAsync(isSuccessfulRule: false, errorMessage: "Error message6"));

            var checkResult = await domainRule.CheckAsync();

            checkResult.ShouldFail();

            checkResult.Errors.Select(x => x.Message)
                .Should().HaveCount(1)
                .And
                .Contain("Error message6");
        }
    }
}