#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace BuildingBlocks.Domain.DomainRules.AsyncVersion
{
    public sealed class AndCompositeDomainRuleAsync : CompositeDomainRuleAsyncBase
    {
        public AndCompositeDomainRuleAsync(ICollection<IDomainRuleAsync> domainRule)
            : base(domainRule)
        {
        }

        protected override Result CombineDomainRulesResults(Result[] results)
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