using System;
using System.Collections.Generic;

namespace ComercialColindres.Servicios.Clases
{
    public interface ICacheAdapter
    {
        T TryGetValue<T>(string keyCache, Func<T> metodo);
        void Add(string keyCache, object valor);
        void Increment(string key, uint amount);
        void Remove(string keyCache);
        void Remove(List<string> keysCache);
    }
}
