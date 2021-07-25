#region

using System.Threading.Tasks;

#endregion

namespace BuildingBlocks.Domain.DomainRules.AsyncVersion
{
    public interface IDomainRuleAsync
    {
        Task<Result> CheckAsync();
    }
}