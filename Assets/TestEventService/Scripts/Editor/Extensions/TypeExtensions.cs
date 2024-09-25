using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestEventService.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetDerivedTypes(this Type self)
        {
            return Assembly.GetAssembly(self).GetTypes().Where(type => type.IsClass && !type.IsAbstract && self.IsAssignableFrom(type));
        }
    }
}