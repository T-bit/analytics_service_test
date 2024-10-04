using System.Collections.Generic;

namespace TestEventService.Extensions
{
    public static class EnumerableExtensions
    {
        public static List<T> ToList<T>(this IEnumerable<T> self, List<T> list, bool append = false)
        {
            if (!append)
            {
                list.Clear();
            }

            list.AddRange(self);

            return list;
        }
    }
}