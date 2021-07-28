using System;
using System.Collections.Generic;
using System.Linq;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using FluentAssertions;
using MoreLinq;

namespace Reservation.Domain.Tests.Helpers
{
    public static class ResultTessExtensions
    {
        public static void ShouldSucceed(this Result result)
        {
            string becauseMessage = string.Empty;
            
            if (result.Failed) 
                becauseMessage = result.Errors.Select(x => x.Message).ToDelimitedString("\n");
            
            result.Succeeded.Should().Be(true, becauseMessage);
            result.Failed.Should().Be(false, becauseMessage);
        }

        public static void ShouldFail(this Result result)
        {
            string becauseMessage = string.Empty;
            
            if (result.Failed) 
                becauseMessage = result.Errors.Select(x => x.Message).ToDelimitedString("\n");
            
            result.Failed.Should().Be(true, becauseMessage);
            result.Succeeded.Should().Be(false, becauseMessage);
        }
        
        public static void ShouldContainSomethingLike(this IEnumerable<Error> errors, string errorMessages)
        {
            errors.Select(x => x.Message)
                .Should()
                .ContainMatch(errorMessages);
        }
        
        
        public static void ThrowIfNotSuccessful(this Result result)
        {
            if (result.Failed)
            {
                var errors = result.Errors!.Select(x=> x.Message).ToDelimitedString("\n");
                throw new InvalidOperationException(errors);
            }
        }

    }
}