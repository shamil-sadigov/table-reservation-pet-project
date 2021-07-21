#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace BuildingBlocks.Domain.DomainRules.SyncVersion
{
    public sealed class AndCompositeDomainRule : CompositeDomainRuleBase
    {
        public AndCompositeDomainRule(ICollection<IDomainRule> domainRules)
            : base(domainRules)
        {
        }

        protected override Result CombineDomainRulesResults(IEnumerable<Result> results)
        {
            var errors = new List<Error>();

            var allResultAreSuccessful = results.Aggregate(seed: true, (isSuccessful, result) =>
            {
                if (result.Failed)
                    errors.AddRange(result.Errors!);

                return isSuccessful && result.Succeeded;
            });

            return allResultAreSuccessful ? Result.Success() : Result.Failure(errors);
        }
    }
}