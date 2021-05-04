using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToTimestamp(this DateTime dateTime)
        {
            var epoch = new DateTime(1970, 1, 1);
            var time = dateTime.Subtract(epoch);

            return (long) time.TotalSeconds;
        }
    }
}
