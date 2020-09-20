using System;
using System.Runtime.Caching;

namespace Cashing
{
    public class CustomMemoryCache<T> : ICashe<T>
    {
        ObjectCache _cache = MemoryCache.Default;
        string _prefix;

        public CustomMemoryCache(string prefix)
        {
            _prefix = prefix;
        }
        public CustomMemoryCache()
        {
            _cache = MemoryCache.Default;
        }
        public T Get(string key)
        {
            var value = _cache.Get(_prefix + key);
            if (value == null)          
                return default(T);           

            return (T)value;
        }

        public void Set(string key, T value, DateTimeOffset expirationDate)
        {
            _cache.Set(_prefix + key, value, expirationDate);
        }
        public void Set(string key, T value, CacheItemPolicy policy)
        {
            _cache.Set(_prefix + key, value, policy);
        }
    }
}
