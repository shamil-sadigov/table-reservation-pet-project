using System;

namespace BuildingBlocks.Domain.DomainRules
{
    public class SystemTime
    {
        public static DateTime Now => DateTime.UtcNow;
    }
}