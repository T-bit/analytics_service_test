using System.Collections.Generic;
using TestEventService.Extensions;
using TestEventService.Services;
using UnityEngine;

namespace TestEventService
{
    public sealed class Game : MonoBehaviour
    {
        private readonly List<IService> _services = new();

        private void Initialize()
        {
            _services.Initialize();
        }

        private void Release()
        {
            _services.Release();
            _services.Clear();
        }

        #region Unity

        private void Awake()
        {
            gameObject.GetComponentsInChildren(_services);
        }

        private void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            Release();
        }

        private void OnApplicationQuit()
        {
            Release();
        }

        #endregion
    }
}