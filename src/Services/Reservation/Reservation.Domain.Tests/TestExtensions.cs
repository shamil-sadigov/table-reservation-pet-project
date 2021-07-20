using System.Collections.Generic;
using System.Linq;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.BusinessRule;
using FluentAssertions;

namespace Reservation.Domain.Tests
{
    // TODO: move to shared project
    
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
        
        public static void ShouldContain(this IEnumerable<Error> errors, params string[] errorMessages)
        {
            errors.Select(x => x.Message)
                .Should()
                .Contain(errorMessages);
        }
    }
}