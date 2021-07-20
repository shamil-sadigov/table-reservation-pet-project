using System;
using System.Linq;

namespace BuildingBlocks.Domain.BusinessRule
{
    public static class BusinessRuleExtensions
    {
        public static IBusinessRule And(this IBusinessRule businessRule, params IBusinessRule[] businessRules)
        {
            var combinedRules = businessRules.Append(businessRule);

            return new AndCompositeBusinessRule(combinedRules.ToList());
        }
        
        public static IBusinessRule Or(this IBusinessRule businessRule, params IBusinessRule[] businessRules)
        {
            var combinedRules = businessRules.Append(businessRule);

            return new OrCompositeBusinessRule(combinedRules.ToList());
        }
    }
}