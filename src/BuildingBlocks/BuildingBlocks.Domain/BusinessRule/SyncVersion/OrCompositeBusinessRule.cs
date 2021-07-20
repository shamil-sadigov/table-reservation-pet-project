using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Domain.BusinessRule.SyncVersion
{
    public sealed class OrCompositeBusinessRule:CompositeBusinessRuleBase
    {
        public OrCompositeBusinessRule(ICollection<IBusinessRule> businessRules) 
            :base(businessRules) {}
        
        protected override Result CombineBusinessRulesResults(IEnumerable<Result> results)
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

