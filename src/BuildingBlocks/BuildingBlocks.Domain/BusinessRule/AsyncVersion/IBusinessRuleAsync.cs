using System.Threading.Tasks;

namespace BuildingBlocks.Domain.BusinessRule.AsyncVersion
{
    public interface IBusinessRuleAsync
    {
        Task<Result> Check();
    }
}