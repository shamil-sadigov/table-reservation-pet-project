using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingBlocks.Domain.BusinessRule
{
    public sealed class OrCompositeBusinessRule:CompositeBusinessRuleBase
    {
        public OrCompositeBusinessRule(ICollection<IBusinessRule> businessRules) 
            :base(businessRules) {}
        
        protected override CheckResult CombineBusinessRulesResults(CheckResult[] results)
        {
            var errors = new List<Error>();
            
            var anySuccessfulResultExist = results.Aggregate(seed: false, (isSuccessful, result) =>
            {
                if (result.Failed)
                    errors.AddRange(result.Errors!);

                return isSuccessful || result.Succeeded;
            });
            
            return anySuccessfulResultExist ? CheckResult.Success() : CheckResult.Failure(errors);
        }
    }
}

