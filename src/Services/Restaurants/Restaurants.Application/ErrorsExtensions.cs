#region

using System.Collections.Generic;
using System.Linq;
using BuildingBlocks.Domain;
using MoreLinq;

#endregion

namespace Restaurants.Application
{
    public static class ErrorsExtensions
    {
        public static string Stringify(this IEnumerable<Error> errors)
            => errors.Select(x => x.Message).ToDelimitedString(",");
    }
}