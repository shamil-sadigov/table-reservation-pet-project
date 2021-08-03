#region

using System;

#endregion

namespace BuildingBlocks.Domain.DomainRules
{
    public static class SystemClock
    {
        private static DateTime? _customDate;

        public static DateTime DateTimeNow => _customDate ?? DateTime.UtcNow;
        public static DateTime DateNow => _customDate?.Date ?? DateTime.UtcNow.Date;

        public static void Set(DateTime customDate) => _customDate = customDate;
        public static void Reset() => _customDate = null;
    }
}