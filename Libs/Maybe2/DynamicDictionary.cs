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
            return _dictionary.ContainsKey(NormalizeKey(key));
        }


        public bool Remove(string key)
        {
            return _dictionary.Remove(NormalizeKey(key));
        }

        protected virtual string NormalizeKey(string key)
        {
            return (key ?? "");//.ToLower();
        }

        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            string name = NormalizeKey(binder.Name);
            result = _dictionary.GetOrDefault(name);
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _dictionary[NormalizeKey(binder.Name)] = (T)value;
            return true;
        }


        public T this[string key]
        {
            get
            {
                return _dictionary.GetOrDefault(NormalizeKey(key));

            }
            set
            {
                _dictionary[NormalizeKey(key)] = value;
            }
        }
    }
}
