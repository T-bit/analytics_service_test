using TestEventService.Attributes;
using TestEventService.Services;
using UnityEngine;

namespace TestEventService
{
    public sealed class Game : MonoSingleton<Game>
    {
        [SerializeReference]
        [Polymorphic]
        private IService[] _services = new IService[0];
    }
}