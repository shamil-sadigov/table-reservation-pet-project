using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;

namespace BuildingBlocks.Domain.BusinessRule
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

        public async Task<CheckResult> Check()
        {
            var rules = BusinessRules.Select(x => x.Check());
            
            CheckResult[] results = await Task.WhenAll(rules);
            
            var result =  CombineBusinessRulesResults(results);

            return result;
        }

        protected abstract CheckResult CombineBusinessRulesResults(CheckResult[] results);
    }
}