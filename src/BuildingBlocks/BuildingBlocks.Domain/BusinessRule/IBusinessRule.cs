#region

#endregion

using System.Threading.Tasks;

namespace BuildingBlocks.Domain.BusinessRule
{
    public interface IBusinessRule
    {
        Task<CheckResult> Check();
    }
}