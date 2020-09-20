using System;
using StackExchange.Redis;
using System.Runtime.Serialization;
using System.IO;

namespace Cashing
{
    public class RedisCache<T> : ICashe<T>
    {
        private const string localHostName = "localhost";
        ConnectionMultiplexer _redisConnection;
        string _prefix;

        DataContractSerializer _serializer = new DataContractSerializer(typeof(T));

        public RedisCache(string hostName, string prefix)
        {
            _prefix = prefix;
            var options = new ConfigurationOptions()
            {
                AbortOnConnectFail = false,
                EndPoints = { hostName }
            };
            _redisConnection = ConnectionMultiplexer.Connect(options);
        }

        public RedisCache()
        {
            var options = new ConfigurationOptions()
            {
                AbortOnConnectFail = false,
                EndPoints = { localHostName }
            };
            _redisConnection = ConnectionMultiplexer.Connect(options);
        }

        public T Get(string key)
        {
            var db = _redisConnection.GetDatabase();
            byte[] s = db.StringGet(_prefix + key);
            if (s == null)
            {
                return default(T);
            }

            return (T)_serializer.ReadObject(new MemoryStream(s));
        }

        public void Set(string key, T value, DateTimeOffset expirationDate)
        {
            var db = _redisConnection.GetDatabase();
            var redisKey = _prefix + key;

            if (value == null)
            {
                db.StringSet(redisKey, RedisValue.Null);
            }
            else
            {
                var stream = new MemoryStream();
                _serializer.WriteObject(stream, value);
                db.StringSet(redisKey, stream.ToArray(), expirationDate - DateTimeOffset.Now);
            }
        }
    }
}
