using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Domain.BusinessRule.AsyncVersion
{
    public sealed class AndCompositeBusinessRuleAsync:CompositeBusinessRuleAsyncBase
    {
        public AndCompositeBusinessRuleAsync(ICollection<IBusinessRuleAsync> businessRules)
            : base(businessRules){ }
        
        protected override Result CombineBusinessRulesResults(Result[] results)
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