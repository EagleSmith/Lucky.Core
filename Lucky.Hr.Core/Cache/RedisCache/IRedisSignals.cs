using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucky.Hr.Caching;
using Lucky.Hr.Core.Infrastructure;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;

namespace Lucky.Hr.Core.Cache.RedisCache
{
    public interface IRedisSignals : IVolatileProvider
    {
        void Trigger<T>(string key, T signal);
        IVolatileToken When<T>(T signal);
    }
    [Serializable]
    public class RedisSignals : IRedisSignals
    {
        readonly IDictionary<object, Token> _tokens = new Dictionary<object, Token>();

        public void Trigger<T>(string key, T signal)
        {
            lock (_tokens)
            {
                Token token;
                if (_tokens.TryGetValue(signal, out token))
                {
                    _tokens.Remove(signal);
                    EngineContext.Current.Resolve<ICacheClient>().Remove(key);
                    token.Trigger();
                }
            }

        }

        public IVolatileToken When<T>(T signal)
        {
            lock (_tokens)
            {
                Token token;
                if (!_tokens.TryGetValue(signal, out token))
                {
                    token = new Token();
                    _tokens[signal] = token;
                }
                return token;
            }
        }
        [Serializable]
        class Token : IVolatileToken
        {
            public Token()
            {
                IsCurrent = true;
            }
            public bool IsCurrent { get; private set; }
            public void Trigger() { IsCurrent = false; }
        }
    }
}
