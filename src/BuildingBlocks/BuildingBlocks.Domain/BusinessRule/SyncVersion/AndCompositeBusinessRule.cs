using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Domain.BusinessRule.SyncVersion
{
    public sealed class AndCompositeBusinessRule:CompositeBusinessRuleBase
    {
        public AndCompositeBusinessRule(ICollection<IBusinessRule> businessRules)
            : base(businessRules){ }
        
        protected override Result CombineBusinessRulesResults(IEnumerable<Result> results)
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