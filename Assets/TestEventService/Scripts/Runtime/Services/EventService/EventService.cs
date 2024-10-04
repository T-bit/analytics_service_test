using System;
using UnityEngine;

namespace TestEventService.Services
{
    [Serializable]
    public sealed class EventService : Service, IEventService
    {
        [SerializeField]
        private string _serverUrl;

        [SerializeField]
        private float _cooldownBeforeSend = 1f;

        #region IEventService

        void IEventService.TrackEvent(string type, string data)
        {
        }

        #endregion
    }
}