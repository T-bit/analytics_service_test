using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using TestEventService.Events;
using TestEventService.Extensions;
using TestEventService.Network;
using TestEventService.Scopes;
using TestEventService.Utilities;
using UnityEngine;
using UnityEngine.Assertions;
using Event = TestEventService.Events.Event;

namespace TestEventService.Services
{
    [Serializable]
    public sealed class EventService : Service, IEventService
    {
        private static readonly string EventsDataPath = $"{nameof(EventsData)}.json";

        private List<Event> _events;
        private bool _saving;
        private bool _sending;

        [SerializeField]
        private string _serverUrl;

        [SerializeField]
        private float _cooldownBeforeSend;

        [SerializeField]
        private float _cooldownBeforeSave;

        [SerializeField]
        private int _sendEventLimit;

        [SerializeField]
        private int _saveEventLimit;

        [SerializeField]
        private bool _logResponse;

        protected override void OnInitialize()
        {
            _events = PersistentDataUtility.TryReadJson(EventsDataPath, out var eventsJson)
                ? SerializationUtility.Deserialize<List<Event>>(eventsJson)
                : new List<Event>();

            if (_events.Count > 0)
            {
                StartSendProcess();
            }
        }

        protected override void OnRelease()
        {
            Save();

            _events.Clear();
        }

        private void AddEvent(string type, string data)
        {
            while (_events.Count >= _saveEventLimit)
            {
                _events.RemoveAt(0);
            }

            _events.Add(new Event
            {
                Type = type,
                Data = data
            });

            StartSendProcess();
            StartSaveProcess();
        }

        private void StartSaveProcess()
        {
            if (!_saving)
            {
                SaveProcessAsync(CancellationToken).Forget();
            }
        }

        private void StartSendProcess()
        {
            if (!_sending)
            {
                SendProcessAsync(CancellationToken).Forget();
            }
        }

        private async UniTask SaveProcessAsync(CancellationToken cancellationToken)
        {
            _saving = true;

            await UniTask.Delay(TimeSpan.FromSeconds(_cooldownBeforeSave), cancellationToken: cancellationToken);

            if (!_saving)
            {
                return;
            }

            Save();

            _saving = false;
        }

        private async UniTask SendProcessAsync(CancellationToken cancellationToken)
        {
            _sending = true;

            while (_events.Count > 0)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_cooldownBeforeSend), cancellationToken: cancellationToken);

                try
                {
                    await SendEventsAsync(cancellationToken);
                }
                catch (Exception exception) when (exception is not OperationCanceledException)
                {
                    Debug.LogWarning(exception);
                }
            }

            _sending = false;
        }

        private async UniTask SendEventsAsync(CancellationToken cancellationToken)
        {
            using (ListScope<Event>.Create(out var batchEvents))
            {
                _events.Take(_sendEventLimit)
                       .ToList(batchEvents);

                var eventsData = new EventsData { Events = batchEvents };
                var eventsDataJson = SerializationUtility.Serialize(eventsData, false);
                var request = new Request(_serverUrl).SetMethod(HttpRequestMethod.Post)
                                                     .SetJson(eventsDataJson);
                var response = await request.ExecuteAsync(cancellationToken);

                if (response.ResponseCode != 200)
                {
                    Debug.LogWarning($"{nameof(EventService)}: Got {response.ResponseCode} response code from server. Response:\n{response.ToString()}.");

                    return;
                }

                if (_logResponse)
                {
                    Debug.Log($"{nameof(EventService)}: Got {response.ResponseCode} response code from server. Response:\n{response.ToString()}.");
                }

                _saving = false;
                _events.RemoveRange(0, batchEvents.Count);

                Save();
            }
        }

        private void Save()
        {
            Assert.IsNotNull(_events);

            try
            {
                var eventsDataJson = SerializationUtility.Serialize(_events, true);

                PersistentDataUtility.WriteAllText(EventsDataPath, eventsDataJson);
            }
            catch (Exception exception)
            {
                Debug.LogWarning(exception);
            }
        }

        [ContextMenu("Add some events")]
        private void AddSome()
        {
            for (var i = 0; i < 5; i++)
            {
                AddEvent($"type_{i}", Guid.NewGuid().ToString());
            }
        }

        #region IEventService

        void IEventService.TrackEvent(string type, string data)
        {
            AddEvent(type, data);
        }

        #endregion
    }
}