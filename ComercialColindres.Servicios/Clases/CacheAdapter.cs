using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using System;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Clases
{
    public class CacheAdapter : ICacheAdapter
    {
        private readonly ICacheClient _cacheClient;

        public CacheAdapter(ICacheClient cacheClient)
        {
            _cacheClient = cacheClient;
        }

        public T TryGetValue<T>(string keyCache, Func<T> metodo)
        {
            var datos = _cacheClient.Get<T>(keyCache);
            if (datos != null)
            {
                return datos;
            }
            datos = metodo();
            _cacheClient.Set(keyCache, datos);
            return datos;
        }

        public void Add(string keyCache, object valor)
        {
            _cacheClient.Set(keyCache, valor);
        }

        public void Increment(string key, uint amount)
        {
            _cacheClient.Increment(key, amount);
        }

        public void Remove(string keyCache)
        {
            var cache = ((MemoryCacheClient)(_cacheClient));
            cache.RemoveByRegex(keyCache);
        }

        public void Remove(List<string> keysCache)
        {
            foreach (var itemKeyCache in keysCache)
            {
                Remove(itemKeyCache);
            }
        }
    }
}
