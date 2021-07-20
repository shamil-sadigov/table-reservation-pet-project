using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Domain.BusinessRule.AsyncVersion
{
    public sealed class OrCompositeBusinessRuleAsync:CompositeBusinessRuleAsyncBase
    {
        public OrCompositeBusinessRuleAsync(ICollection<IBusinessRuleAsync> businessRules) 
            :base(businessRules) {}
        
        protected override Result CombineBusinessRulesResults(Result[] results)
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

