using System;

namespace Reservation.Domain.Tests
{
    public static class StringExtension
    {
        public static TimeSpan AsTimeSpan(this string str) => TimeSpan.Parse(str);
    }
}