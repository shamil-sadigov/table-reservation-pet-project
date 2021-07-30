#region

using BuildingBlocks.Domain.DomainRules;

#endregion

namespace BuildingBlocks.Domain.Tests
{
    public class SystemTimeStub : ISystemTime
    {
        public static readonly ISystemTime Instance = new SystemTimeStub();
    }
}