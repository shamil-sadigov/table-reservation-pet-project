using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingBlocks.Domain.BusinessRule
{
    public sealed class AndCompositeBusinessRule:CompositeBusinessRuleBase
    {
        public AndCompositeBusinessRule(ICollection<IBusinessRule> businessRules)
            : base(businessRules){ }
        
        protected override CheckResult CombineBusinessRulesResults(CheckResult[] results)
        {
            var errors = new List<Error>();
            
            var allResultAreSuccessful = results.Aggregate(seed: true, (isSuccessful, result) =>
            {
                if (result.Failed)
                    errors.AddRange(result.Errors!);

                return isSuccessful && result.Succeeded;
            });
            
            return allResultAreSuccessful ? CheckResult.Success() : CheckResult.Failure(errors);
        }
    }
}