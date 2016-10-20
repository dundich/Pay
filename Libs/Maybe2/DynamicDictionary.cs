using System.Collections.Generic;
using System.Dynamic;

namespace Maybe2
{
    public class DynamicDictionary<T> : DynamicObject
    {
        readonly IDictionary<string, T> _dictionary
            = new Dictionary<string, T>();


        public DynamicDictionary() : this(new Dictionary<string, T>())
        {
        }

        public DynamicDictionary(IDictionary<string, T> dict)
            : base()
        {
            _dictionary = dict;
        }


        public IDictionary<string, T> GetOriginal()
        {
            return _dictionary;
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(getKey(key));
        }


        public bool Remove(string key)
        {
            return _dictionary.Remove(getKey(key));
        }

        private static string getKey(string key)
        {
            return (key ?? "").ToLower();
        }

        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            string name = getKey(binder.Name);
            result = _dictionary.GetOrDefault(name);
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _dictionary[getKey(binder.Name)] = (T)value;
            return true;
        }
    }
}
