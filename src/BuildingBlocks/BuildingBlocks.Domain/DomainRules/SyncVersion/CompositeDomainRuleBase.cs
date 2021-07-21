#region

using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

#endregion

namespace BuildingBlocks.Domain.DomainRules.SyncVersion
{
    public abstract class CompositeDomainRuleBase : IDomainRule
    {
        protected CompositeDomainRuleBase(ICollection<IDomainRule> domainRules)
        {
            DomainRules = domainRules ?? throw new ArgumentNullException(nameof(domainRules));

            if (!domainRules.AtLeast(2))
                throw new ArgumentException("Should have at least two elements", nameof(domainRules));
        }

        protected ICollection<IDomainRule> DomainRules { get; }

        public Result Check()
        {
            var rules = DomainRules.Select(x => x.Check());
            var result = CombineDomainRulesResults(rules);
            return result;
        }

        protected abstract Result CombineDomainRulesResults(IEnumerable<Result> results);
    }
}