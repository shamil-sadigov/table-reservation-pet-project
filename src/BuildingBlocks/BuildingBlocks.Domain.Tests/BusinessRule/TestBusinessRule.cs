using System.Threading.Tasks;
using BuildingBlocks.Domain.BusinessRule;

namespace BuildingBlocks.Domain.Tests.BusinessRule
{
    public class TestBusinessRule:IBusinessRule
    {
        private readonly bool _isSuccessfulRule;
        private readonly string _errorMessage;

        public TestBusinessRule(bool isSuccessfulRule, string errorMessage)
        {
            _isSuccessfulRule = isSuccessfulRule;
            _errorMessage = errorMessage;
        }
        
        public async Task<CheckResult> Check()
        {
            await Task.Yield();
            
            if (_isSuccessfulRule)
                return CheckResult.Success();

            var error = new Error(_errorMessage);
            
            return CheckResult.Failure(error);
        }
        
        
    }
}