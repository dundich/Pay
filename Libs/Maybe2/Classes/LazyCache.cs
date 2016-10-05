using System;

namespace Maybe2.Classes
{
    public class LazyCache<TModel>
    {
        private Lazy<TModel> _lazyObj;
        private readonly Func<TModel> _valueFactory;

        protected LazyCache()
        {
            Reset();
        }

        public LazyCache(Func<TModel> valueFactory)
            : this()
        {
            _valueFactory = valueFactory;
        }

        public void Reset()
        {
            _lazyObj = new Lazy<TModel>(MapingFunc());
        }

        public TModel Value
        {
            get { return _lazyObj.Value; }
        }

        public bool IsValueCreated
        {
            get { return _lazyObj.IsValueCreated; }
        }

        protected virtual Func<TModel> MapingFunc()
        {
            return () =>
            {
                return _valueFactory();
            };
        }
    }
}
