#region

using System.Linq;

#endregion

namespace BuildingBlocks.Domain.DomainRules.AsyncVersion
{
    public static class DomainRuleAsyncExtensions
    {
        public static IDomainRuleAsync And(this IDomainRuleAsync domainRuleAsync,
            params IDomainRuleAsync[] domainRules)
        {
            var combinedRules = domainRules.Append(domainRuleAsync);

            return new AndCompositeDomainRuleAsync(combinedRules.ToList());
        }

        public static IDomainRuleAsync Or(this IDomainRuleAsync domainRuleAsync,
            params IDomainRuleAsync[] domainRules)
        {
            var combinedRules = domainRules.Append(domainRuleAsync);

            return new OrCompositeDomainRuleAsync(combinedRules.ToList());
        }
    }
}