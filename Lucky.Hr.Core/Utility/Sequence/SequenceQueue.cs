using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis.Extensions.Core.Extensions;


namespace Lucky.Core.Utility.Sequence
{
    public static class SequenceQueue
    {
        private static ConcurrentQueue<long> _queue = new ConcurrentQueue<long>();
        private static ConcurrentQueue<Guid> _queueGuids = new ConcurrentQueue<Guid>();
        private static ConcurrentDictionary<string,Queue<string>> _dictionary=new ConcurrentDictionary<string, Queue<string>>();
        private static Id64Generator id64Generator;
        private static IdGuidGenerator idGuid;
        private static IdStringGeneratorWrapper _generatorWrapper;
        private static Object _obj = new Object();
        

        static SequenceQueue()
        {
            id64Generator = new Id64Generator();
            idGuid = new IdGuidGenerator();
            _generatorWrapper=new IdStringGeneratorWrapper(id64Generator);
        }

        public static string NewIdString(string prefix = "Fix")
        {
            lock (_obj)
            {
                if (_dictionary.ContainsKey(prefix))
                {
                    if (_dictionary[prefix].Count > 0)
                        return prefix+_dictionary[prefix].Dequeue();
                    else
                    {
                        _generatorWrapper.Take(1000).ForEach(a => _dictionary[prefix].Enqueue(a));
                        return prefix + _dictionary[prefix].Dequeue();
                    }
                }
                else
                {
                    var _queuestr=new Queue<string>();
                    _generatorWrapper.Take(1000).ForEach(a => _queuestr.Enqueue(a));
                    _dictionary[prefix] = _queuestr;
                    return prefix + _queuestr.Dequeue();
                }
                
            }
            
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
            Guid guid = Guid.Empty;
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
