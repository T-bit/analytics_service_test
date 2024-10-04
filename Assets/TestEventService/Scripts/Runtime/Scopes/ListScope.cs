using System;
using System.Collections.Generic;
using TestEventService.Utilities;

namespace TestEventService.Scopes
{
    public readonly struct ListScope<T> : IDisposable
    {
        private readonly List<T> _list;

        private ListScope(List<T> list)
        {
            _list = list;
        }

        public static ListScope<T> Create(out List<T> list)
        {
            list = PoolUtility<List<T>>.Pull();

            return new ListScope<T>(list);
        }

        #region IDisposable

        void IDisposable.Dispose()
        {
            _list.Clear();

            PoolUtility<List<T>>.Push(_list);
        }

        #endregion
    }
}