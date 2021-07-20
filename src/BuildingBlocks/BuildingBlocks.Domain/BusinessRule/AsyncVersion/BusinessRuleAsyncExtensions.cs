using System.Linq;

namespace BuildingBlocks.Domain.BusinessRule.AsyncVersion
{
    public static class BusinessRuleAsyncExtensions
    {
        public static IBusinessRuleAsync And(this IBusinessRuleAsync businessRuleAsync, params IBusinessRuleAsync[] businessRules)
        {
            var combinedRules = businessRules.Append(businessRuleAsync);

            return new AndCompositeBusinessRuleAsync(combinedRules.ToList());
        }
        
        public static IBusinessRuleAsync Or(this IBusinessRuleAsync businessRuleAsync, params IBusinessRuleAsync[] businessRules)
        {
            var combinedRules = businessRules.Append(businessRuleAsync);

            return new OrCompositeBusinessRuleAsync(combinedRules.ToList());
        }
    }
}