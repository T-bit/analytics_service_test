using System.Diagnostics;
using UnityEngine;

namespace TestEventService.Attributes
{
    [Conditional("UNITY_EDITOR")]
    public sealed class PolymorphicAttribute : PropertyAttribute
    {
    }
}