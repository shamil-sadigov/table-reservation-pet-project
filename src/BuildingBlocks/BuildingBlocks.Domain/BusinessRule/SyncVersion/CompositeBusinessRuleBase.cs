using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace BuildingBlocks.Domain.BusinessRule.SyncVersion
{
    public abstract class CompositeBusinessRuleBase:IBusinessRule
    {
        protected ICollection<IBusinessRule> BusinessRules { get;  }

        protected CompositeBusinessRuleBase(ICollection<IBusinessRule> businessRules)
        {
            BusinessRules = businessRules ?? throw new ArgumentNullException(nameof(businessRules));

            if (!businessRules.AtLeast(2))
            {
                throw new ArgumentException("Should have at least two elements", nameof(businessRules));
            }
        }

        public Result Check()
        {
            var rules = BusinessRules.Select(x => x.Check());
            var result =  CombineBusinessRulesResults(rules);
            return result;
        }

        protected abstract Result CombineBusinessRulesResults(IEnumerable<Result> results);
    }
}