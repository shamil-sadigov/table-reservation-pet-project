#region

using System;

#endregion

namespace Restaurant.Tests.Shared
{
    public static class StringExtension
    {
        public static TimeSpan AsTimeSpan(this string str) => TimeSpan.Parse(str);
    }
}