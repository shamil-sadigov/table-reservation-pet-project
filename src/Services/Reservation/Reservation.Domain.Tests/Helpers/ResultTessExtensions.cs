using System.Collections.Generic;
using System.Linq;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using FluentAssertions;

namespace Reservation.Domain.Tests.Helpers
{
    public static class ResultTessExtensions
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

        
        public static void ShouldContainSomethingLike(this IEnumerable<Error> errors, string errorMessages)
        {
            errors.Select(x => x.Message)
                .Should()
                .ContainMatch(errorMessages);
        }

    }
}