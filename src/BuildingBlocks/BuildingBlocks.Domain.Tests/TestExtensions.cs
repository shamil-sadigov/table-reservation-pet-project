#region

using BuildingBlocks.Domain.DomainRules;
using FluentAssertions;

#endregion

namespace BuildingBlocks.Domain.Tests
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