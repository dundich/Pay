using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace Maybe2
{
    [DebuggerStepThrough]
    public static class DictionaryExtensions
    {
        public static IDictionary<K, V> SetKeyValue<K, V>(this IDictionary<K, V> dict, K key, V value)
        {
            if (dict == null) throw new ArgumentNullException("dictionary");
            dict[key] = value;
            return dict;
        }

        public static V GetOrDefault<K, V>(this IDictionary<K, V> dict, K key, V onMissing = default(V))
        {
            if (dict == null)
                return onMissing;

            V value;
            return key != null && dict.TryGetValue(key, out value) ? value : onMissing;
        }

        public static U GetOrInsertNew<T, U>(this IDictionary<T, U> dic, T key)
               where U : new()
        {
            if (dic == null) throw new ArgumentNullException("dictionary");
            if (dic.ContainsKey(key)) return dic[key];
            U newObj = new U();
            dic[key] = newObj;
            return newObj;
        }

        public static U GetOrInsertNew<T, U>(this IDictionary<T, U> dic, T key, Func<T, U> create)
        {
            if (dic == null) throw new ArgumentNullException("dictionary");
            if (dic.ContainsKey(key)) return dic[key];
            U newObj = create(key);
            dic[key] = newObj;
            return newObj;
        }

        public static KeyValuePair<TKey, TValue> PairWith<TKey, TValue>(this TKey key, TValue value)
        {
            return new KeyValuePair<TKey, TValue>(key, value);
        }


        public static TValue LazyGetOrAdd<TKey, TValue>(
            this ConcurrentDictionary<TKey, Lazy<TValue>> dictionary,
            TKey key,
            Func<TKey, TValue> valueFactory)
        {
            if (dictionary == null) throw new ArgumentNullException("dictionary");
            var result = dictionary.GetOrAdd(key, new Lazy<TValue>(() => valueFactory(key)));
            return result.Value;
        }

        public static ConcurrentDictionary<TKey, TValue> ToConcurrentDictionary<TKey, TValue>(this IDictionary<TKey, TValue> from)
        {
            var to = new ConcurrentDictionary<TKey, TValue>();
            foreach (var entry in from)
            {
                to[entry.Key] = entry.Value;
            }
            return to;
        }

        public static bool UnorderedEquivalentTo<K, V>(this IDictionary<K, V> thisMap, IDictionary<K, V> otherMap)
        {
            if (thisMap == null || otherMap == null) return thisMap == otherMap;
            if (thisMap.Count != otherMap.Count) return false;

            foreach (var entry in thisMap)
            {
                V otherValue;
                if (!otherMap.TryGetValue(entry.Key, out otherValue)) return false;
                if (!Equals(entry.Value, otherValue)) return false;
            }

            return true;
        }

        public static IDictionary<TKey, TValue> Overload<TKey, TValue>(this IDictionary<TKey, TValue> thisMap, IEnumerable<KeyValuePair<TKey, TValue>> otherMap)
        {
            otherMap.ForEach(kvp => thisMap[kvp.Key] = kvp.Value);
            return thisMap;
        }


        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> otherMap)
        {
            var d = new Dictionary<TKey, TValue>();
            d.Overload(otherMap);
            return d;
        }


        public static DynamicDictionary<V> GetDynamicDictionary<V>(this IDictionary<string, V> dict)
        {
            return new DynamicDictionary<V>(dict);
        }
    }
}