#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace BuildingBlocks.Domain.DomainRules.AsyncVersion
{
    public sealed class OrCompositeDomainRuleAsync : CompositeDomainRuleAsyncBase
    {
        public OrCompositeDomainRuleAsync(ICollection<IDomainRuleAsync> domainRule)
            : base(domainRule)
        {
        }

        protected override Result CombineDomainRulesResults(Result[] results)
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