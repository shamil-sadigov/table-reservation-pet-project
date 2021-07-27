using System;

namespace BuildingBlocks.Domain.DomainRules
{
    public interface ISystemTime
    {
        public DateTime DateNow => DateTime.Today;
        public DateTime DateTimeNow => DateTime.UtcNow;
    }
}