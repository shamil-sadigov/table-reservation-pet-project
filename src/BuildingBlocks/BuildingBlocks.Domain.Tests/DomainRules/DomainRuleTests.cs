#region

using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Tests.Shared;
using FluentAssertions;
using Xunit;

#endregion

namespace BuildingBlocks.Domain.Tests.DomainRules
{
    public class DomainRuleTests
    {
        [Fact]
        public async Task DomainRule_check_is_successful()
        {
            var domainRule = new TestDomainRuleAsync(
                isSuccessfulRule: true,
                errorMessage: "Error message");

            var result = await domainRule.CheckAsync();

            result.ShouldSucceed();
        }

        [Fact]
        public async Task DomainRule_check_is_failed()
        {
            var domainRule = new TestDomainRuleAsync(
                isSuccessfulRule: false,
                errorMessage: "Error message");

            var checkResult = await domainRule.CheckAsync();

            checkResult.ShouldFail();

            checkResult.Errors!
                .Should()
                .HaveCount(1)
                .And
                .Subject.Single().Message
                .Should().Be("Error message");
        }
    }
}