using System;


namespace Cashing
{
    public interface ICashe<T>
    {
        T Get(string key);
        void Set(string key, T value, DateTimeOffset expirationDate);
    }
}
