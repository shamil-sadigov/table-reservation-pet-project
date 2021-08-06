using System;

namespace Restaurants.Domain.Tests.Helpers
{
    public static class StringExtension
    {
        public static TimeSpan AsTimeSpan(this string str) => TimeSpan.Parse(str);
    }
}