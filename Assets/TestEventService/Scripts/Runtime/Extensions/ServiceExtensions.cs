using System.Collections.Generic;
using TestEventService.Services;

namespace TestEventService.Extensions
{
    public static class ServiceExtensions
    {
        public static void Initialize(this IEnumerable<IService> self)
        {
            foreach (var service in self)
            {
                service.Initialize();
            }
        }

        public static void Release(this IEnumerable<IService> self)
        {
            foreach (var service in self)
            {
                service.Release();
            }
        }
    }
}