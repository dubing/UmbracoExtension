using System;
using System.Collections;
using System.Collections.Generic;

namespace AKQA.Common.Caching
{
    public class LruCache<TKey, TValue> : IDictionary<TKey, TValue>
    {
        // Dictionary that will hold the data to be cached
        private Dictionary<TKey, TValue> _dict = new Dictionary<TKey, TValue>();

        // List of keys for maintaining the age of each item in the cache
        // Keys are ordered from oldest to newest
        private LinkedList<TKey> _list = new LinkedList<TKey>();

        private object _locker = new object();

        private int _maxSize = 0;
        private int _currentSize = 0;
        private int _itemSize = 1;
        private int _hitCount = 0;
        private int _missCount = 0;
        private int _discardedCount = 0;

        /// <summary>
        /// Initializes a new instance of LruCache<TKey, TValue>.
        /// </summary>
        /// <param name="maxSize">The maximum size of the cache.</param>
        /// <param name="itemSize">The size of each entry in the cache.</param>
        public LruCache(int maxSize, int itemSize)
        {
            if (maxSize <= 0)
            {
                throw new ArgumentOutOfRangeException("maxSize cannot be <= 0");
            }

            if (itemSize < 0)
            {
                throw new ArgumentOutOfRangeException("itemSize cannot be < 0");
            }

            if (maxSize < itemSize)
            {
                throw new ArgumentOutOfRangeException("maxSize cannot be < itemSize");
            }

            _maxSize = maxSize;
            _itemSize = itemSize;
        }

        /// <summary>
        /// Initializes a new instance of LruCache<TKey, TValue>.
        /// </summary>
        /// <param name="maxSize">The maximum number of entries in the cache.</param>
        public LruCache(int maxSize)
            : this(maxSize, 1)
        {
        }

