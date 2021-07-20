using System.Threading.Tasks;
using BuildingBlocks.Domain.BusinessRule;
using BuildingBlocks.Domain.BusinessRule.AsyncVersion;

namespace BuildingBlocks.Domain.Tests.BusinessRule
{
    public class TestBusinessRuleAsync:IBusinessRuleAsync
    {
        private readonly bool _isSuccessfulRule;
        private readonly string _errorMessage;

        public TestBusinessRuleAsync(bool isSuccessfulRule, string errorMessage)
        {
            _isSuccessfulRule = isSuccessfulRule;
            _errorMessage = errorMessage;
        }
        
        public async Task<Result> Check()
        {
            await Task.Yield();
            
            if (_isSuccessfulRule)
                return Result.Success();

            var error = new Error(_errorMessage);
            
            return Result.Failure(error);
        }
        
        
    }
}