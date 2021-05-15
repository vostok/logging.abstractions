
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;


namespace Vostok.Logging.Abstractions.CommonsCopy
{
    /// <summary>
    /// <para>An immutable dictionary implemented using an array.</para>
    /// <para>Search takes linear time, so this collection should only be used with small number of items.</para>
    /// </summary>
    [PublicAPI]
    internal class ImmutableArrayDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        private const int DefaultCapacity = 4;

        /// <summary>
        /// An empty <see cref="ImmutableArrayDictionary{TKey,TValue}"/>.
        /// </summary>
        public static readonly ImmutableArrayDictionary<TKey, TValue> Empty = new ImmutableArrayDictionary<TKey, TValue>(0);

        private readonly Pair[] pairs;

        private readonly IEqualityComparer<TKey> keyComparer;

        /// <summary>
        /// Create a new <see cref="ImmutableArrayDictionary{TKey,TValue}"/> with default capacity.
        /// </summary>
        public ImmutableArrayDictionary([CanBeNull] IEqualityComparer<TKey> keyComparer = null)
            : this(DefaultCapacity, keyComparer)
        {
        }

        /// <summary>
        /// Create a new <see cref="ImmutableArrayDictionary{TKey,TValue}"/> with the given capacity.
        /// </summary>
        public ImmutableArrayDictionary(int capacity, [CanBeNull] IEqualityComparer<TKey> keyComparer = null)
            : this(new Pair[capacity], 0, keyComparer)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "The capacity must be non-negative");
        }

        /// <summary>
        /// Create a new <see cref="ImmutableArrayDictionary{TKey,TValue}"/> with pairs from given <paramref name="source"/>.
        /// </summary>
        public ImmutableArrayDictionary(IReadOnlyDictionary<TKey, TValue> source, [CanBeNull] IEqualityComparer<TKey> keyComparer = null)
            : this(new Pair[source.Count], source.Count, keyComparer)
        {
            var index = 0;

            foreach (var pair in source)
                pairs[index++] = new Pair(pair.Key, pair.Value, this.keyComparer.GetHashCode(pair.Key));
        }

        private ImmutableArrayDictionary(Pair[] pairs, int count, IEqualityComparer<TKey> keyComparer)
        {
            this.pairs = pairs;
            this.keyComparer = keyComparer ?? EqualityComparer<TKey>.Default;

            Count = count;
        }

        /// <inheritdoc />
        public int Count { get; }

        /// <inheritdoc />
        public IEnumerable<TKey> Keys =>
            this.Select(pair => pair.Key);

        /// <inheritdoc />
        public IEnumerable<TValue> Values =>
            this.Select(pair => pair.Value);

        /// <inheritdoc />
        public bool ContainsKey(TKey key) =>
            Find(key, out _, out _);

        /// <inheritdoc />
        public bool TryGetValue(TKey key, out TValue value) =>
            Find(key, out value, out _);

        /// <summary>
        /// Returns a new <see cref="ImmutableArrayDictionary{TKey,TValue}"/> with the same data plus <paramref name="key"/> set to <paramref name="value"/>.
        /// </summary>
        /// <param name="key">The key to set value for.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="overwrite">Specifies the behavior in case a value with the same key exists. If <c>true</c>, the value will be overwritten in the returned dictionary. Otherwise, the new value is ignored and the original dictionary is returned.</param>
        public ImmutableArrayDictionary<TKey, TValue> Set(TKey key, TValue value, bool overwrite = true)
        {
            Pair[] newPairs;

            var hash = ComputeHash(key);

            var newPair = new Pair(key, value, hash);

            if (Find(key, hash, out var oldValue, out var oldIndex))
            {
                if (!overwrite || Equals(value, oldValue))
                    return this;

                newPairs = ReallocateArray(pairs.Length);
                newPairs[oldIndex] = newPair;
                return new ImmutableArrayDictionary<TKey, TValue>(newPairs, Count, keyComparer);
            }

            if (pairs.Length == Count)
            {
                newPairs = ReallocateArray(Math.Max(DefaultCapacity, pairs.Length * 2));
                newPairs[Count] = newPair;
                return new ImmutableArrayDictionary<TKey, TValue>(newPairs, Count + 1, keyComparer);
            }

            lock (pairs)
            {
                if (pairs[Count].IsOccupied)
                {
                    newPairs = ReallocateArray(pairs.Length);
                    newPairs[Count] = newPair;
                    return new ImmutableArrayDictionary<TKey, TValue>(newPairs, Count + 1, keyComparer);
                }

                pairs[Count] = newPair;
            }

            return new ImmutableArrayDictionary<TKey, TValue>(pairs, Count + 1, keyComparer);
        }

        /// <summary>
        /// Returns a new <see cref="ImmutableArrayDictionary{TKey,TValue}"/> with the same data plus <paramref name="pairsToAdd"/> pairs/>.
        /// <remarks>This method not check given collection of <paramref name="pairsToAdd"/> on keys uniqueness. If it contains different elements with the same key, they all will be set.</remarks>
        /// </summary>
        /// <param name="pairsToAdd">The collection of new key-value pairs to set.</param>
        /// <param name="overwrite">Specifies the behavior in case a value with the same key exists. If <c>true</c>, the value will be overwritten in the returned dictionary. Otherwise, the new value is ignored and the original dictionary is returned.</param>
        public ImmutableArrayDictionary<TKey, TValue> SetRange((TKey key, TValue value)[] pairsToAdd, bool overwrite = true)
        {
            var length = pairsToAdd.Length;
            if (length >= 1024)
            {
                return SetRangeSafeFromStackOverflowPath(pairsToAdd, overwrite);
            }

            Pair[] reallocatedArray;

            var indicesCountToOverride = 0;
            var equalItemsCount = 0;
            var oldIndices = new int[length];
            var hashes = new int[length];

            for (var i = 0; i < length; i++)
            {
                var (key, value) = pairsToAdd[i];
                var hash = hashes[i] = ComputeHash(key);
                if (Find(key, hash, out var oldValue, out var oldIndex))
                {
                    if (!overwrite || Equals(value, oldValue))
                    {
                        //Required to distinguish from zero-index and NeedToAppend marker.
                        oldIndices[i] = NoNeedToSetMarker;
                        equalItemsCount++;
                        continue;
                    }

                    oldIndices[i] = oldIndex;
                    indicesCountToOverride++;
                    continue;
                }

                oldIndices[i] = NeedToAppendMarker;
            }

            if (equalItemsCount == length)
                return this;

            var newItemsCount = Count + length - indicesCountToOverride - equalItemsCount;
            if (indicesCountToOverride > 0 || newItemsCount > pairs.Length)
            {
                //If we are have enough capacity but have items to override we will recreate array with old capacity.
                var reallocatedArraySize = newItemsCount > pairs.Length ? Math.Max(DefaultCapacity, newItemsCount * 2) : pairs.Length;
                reallocatedArray = ReallocateArray(reallocatedArraySize);
                var newCount = RefillArray(reallocatedArray, pairsToAdd, oldIndices, hashes, Count);

                return new ImmutableArrayDictionary<TKey, TValue>(reallocatedArray, newCount, keyComparer);
            }

            lock (pairs)
            {
                int newCount;
                if (pairs[Count].IsOccupied)
                {
                    reallocatedArray = ReallocateArray(pairs.Length);
                    newCount = RefillArray(reallocatedArray, pairsToAdd, oldIndices, hashes, Count);
                    return new ImmutableArrayDictionary<TKey, TValue>(reallocatedArray, newCount, keyComparer);
                }

                //RefillArray is generic and can replace items. But at this point there is no chance to have items to replace. There are only with NeedToAppendMarker or NoNeedToSetMarker.
                newCount = RefillArray(pairs, pairsToAdd, oldIndices, hashes, Count);
                return new ImmutableArrayDictionary<TKey, TValue>(pairs, newCount, keyComparer);
            }

            int RefillArray(Pair[] placeholder, (TKey key, TValue value)[] newValues, int[] indicesOnOldValues, int[] hashesOfNewValues, int initialAppendIndex)
            {
                var indexToSet = initialAppendIndex;
                for (var i = 0; i < newValues.Length; i++)
                {
                    var oldIndex = indicesOnOldValues[i];

                    if (oldIndex == NeedToAppendMarker)
                    {
                        var (key, value) = newValues[i];
                        placeholder[indexToSet] = new Pair(key, value, hashesOfNewValues[i]);
                        indexToSet++;
                    }

                    else if (oldIndex >= 0)
                    {
                        var (key, value) = newValues[i];
                        placeholder[oldIndex] = new Pair(key, value, hashesOfNewValues[i]);
                    }

                    //If "oldIndex" have NoNeedToSet marker value then just skip this.
                }

                return indexToSet;
            }
        }



        /// <summary>
        /// Returns a new <see cref="ImmutableArrayDictionary{TKey,TValue}"/> with the same data plus <paramref name="pairsToAdd"/> pairs/>.
        /// <remarks>This method not check given collection of <paramref name="pairsToAdd"/> on keys uniqueness. If it contains different elements with the same key, they all will be set.</remarks>
        /// </summary>
        /// <param name="pairsToAdd">The collection of new key-value pairs to set.</param>
        /// <param name="overwrite">Specifies the behavior in case a value with the same key exists. If <c>true</c>, the value will be overwritten in the returned dictionary. Otherwise, the new value is ignored and the original dictionary is returned.</param>
        public unsafe ImmutableArrayDictionary<TKey, TValue> SetRangeUnsafe((TKey key, TValue value)[] pairsToAdd, bool overwrite = true)
        {
            var length = pairsToAdd.Length;
            if (length >= 1024)
            {
                return SetRangeSafeFromStackOverflowPath(pairsToAdd, overwrite);
            }

            Pair[] reallocatedArray;

            var indicesCountToOverride = 0;
            var equalItemsCount = 0;
            var oldIndices = stackalloc int[length];
            var hashes = stackalloc int[length];

            for (var i = 0; i < length; i++)
            {
                var (key, value) = pairsToAdd[i];
                var hash = hashes[i] = ComputeHash(key);
                if (Find(key, hash, out var oldValue, out var oldIndex))
                {
                    if (!overwrite || Equals(value, oldValue))
                    {
                        //Required to distinguish from zero-index and NeedToAppend marker.
                        oldIndices[i] = NoNeedToSetMarker;
                        equalItemsCount++;
                        continue;
                    }

                    oldIndices[i] = oldIndex;
                    indicesCountToOverride++;
                    continue;
                }

                oldIndices[i] = NeedToAppendMarker;
            }

            if (equalItemsCount == length)
                return this;

            var newItemsCount = Count + length - indicesCountToOverride - equalItemsCount;
            if (indicesCountToOverride > 0 || newItemsCount > pairs.Length)
            {
                //If we are have enough capacity but have items to override we will recreate array with old capacity.
                var reallocatedArraySize = newItemsCount > pairs.Length ? Math.Max(DefaultCapacity, newItemsCount * 2) : pairs.Length;
                reallocatedArray = ReallocateArray(reallocatedArraySize);
                var newCount = RefillArray(reallocatedArray, pairsToAdd, oldIndices, hashes, Count);

                return new ImmutableArrayDictionary<TKey, TValue>(reallocatedArray, newCount, keyComparer);
            }

            lock (pairs)
            {
                int newCount;
                if (pairs[Count].IsOccupied)
                {
                    reallocatedArray = ReallocateArray(pairs.Length);
                    newCount = RefillArray(reallocatedArray, pairsToAdd, oldIndices, hashes, Count);
                    return new ImmutableArrayDictionary<TKey, TValue>(reallocatedArray, newCount, keyComparer);
                }

                //RefillArray is generic and can replace items. But at this point there is no chance to have items to replace. There are only with NeedToAppendMarker or NoNeedToSetMarker.
                newCount = RefillArray(pairs, pairsToAdd, oldIndices, hashes, Count);
                return new ImmutableArrayDictionary<TKey, TValue>(pairs, newCount, keyComparer);
            }

            int RefillArray(Pair[] placeholder, (TKey key, TValue value)[] newValues, int* indicesOnOldValues, int* hashesOfNewValues, int initialAppendIndex)
            {
                var indexToSet = initialAppendIndex;
                for (var i = 0; i < newValues.Length; i++)
                {
                    var oldIndex = indicesOnOldValues[i];

                    if (oldIndex == NeedToAppendMarker)
                    {
                        var (key, value) = newValues[i];
                        placeholder[indexToSet] = new Pair(key, value, hashesOfNewValues[i]);
                        indexToSet++;
                    }

                    else if (oldIndex >= 0)
                    {
                        var (key, value) = newValues[i];
                        placeholder[oldIndex] = new Pair(key, value, hashesOfNewValues[i]);
                    }

                    //If "oldIndex" have NoNeedToSet marker value then just skip this.
                }

                return indexToSet;
            }
        }

        private ImmutableArrayDictionary<TKey, TValue> SetRangeSafeFromStackOverflowPath((TKey key, TValue value)[] pairsToAdd, bool overwrite)
        {
            var x = this;
            foreach (var (key, value) in pairsToAdd)
            {
                x = x.Set(key, value, overwrite);
            }

            return x;
        }

        /// <summary>
        /// Returns a new <see cref="ImmutableArrayDictionary{TKey,TValue}"/> with the same data except the value by <paramref name="key"/>. If there is no such key, the original dictionary is returned.
        /// </summary>
        public ImmutableArrayDictionary<TKey, TValue> Remove(TKey key)
        {
            if (Find(key, out _, out var oldIndex))
            {
                var newPairs = new Pair[pairs.Length];

                Array.Copy(pairs, 0, newPairs, 0, oldIndex);
                Array.Copy(pairs, oldIndex + 1, newPairs, oldIndex, Count - oldIndex - 1);

                return new ImmutableArrayDictionary<TKey, TValue>(newPairs, Count - 1, keyComparer);
            }

            return this;
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
                yield return pairs[i].ToKeyValuePair();
        }

        /// <inheritdoc />
        public TValue this[TKey key] =>
            Find(key, out var value, out _) ? value : throw new KeyNotFoundException($"A value with key '{key}' is not present.");

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private int ComputeHash(TKey key) => key == null ? 0 : keyComparer.GetHashCode(key);

        private bool Find(TKey key, out TValue value, out int index)
            => Find(key, ComputeHash(key), out value, out index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool Find(TKey key, int hash, out TValue value, out int index)
        {
            for (var i = 0; i < Count; i++)
            {
                var property = pairs[i];
                if (hash == property.Hash && keyComparer.Equals(property.Key, key))
                {
                    index = i;
                    value = property.Value;
                    return true;
                }
            }

            index = -1;
            value = default;
            return false;
        }

        private Pair[] ReallocateArray(int capacity)
        {
            var reallocated = new Pair[capacity];

            Array.Copy(pairs, 0, reallocated, 0, Count);

            return reallocated;
        }

        [DebuggerDisplay("[{Key}, {Value}]")]
        private struct Pair
        {
            public readonly TKey Key;
            public readonly TValue Value;
            public readonly int Hash;
            public volatile bool IsOccupied;

            public Pair(TKey key, TValue value, int hash)
            {
                Key = key;
                Value = value;
                Hash = hash;
                IsOccupied = true;
            }

            public KeyValuePair<TKey, TValue> ToKeyValuePair() =>
                new KeyValuePair<TKey, TValue>(Key, Value);
        }

        const int NoNeedToSetMarker = -1;
        const int NeedToAppendMarker = -2;
    }

}
