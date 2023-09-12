using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager_Services.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToTimeStamp(this DateTime date)
        {
            DateTime epochTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan t = (date.ToUniversalTime() - epochTime);
            return (long)t.TotalMilliseconds;
        }
    }
}
