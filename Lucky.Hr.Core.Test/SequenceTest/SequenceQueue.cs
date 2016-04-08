using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Lucky.Hr.Core.Utility.Sequence;
using StackExchange.Redis.Extensions.Core.Extensions;

namespace Lucky.Hr.Core.Test.SequenceTest
{
    public static class SequenceQueue
    {
        private static ConcurrentQueue<long> _queue=new ConcurrentQueue<long>();
        private static Id64Generator id64Generator;

        static SequenceQueue()
        {
            id64Generator = new Id64Generator();
        }
        public static long GetLong()
        {
            long res;
            if (_queue.Count > 0)
            {
                _queue.TryDequeue(out res);
                return res;
            }
            else
            {
                id64Generator.Take(5000).ForEach(a=>_queue.Enqueue(a));
                _queue.TryDequeue(out res);
                return res;
            }
        }
    }
}