        public void Add(TKey key, TValue value)
        {

            if (!ContainsKey(key))
            {
                lock (_locker)
                {
                    _dict.Add(key, value);
                    _list.AddLast(key);

                    _currentSize += _itemSize;

                    TrimToSize(_maxSize);
                }
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            if (!Contains(item))
            {
                lock (_locker)
                {
                    Add(item.Key, item.Value);
                }
            }
        }

        public bool Remove(TKey key)
        {
            lock (_locker)
            {
                if (ContainsKey(key))
                {
                    if (_list.Remove(key) && _dict.Remove(key))
                    {
                        _currentSize -= _itemSize;
                        return true;
                    }
                }

                return false;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            lock (_locker)
            {
                return Remove(item.Key);
            }
        }

        public void Clear()
        {
            lock (_locker)
            {
                _list.Clear();
                _dict.Clear();

                _currentSize = 0;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                lock (_locker)
                {
                    if (ContainsKey(key))
                    {
                        _hitCount++;
                        RenewKey(key);
                        return _dict[key];
                    }
                    else
                    {
                        _missCount++;
                        return default(TValue);
                    }
                }
            }
            set
            {
                lock (_locker)
                {
                    if (!ContainsKey(key))
                    {
                        throw new KeyNotFoundException();
                    }

                    _dict[key] = value;
                    RenewKey(key);
                }
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (_locker)
            {
                if (_dict.TryGetValue(key, out value))
                {
                    _hitCount++;
                    RenewKey(key);
                    return true;
                }
                else
                {
                    _missCount++;
                    return false;
                }
            }
        }

        public bool ContainsKey(TKey key)
        {
            lock (_locker)
            {
                return (_list.Contains(key) && _dict.ContainsKey(key));
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            lock (_locker)
            {
                return ContainsKey(item.Key);
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                lock (_locker)
                {
                    return _dict.Keys;
                }
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                lock (_locker)
                {
                    return _dict.Values;
                }
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            lock (_locker)
            {
                IDictionary<TKey, TValue> dict = _dict;
                dict.CopyTo(array, arrayIndex);
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            lock (_locker)
            {
                return _dict.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (_locker)
            {
                return _dict.GetEnumerator();
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// The oldest value in the cache
        /// </summary>
        public TValue OldestValue
        {
            get
            {
                lock (_locker)
                {
                    if (Count > 0)
                    {
                        return _dict[_list.First.Value];
                    }

                    return default(TValue);
                }
            }
        }

        /// <summary>
        /// The oldest key in the cache
        /// </summary>
        public TKey OldestKey
        {
            get
            {
                lock (_locker)
                {
                    if (Count > 0)
                    {
                        return _list.First.Value;
                    }

                    return default(TKey);
                }
            }
        }

        /// <summary>
        /// The newest value in the cache
        /// </summary>
        public TValue NewestValue
        {
            get
            {
                lock (_locker)
                {
                    if (Count > 0)
                    {
                        return _dict[_list.Last.Value];
                    }

                    return default(TValue);
                }
            }
        }

        /// <summary>
        /// The newest key in the cache
        /// </summary>
        public TKey NewestKey
        {
            get
            {
                lock (_locker)
                {
                    if (Count > 0)
                    {
                        return _list.Last.Value;
                    }

                    return default(TKey);
                }
            }
        }

        /// <summary>
        /// The number of elements currently in the cache
        /// </summary>
        public int Count
        {
            get
            {
                lock (_locker)
                {
                    return _dict.Count;
                }
            }
        }

        /// <summary>
        /// The maximum size of the cache
        /// </summary>
        public int MaxSize
        {
            get
            {
                lock (_locker)
                {
                    return _maxSize;
                }
            }
        }

        /// <summary>
        /// The current size of the cache.  Depending on the item size, this 
        /// is not necessarily the same as the number of elements in the cache.
        /// </summary>
        public int CurrentSize
        {
            get
            {
                lock (_locker)
                {
                    return _currentSize;
                }
            }
        }

        /// <summary>
        /// The size of each item in the cache
        /// </summary>
        public int ItemSize
        {
            get
            {
                lock (_locker)
                {
                    return _itemSize;
                }
            }
        }

        /// <summary>
        /// The number of times an item has been successfully retrieved
        /// from the cache
        /// </summary>
        public int HitCount
        {
            get
            {
                lock (_locker)
                {
                    return _hitCount;
                }
            }
        }

        /// <summary>
        /// The number of times an item has been unsuccessfully retrieved
        /// from the cache.  Usually because it does not exist.
        /// </summary>
        public int MissCount
        {
            get
            {
                lock (_locker)
                {
                    return _missCount;
                }
            }
        }

        /// <summary>
        /// The number of items that have been removed from the cache due
        /// to reaching the maximum size
        /// </summary>
        public int DiscardedCount
        {
            get
            {
                lock (_locker)
                {
                    return _discardedCount;
                }
            }
        }

        /// <summary>
        /// The ratio of the current size to the maximum size, indicating
        /// how full the cache is.  Rounded to two decimal places.
        /// </summary>
        public decimal LoadFactor
        {
            get
            {
                lock (_locker)
                {
                    var current = Convert.ToDecimal(_currentSize);
                    var max = Convert.ToDecimal(_maxSize);
                    var load = (current / max);

                    return Math.Round(load, 2);
                }
            }
        }

        /// <summary>
        /// Resizes the cache to the specified size.  If the new size is less than 
        /// the current, then oldest items are automatically discarded.
        /// </summary>
        /// <param name="maxSize">The new maximum size of the cache</param>
        public void Resize(int maxSize)
        {
            lock (_locker)
            {
                if (maxSize <= 0)
                {
                    throw new ArgumentOutOfRangeException("maxSize cannot be <= 0");
                }

                if (maxSize < _itemSize)
                {
                    throw new ArgumentOutOfRangeException("maxSize cannot be < ItemSize");
                }

                _maxSize = maxSize;
                TrimToSize(_maxSize);
            }
        }

        /// <summary>
        /// A string representation of the current state of the cache for debugging/logging purposes
        /// </summary>
        public override string ToString()
        {
            lock (_locker)
            {
                return string.Format(
                    @"LruCache<{0}, {1}>
                    Max Size: {2}
                    Current Size: {3}
                    Item Size {4}
                    Hits: {5}
                    Misses: {6}
                    Discarded: {7}
                    Load Factor: {8}",
                typeof(TKey).Name, typeof(TValue).Name,
                MaxSize,
                CurrentSize,
                ItemSize,
                HitCount,
                MissCount,
                DiscardedCount,
                LoadFactor);
            }
        }

        private void TrimToSize(int maxSize)
        {
            lock (_locker)
            {
                while (_currentSize > maxSize)
                {
                    Remove(OldestKey);
                    _discardedCount++;
                }
            }
        }

        private void RenewKey(TKey key)
        {
            lock (_locker)
            {
                if (_list.Contains(key))
                {
                    _list.Remove(key);
                    _list.AddLast(key);
                }
            }
        }
    }
}
