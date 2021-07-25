#region

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

#endregion

namespace BuildingBlocks.Domain.Tests.DomainRules
{
    public class DomainRuleTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public DomainRuleTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

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