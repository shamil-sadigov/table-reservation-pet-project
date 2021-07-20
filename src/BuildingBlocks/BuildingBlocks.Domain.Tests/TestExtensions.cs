using BuildingBlocks.Domain.BusinessRule;
using FluentAssertions;

namespace BuildingBlocks.Domain.Tests.BusinessRule
{
    public static class TestExtensions
    {
        public static void ShouldSucceed(this Result result)
        {
            result.Succeeded.Should().Be(true);
            result.Failed.Should().Be(false);
            
        }
        
        public static void ShouldFail(this Result result)
        {
            result.Failed.Should().Be(true);
            result.Succeeded.Should().Be(false);
        }
    }
}