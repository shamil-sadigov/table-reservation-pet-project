#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace BuildingBlocks.Domain
{
    public static class NullChecker
    {
        public static bool TryCheckNullValues(object obj, out List<Error> errors)
        {
            errors = obj.GetType()
                .GetProperties()
                .Where(prop => prop.GetValue(obj) is null)
                .Select(prop => new Error($"'{prop.Name}' should no be null"))
                .ToList();

            return errors.Any();
        }
    }
}