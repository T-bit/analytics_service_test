using System.Collections.Generic;
using TestEventService.Services;
using UnityEngine;

namespace TestEventService
{
    public sealed class Game : MonoBehaviour
    {
        private readonly List<IService> _services = new();
    }
}