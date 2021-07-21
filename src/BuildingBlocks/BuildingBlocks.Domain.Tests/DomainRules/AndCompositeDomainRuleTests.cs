#region

using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Domain.DomainRules.AsyncVersion;
using FluentAssertions;
using Xunit;

#endregion

namespace BuildingBlocks.Domain.Tests.DomainRules
{
    public class AndCompositeDomainRuleTests
    {
        [Fact]
        public async Task Simple_AndCompositeRule_is_successful()
        {
            var domainRule =
                new TestDomainRuleAsync(isSuccessfulRule: true, "Error message1")
                    .And(new TestDomainRuleAsync(isSuccessfulRule: true, "Error message2"));

            var checkResult = await domainRule.Check();

            checkResult.ShouldSucceed();
        }

        [Fact]
        public async Task Complicated_AndCompositeRule_is_successful()
        {
            var compositeRule =
                new TestDomainRuleAsync(isSuccessfulRule: true, "Error message1")
                    .And(new TestDomainRuleAsync(isSuccessfulRule: true, "Error message2"))
                    .And(new TestDomainRuleAsync(isSuccessfulRule: true, "Error message3"))
                    .And(new TestDomainRuleAsync(isSuccessfulRule: true, "Error message4"))
                    .And(new TestDomainRuleAsync(isSuccessfulRule: true, "Error message5"));

            var checkResult = await compositeRule.Check();

            checkResult.ShouldSucceed();
        }


        [Fact]
        public async Task Simple_AndCompositeRule_fails()
        {
            var compositeRule =
                new TestDomainRuleAsync(isSuccessfulRule: true, errorMessage: "Error message1")
                    .And(new TestDomainRuleAsync(isSuccessfulRule: false, errorMessage: "Error message2"));

            var checkResult = await compositeRule.Check();

            checkResult.ShouldFail();

            checkResult.Errors!
                .Single()
                .Message.Should().Be("Error message2");
        }


        [Fact]
        public async Task Complicated_AndCompositeRule_fails()
        {
            var compositeRule =
                new TestDomainRuleAsync(isSuccessfulRule: false, errorMessage: "Error message1") // <--
                    .And(new TestDomainRuleAsync(isSuccessfulRule: true, errorMessage: "Error message2"))
                    .And(new TestDomainRuleAsync(isSuccessfulRule: false, errorMessage: "Error message3")) // <--
                    .And(new TestDomainRuleAsync(isSuccessfulRule: true, errorMessage: "Error message4"))
                    .And(new TestDomainRuleAsync(isSuccessfulRule: false, errorMessage: "Error message5")) // <--
                    .And(new TestDomainRuleAsync(isSuccessfulRule: false, errorMessage: "Error message6")) // <--
                    .And(new TestDomainRuleAsync(isSuccessfulRule: true, errorMessage: "Error message7"));

            var checkResult = await compositeRule.Check();

            checkResult.ShouldFail();

            checkResult.Errors!
                .Select(x => x.Message)
                .Should()
                .HaveCount(4)
                .And
                .Contain(new[]
                {
                    "Error message1",
                    "Error message3",
                    "Error message5",
                    "Error message6"
                });
        }
    }
}