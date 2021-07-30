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
    public class OrCompositeDomainRuleTests
    {
        [Fact]
        public async Task Simple_OrCompositeRule_is_successful()
        {
            var domainRule = new TestDomainRuleAsync(isSuccessfulRule: true, "Error message1")
                .Or(new TestDomainRuleAsync(isSuccessfulRule: false, "Error message2"));

            var checkResult = await domainRule.CheckAsync();

            checkResult.ShouldSucceed();
        }

        [Fact]
        public async Task Complicated_OrCompositeRule_is_successful()
        {
            var compositeRule =
                new TestDomainRuleAsync(isSuccessfulRule: false, "Error message1")
                    .Or(new TestDomainRuleAsync(isSuccessfulRule: false, "Error message2"))
                    .Or(new TestDomainRuleAsync(isSuccessfulRule: false, "Error message3"))
                    .Or(new TestDomainRuleAsync(isSuccessfulRule: true, "Error message4"))
                    .Or(new TestDomainRuleAsync(isSuccessfulRule: false, "Error message5"));

            var checkResult = await compositeRule.CheckAsync();

            checkResult.ShouldSucceed();
        }


        [Fact]
        public async Task OrCompositeRule_fails()
        {
            var compositeRule =
                new TestDomainRuleAsync(isSuccessfulRule: false, errorMessage: "Error message1")
                    .Or(new TestDomainRuleAsync(isSuccessfulRule: false, errorMessage: "Error message2"))
                    .Or(new TestDomainRuleAsync(isSuccessfulRule: false, errorMessage: "Error message3"));

            var checkResult = await compositeRule.CheckAsync();

            checkResult.ShouldFail();

            checkResult.Errors!
                .Select(x => x.Message)
                .Should().HaveCount(3)
                .And.Contain("Error message1", "Error message2", "Error message3");
        }
    }
}