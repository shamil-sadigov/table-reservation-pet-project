using System.Linq;

namespace BuildingBlocks.Domain.BusinessRule.SyncVersion
{
    public static class BusinessRuleExtensions
    {
        public static IBusinessRule And(this IBusinessRule businessRuleAsync, params IBusinessRule[] businessRules)
        {
            var combinedRules = businessRules.Append(businessRuleAsync);

            return new AndCompositeBusinessRule(combinedRules.ToList());
        }
        
        public static IBusinessRule Or(this IBusinessRule businessRuleAsync, params IBusinessRule[] businessRules)
        {
            var combinedRules = businessRules.Append(businessRuleAsync);

            return new OrCompositeBusinessRule(combinedRules.ToList());
        }
    }
}