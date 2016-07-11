using System.Collections.Generic;

namespace Restrictions.Graph
{
    class DataByKey<TKey>
    {
        #region Fields

        readonly Dictionary<TKey, object> _store = new Dictionary<TKey, object>();

        #endregion

        #region Public Methods and Operators

        public void Add<TValue>(TKey key, TValue value)
        {
            _store.Add(key, value);
        }

        public TValue Get<TValue>(TKey key)
        {
            return (TValue)_store[key];
        }

        #endregion
    }
}