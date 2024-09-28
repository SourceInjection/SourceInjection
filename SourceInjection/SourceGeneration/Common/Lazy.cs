using System;

namespace SourceInjection.SourceGeneration.Common
{
    internal class Lazy<T>
    {
        private Func<T> _loadingFun;
        private T _value;

        public Lazy(Func<T> loadingFun)
        {
            if (loadingFun is null)
                throw new ArgumentNullException(nameof(loadingFun));
            _loadingFun = loadingFun;
        }

        public T Value
        {
            get
            {
                if (_loadingFun != null)
                {
                    _value = _loadingFun();
                    _loadingFun = null;
                }
                return _value;
            }
        }
    }
}
