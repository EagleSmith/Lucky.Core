using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enyim.Caching;

namespace Lucky.Core.Cache.Memcached
{
    public static class MemcachedHelper
    {
        private static MemcachedClient _client;
        static readonly object Padlock = new object();
        //线程安全的单例模式
        public static MemcachedClient GetInstance()
        {
            if (_client == null)
            {
                lock (Padlock)
                {
                    if (_client == null)
                    {
                        _client = new MemcachedClient();
                    }
                }
            }
            return _client;
        }
    }
}
