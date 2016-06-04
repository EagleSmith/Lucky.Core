using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Lucky.Core.Utility.Sequence;
using StackExchange.Redis.Extensions.Core.Extensions;


namespace Lucky.Core.Test.SequenceTest
{
    public static class SequenceQueue
    {
        private static ConcurrentQueue<long> _queue=new ConcurrentQueue<long>();
        private static ConcurrentQueue<Guid> _queueGuids = new ConcurrentQueue<Guid>();
        private static Id64Generator id64Generator;
        private static IdGuidGenerator idGuid;
        private static  Object _obj = new Object();
        static SequenceQueue()
        {
            id64Generator = new Id64Generator();
            idGuid=new IdGuidGenerator();
        }
        public static long NewIdLong()
        {
            long res;
            if (_queue.Count > 0)
            {
                _queue.TryDequeue(out res);
                return res;
            }
            else
            {
                lock (_obj)
                {
                    id64Generator.Take(5000).ForEach(a => _queue.Enqueue(a));
                }
                
                _queue.TryDequeue(out res);
                return res;
            }
        }

        public static Guid NewIdGuid()
        {
            Guid guid =Guid.Empty;
            if (_queueGuids.Count > 0)
            {
                _queueGuids.TryDequeue(out guid);
                return guid;
            }
            else
            {
                lock (_obj)
                {
                    idGuid.Take(5000).ForEach(a => _queueGuids.Enqueue(a));
                }
                _queueGuids.TryDequeue(out guid);
                return guid;
            }
        }
    }
}
