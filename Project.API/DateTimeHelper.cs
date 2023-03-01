using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.API
{
    public class DateTimeHelper
    {
        public static DateTime KeepOnlyYearMonthDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        public static IList<DateTime> KeepOnlyYearMonthDay(IList<DateTime> dates)
        {
            return (from date in dates select KeepOnlyYearMonthDay(date)).ToList();
        }
    }
}
