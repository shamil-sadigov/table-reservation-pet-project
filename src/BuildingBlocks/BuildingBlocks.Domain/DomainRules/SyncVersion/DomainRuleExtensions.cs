#region

using System.Linq;

#endregion

namespace BuildingBlocks.Domain.DomainRules.SyncVersion
{
    public static class DomainRuleExtensions
    {
        public static IDomainRule And(this IDomainRule domainRuleAsync, params IDomainRule[] domainRules)
        {
            var combinedRules = domainRules.Append(domainRuleAsync);

            return new AndCompositeDomainRule(combinedRules.ToList());
        }

        public static IDomainRule Or(this IDomainRule domainRuleAsync, params IDomainRule[] domainRules)
        {
            var combinedRules = domainRules.Append(domainRuleAsync);

            return new OrCompositeDomainRule(combinedRules.ToList());
        }
    }
}