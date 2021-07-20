using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Domain.BusinessRule;
using FluentAssertions;
using Xunit;

namespace BuildingBlocks.Domain.Tests.BusinessRule
{
    public class AndCompositeBusinessRuleTests
    {
        [Fact]
        public async Task Simple_AndCompositeRule_is_successful()
        {
            var businessRule = 
                new TestBusinessRule(isSuccessfulRule: true, "Error message1")
                    .And(new TestBusinessRule(isSuccessfulRule: true, "Error message2"));
            
            var checkResult = await businessRule.Check();

            checkResult.ShouldSucceed();
        }
        
        [Fact]
        public async Task Complicated_AndCompositeRule_is_successful()
        {
            var compositeRule = 
                new TestBusinessRule(isSuccessfulRule: true, "Error message1")
                    .And(new TestBusinessRule(isSuccessfulRule: true, "Error message2"))
                    .And(new TestBusinessRule(isSuccessfulRule: true, "Error message3"))
                    .And(new TestBusinessRule(isSuccessfulRule: true, "Error message4"))
                    .And(new TestBusinessRule(isSuccessfulRule: true, "Error message5"));
            
            var checkResult = await compositeRule.Check();

            checkResult.ShouldSucceed();
        }

        
        [Fact]
        public async Task Simple_AndCompositeRule_fails()
        {
            var compositeRule = 
                new TestBusinessRule(isSuccessfulRule: true, errorMessage: "Error message1")
                    .And(new TestBusinessRule(isSuccessfulRule: false, errorMessage: "Error message2"));
            
            var checkResult = await compositeRule.Check();

            checkResult.ShouldFail();

            checkResult.Errors!
                .Single()
                .Message.Should().Be("Error message2");
        }
        
        
        [Fact]
        public async Task Complicated_AndCompositeRule_fails()
        { 
            var compositeRule = 
                     new TestBusinessRule(isSuccessfulRule: false, errorMessage: "Error message1")  // <--
                .And(new TestBusinessRule(isSuccessfulRule: true, errorMessage: "Error message2")) 
                .And(new TestBusinessRule(isSuccessfulRule: false, errorMessage: "Error message3")) // <--
                .And(new TestBusinessRule(isSuccessfulRule: true, errorMessage: "Error message4"))
                .And(new TestBusinessRule(isSuccessfulRule: false, errorMessage: "Error message5")) // <--
                .And(new TestBusinessRule(isSuccessfulRule: false, errorMessage: "Error message6")) // <--
                .And(new TestBusinessRule(isSuccessfulRule: true, errorMessage: "Error message7"));
            
            var checkResult = await compositeRule.Check();

            checkResult.ShouldFail();

            checkResult.Errors!
                .Select(x => x.Message)
                .Should()
                .HaveCount(4)
            .And
            .Contain(new []
            {
                "Error message1", 
                "Error message3",
                "Error message5",
                "Error message6"
            });
        }


    }
}