#region

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MoreLinq;
using Xunit;
using Xunit.Abstractions;

#endregion

namespace BuildingBlocks.Domain.Tests.BusinessRule
{
    public class BusinessRuleTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public BusinessRuleTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task BusinessRule_check_is_successful()
        {
            var businessRule = new TestBusinessRuleAsync(
                isSuccessfulRule: true,
                errorMessage: "Error message");

            var result = await businessRule.Check();

            result.ShouldSucceed();
        }

        [Fact]
        public async Task BusinessRule_check_is_failed()
        {
            var businessRule = new TestBusinessRuleAsync(
                isSuccessfulRule: false,
                errorMessage: "Error message");

            var checkResult = await businessRule.Check();

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