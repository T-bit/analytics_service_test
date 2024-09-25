using System;
using System.Reflection;
using UnityEditor;

namespace TestEventService.Extensions
{
    public static class SerializedPropertyExtensions
    {
        public static Type GetFieldType(this SerializedProperty self)
        {
            return GetFieldType(self.managedReferenceFieldTypename);
        }

        public static Type GetValueType(this SerializedProperty self)
        {
            return GetFieldType(self.managedReferenceFullTypename);
        }

        private static Type GetFieldType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return null;
            }

            var splitIndex = typeName.IndexOf(' ');
            var assembly = Assembly.Load(typeName[..splitIndex]);

            return assembly.GetType(typeName[(splitIndex + 1)..]);
        }
    }
}