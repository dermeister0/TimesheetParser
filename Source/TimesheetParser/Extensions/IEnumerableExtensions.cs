using System.Collections.Generic;

namespace TimesheetParser.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> SelectOdds<T>(this IEnumerable<T> enumerable)
        {
            var odd = false;

            foreach (var item in enumerable)
            {
                if (odd)
                    yield return item;

                odd = !odd;
            }
        }
    }
}