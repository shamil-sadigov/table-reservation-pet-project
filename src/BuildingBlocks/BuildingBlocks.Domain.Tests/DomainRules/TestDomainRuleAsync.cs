#region

using System.Threading.Tasks;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.AsyncVersion;

#endregion

namespace BuildingBlocks.Domain.Tests.DomainRules
{
    public class TestDomainRuleAsync : IDomainRuleAsync
    {
        private readonly string _errorMessage;
        private readonly bool _isSuccessfulRule;

        public TestDomainRuleAsync(bool isSuccessfulRule, string errorMessage)
        {
            _isSuccessfulRule = isSuccessfulRule;
            _errorMessage = errorMessage;
        }

        public async Task<Result> CheckAsync()
        {
            await Task.Yield();

            return _isSuccessfulRule ? Result.Success() : new Error(_errorMessage);
        }
    }
}