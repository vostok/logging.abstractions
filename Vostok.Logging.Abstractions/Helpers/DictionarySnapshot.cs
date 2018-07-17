using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Vostok.Logging.Abstractions.Helpers
{
    internal class DictionarySnapshot<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
        where TValue : class
    // TODO(krait): Also copy this to vostok.commons.
    {
        private const int DefaultCapacity = 4;

        public static readonly DictionarySnapshot<TKey, TValue> Empty = new DictionarySnapshot<TKey, TValue>(0);

        private readonly Pair[] keyValuePairs;

        private readonly IEqualityComparer<TKey> keyComparer;

        public DictionarySnapshot(IEqualityComparer<TKey> keyComparer = null)
            : this(DefaultCapacity, keyComparer)
        {
        }

        public DictionarySnapshot(int capacity, IEqualityComparer<TKey> keyComparer = null)
            : this(new Pair[capacity], 0, keyComparer)
        {
        }

        private DictionarySnapshot(Pair[] keyValuePairs, int count, IEqualityComparer<TKey> keyComparer)
        {
            this.keyValuePairs = keyValuePairs;
            this.keyComparer = keyComparer;
            Count = count;
        }

        public int Count { get; }

        public IEnumerable<TKey> Keys => this.Select(pair => pair.Key);

        public IEnumerable<TValue> Values => this.Select(pair => pair.Value);

        public TValue this[TKey key] => Find(key, out var value, out var _) ? value : throw new KeyNotFoundException($"A value with key '{key}' is not present.");

        public bool ContainsKey(TKey key)
        {
            return Find(key, out var _, out var _);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return Find(key, out value, out var _);
        }

        public DictionarySnapshot<TKey, TValue> Set(TKey key, TValue value)
        {
            Pair[] newProperties;

            var newProperty = new Pair(key, value);

            if (Find(key, out var oldValue, out var oldIndex))
            {
                if (Equals(value, oldValue))
                    return this;

                newProperties = ReallocateArray(keyValuePairs.Length);
                newProperties[oldIndex] = newProperty;
                return new DictionarySnapshot<TKey, TValue>(newProperties, Count, keyComparer);
            }

            if (keyValuePairs.Length == Count)
            {
                newProperties = ReallocateArray(Math.Max(DefaultCapacity, keyValuePairs.Length*2));
                newProperties[Count] = newProperty;
                return new DictionarySnapshot<TKey, TValue>(newProperties, Count + 1, keyComparer);
            }

            if (Interlocked.CompareExchange(ref keyValuePairs[Count], newProperty, null) != null)
            {
                newProperties = ReallocateArray(keyValuePairs.Length);
                newProperties[Count] = newProperty;
                return new DictionarySnapshot<TKey, TValue>(newProperties, Count + 1, keyComparer);
            }

            return new DictionarySnapshot<TKey, TValue>(keyValuePairs, Count + 1, keyComparer);
        }

        public DictionarySnapshot<TKey, TValue> Remove(TKey key)
        {
            if (Find(key, out var _, out var oldIndex))
            {
                var newProperties = new Pair[keyValuePairs.Length];

                Array.Copy(keyValuePairs, 0, newProperties, 0, oldIndex);
                Array.Copy(keyValuePairs, oldIndex + 1, newProperties, oldIndex, keyValuePairs.Length - oldIndex - 1);

                return new DictionarySnapshot<TKey, TValue>(newProperties, Count - 1, keyComparer);
            }

            return this;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
                yield return keyValuePairs[i].ToKeyValuePair();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private bool Find(TKey key, out TValue value, out int index)
        {
            for (var i = 0; i < Count; i++)
            {
                var property = keyValuePairs[i];
                if (keyComparer?.Equals(property.Key, key) ?? property.Key.Equals(key))
                {
                    index = i;
                    value = property.Value;
                    return true;
                }
            }

            index = -1;
            value = null;
            return false;
        }

        private Pair[] ReallocateArray(int capacity)
        {
            var reallocated = new Pair[capacity];

            Array.Copy(keyValuePairs, 0, reallocated, 0, Count);

            return reallocated;
        }

        private class Pair
        {
            public Pair(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }

            public TKey Key { get; }
            public TValue Value { get; }

            public KeyValuePair<TKey, TValue> ToKeyValuePair()
            {
                return new KeyValuePair<TKey, TValue>(Key, Value);
            }
        }
    }
}
