using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;

namespace BuildingBlocks.Domain.BusinessRule.AsyncVersion
{
    public abstract class CompositeBusinessRuleAsyncBase:IBusinessRuleAsync
    {
        protected ICollection<IBusinessRuleAsync> BusinessRules { get;  }

        protected CompositeBusinessRuleAsyncBase(ICollection<IBusinessRuleAsync> businessRules)
        {
            BusinessRules = businessRules ?? throw new ArgumentNullException(nameof(businessRules));

            if (!businessRules.AtLeast(2))
            {
                throw new ArgumentException("Should have at least two elements", nameof(businessRules));
            }
        }

        public async Task<Result> Check()
        {
            var rules = BusinessRules.Select(x => x.Check());
            
            Result[] results = await Task.WhenAll(rules);
            
            var result =  CombineBusinessRulesResults(results);

            return result;
        }

        protected abstract Result CombineBusinessRulesResults(Result[] results);
    }
}