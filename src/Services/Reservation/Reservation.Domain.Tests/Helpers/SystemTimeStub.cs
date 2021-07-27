using BuildingBlocks.Domain.DomainRules;

namespace Reservation.Domain.Tests.Helpers
{
    public class SystemTimeStub:ISystemTime
    {
        public static readonly ISystemTime Instance = new SystemTimeStub();
    }
}