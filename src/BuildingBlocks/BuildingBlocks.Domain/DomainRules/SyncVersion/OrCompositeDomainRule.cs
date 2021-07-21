#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace BuildingBlocks.Domain.DomainRules.SyncVersion
{
    public sealed class OrCompositeDomainRule : CompositeDomainRuleBase
    {
        public OrCompositeDomainRule(ICollection<IDomainRule> domainRules)
            : base(domainRules)
        {
        }

        protected override Result CombineDomainRulesResults(IEnumerable<Result> results)
        {
            var errors = new List<Error>();

            var anySuccessfulResultExist = results.Aggregate(seed: false, (isSuccessful, result) =>
            {
                if (result.Failed)
                    errors.AddRange(result.Errors!);

                return isSuccessful || result.Succeeded;
            });

            return anySuccessfulResultExist ? Result.Success() : Result.Failure(errors);
        }
    }
}