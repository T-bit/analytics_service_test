using System;
using System.Collections.Generic;
using TestEventService.Extensions;
using TestEventService.Services;
using UnityEngine;

namespace TestEventService
{
    public sealed class Game : MonoBehaviour
    {
        private readonly List<IService> _services = new();

        #region Unity

        private void Awake()
        {
            gameObject.GetComponentsInChildren(_services);
        }

        private void Start()
        {
            _services.Initialize();
        }

        private void OnDestroy()
        {
            _services.Release();
        }

        #endregion
    }
}