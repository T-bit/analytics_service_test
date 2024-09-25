using UnityEngine;
using UnityEngine.Assertions;

namespace TestEventService
{
    public abstract class MonoSingleton<T> : MonoBehaviour
        where T : MonoSingleton<T>
    {
        public static T Instance
        {
            get;
            private set;
        }

        #region Unity

        protected virtual void Awake()
        {
            Assert.IsNull(Instance);

            Instance = (T)this;
        }

        protected virtual void OnDestroy()
        {
            Assert.IsNotNull(Instance);

            Instance = default;
        }

        #endregion
    }
}