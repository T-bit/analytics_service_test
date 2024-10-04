using System.Threading;
using Cysharp.Threading.Tasks;
using TestEventService.Extensions;
using UnityEngine;
using UnityEngine.Assertions;

namespace TestEventService.Services
{
    [DisallowMultipleComponent]
    public abstract class Service : MonoBehaviour, IService
    {
        private CancellationTokenSource _cancellationTokenSource;

        protected bool Initialized { get; private set; }

        protected CancellationToken CancellationToken => _cancellationTokenSource.Token;

        protected virtual void OnInitialize()
        {
        }

        protected virtual void OnRelease()
        {
        }

        #region IService

        bool IService.Initialized => Initialized;

        void IService.Initialize()
        {
            Assert.IsFalse(Initialized);

            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(gameObject.GetCancellationTokenOnDestroy());

            OnInitialize();
            Initialized = true;
        }

        void IService.Release()
        {
            Assert.IsTrue(Initialized);

            _cancellationTokenSource.CancelAndDispose();
            _cancellationTokenSource = null;

            OnRelease();
            Initialized = false;
        }

        #endregion
    }
}