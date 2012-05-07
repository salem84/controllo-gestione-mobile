using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlloGestione.Utility
{
    public static class ExtensionMethods
    {
        static int[] weights = { 60 * 60 * 1000, 60 * 1000, 1000, 1 };

        public static TimeSpan ToTimeSpan(this string s)
        {
            string[] parts = s.Split('.', ':');
            long ms = 0;
            for (int i = 0; i < parts.Length && i < weights.Length; i++)
                ms += Convert.ToInt64(parts[i]) * weights[i];
            return TimeSpan.FromMilliseconds(ms);
        }


        public static DateTime ToWestEuropeTime(this DateTime dt)
        {
            var currentUserTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            return TimeZoneInfo.ConvertTime(dt, currentUserTimeZoneInfo);
        }
    }
}