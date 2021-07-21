#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;

#endregion

namespace BuildingBlocks.Domain.DomainRules.AsyncVersion
{
    public abstract class CompositeDomainRuleAsyncBase : IDomainRuleAsync
    {
        protected CompositeDomainRuleAsyncBase(ICollection<IDomainRuleAsync> domainRule)
        {
            DomainRules = domainRule ?? throw new ArgumentNullException(nameof(domainRule));

            if (!domainRule.AtLeast(2))
                throw new ArgumentException("Should have at least two elements", nameof(domainRule));
        }

        protected ICollection<IDomainRuleAsync> DomainRules { get; }

        public async Task<Result> Check()
        {
            var rules = DomainRules.Select(x => x.Check());

            Result[] results = await Task.WhenAll(rules);

            var result = CombineDomainRulesResults(results);

            return result;
        }

        protected abstract Result CombineDomainRulesResults(Result[] results);
    }
}