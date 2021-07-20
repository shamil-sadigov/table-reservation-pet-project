using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Domain.BusinessRule;
using BuildingBlocks.Domain.BusinessRule.AsyncVersion;
using FluentAssertions;
using Xunit;

namespace BuildingBlocks.Domain.Tests.BusinessRule
{
    public class OrCompositeBusinessRuleTests
    {
        [Fact]
        public async Task Simple_OrCompositeRule_is_successful()
        {
            var businessRule = new TestBusinessRuleAsync(isSuccessfulRule: true, "Error message1")
                    .Or(new TestBusinessRuleAsync(isSuccessfulRule: false, "Error message2"));
            
            var checkResult = await businessRule.Check();

            checkResult.ShouldSucceed();
        }
        
        [Fact]
        public async Task Complicated_OrCompositeRule_is_successful()
        {
            var compositeRule = 
                new TestBusinessRuleAsync(isSuccessfulRule: false, "Error message1")
                    .Or(new TestBusinessRuleAsync(isSuccessfulRule: false, "Error message2"))
                    .Or(new TestBusinessRuleAsync(isSuccessfulRule: false, "Error message3"))
                    .Or(new TestBusinessRuleAsync(isSuccessfulRule: true, "Error message4"))
                    .Or(new TestBusinessRuleAsync(isSuccessfulRule: false, "Error message5"));
            
            var checkResult = await compositeRule.Check();

            checkResult.ShouldSucceed();
        }

        
        [Fact]
        public async Task OrCompositeRule_fails()
        {
            var compositeRule = 
                new TestBusinessRuleAsync(isSuccessfulRule: false, errorMessage: "Error message1")
                    .Or(new TestBusinessRuleAsync(isSuccessfulRule: false, errorMessage: "Error message2"))
                    .Or(new TestBusinessRuleAsync(isSuccessfulRule: false, errorMessage: "Error message3"));
            
            var checkResult = await compositeRule.Check();

            checkResult.ShouldFail();

            checkResult.Errors!
                .Select(x => x.Message)
                .Should().HaveCount(3)
                .And.Contain("Error message1", "Error message2", "Error message3");
        }
    }
}