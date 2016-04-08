using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Lucky.Hr.Caching;
using Lucky.Hr.Core.Cache.RedisCache;
using Lucky.Hr.Core.Caching.Memcached;
using Lucky.Hr.Core.Infrastructure;
using Lucky.Hr.Core.Logging;
using Lucky.Hr.Core.Services;
using Lucky.Hr.Core.Utility.ProtoBuffer;
using NUnit.Framework;
using StackExchange.Redis.Extensions.Core;

namespace Lucky.Hr.Core.Test.Cache
{
    [TestFixture]
    public class TestRedisCache
    {
        public IContainer _container;
        private ICacheManager _cacheManager;
        [SetUp]
        public void Init()
        {
            EngineContext.Initialize(false);
            var builder = new ContainerBuilder();

            builder.RegisterType<RedisCacheHolder>().As<ICacheHolder>().SingleInstance();
            builder.RegisterType<MemCacheHolder>().As<IMemCacheHolder>().SingleInstance();
            builder.RegisterType<DefaultCacheContextAccessor>().As<ICacheContextAccessor>().SingleInstance();
            builder.RegisterType<DefaultParallelCacheContext>().As<IParallelCacheContext>().SingleInstance();
            builder.RegisterType<DefaultAsyncTokenProvider>().As<IAsyncTokenProvider>().SingleInstance();
            builder.RegisterType<MemCacheManager>().As<IMemCacheManager>().SingleInstance();

            builder.RegisterType<StackExchange.Redis.Extensions.Newtonsoft.NewtonsoftSerializer>().As<ISerializer>().SingleInstance();
            // builder.RegisterType<StackExchange.Redis.Extensions.Protobuf.ProtobufSerializer>().As<ISerializer>().SingleInstance();

            builder.RegisterType<StackExchangeRedisCacheClient>().As<ICacheClient>().SingleInstance();

            builder.RegisterType<Signals>().As<ISignals>().SingleInstance();
            builder.RegisterType<RedisSignals>().As<IRedisSignals>().SingleInstance();

            builder.RegisterType<MemSignals>().As<IMemSignals>().SingleInstance();
            builder.RegisterType<HrLogger>().As<ILogger>().InstancePerLifetimeScope();
            builder.RegisterType<Clock>().As<IClock>().SingleInstance();
            builder.RegisterType<MemClock>().As<IMemClock>().SingleInstance();
            builder.RegisterType<RedisClock>().As<IRedisClock>().SingleInstance();
            builder.RegisterType<ProtoBufferSerializer>().As<IProtoBufferSerializer>().SingleInstance();
            builder.RegisterType<ProtoBufferDeserializer>().As<IProtoBufferDeserializer>().SingleInstance();

            builder.RegisterModule(new CacheModule());
            _container = builder.Build();
        }
        [Test]
        public void TestGet()
        {
            _cacheManager = _container.Resolve<ICacheManager>(new TypedParameter(typeof(Type), GetType()));
            var result = _cacheManager.Get("testItem", ctx => "testResult");
            Console.WriteLine(result);
        }
        [Test]
        public void TestSignal()
        {
            _cacheManager = _container.Resolve<ICacheManager>(new TypedParameter(typeof(Type), GetType()));
            IRedisSignals _signals = _container.Resolve<IRedisSignals>();
            var cached = DateTime.Now;
            string key = "testItem2";
            Func<DateTime> retrieve = ()
                => _cacheManager.Get(key,
                        ctx =>
                        {
                            ctx.Monitor(_signals.When(key));
                            return cached;
                        });
            Console.WriteLine(retrieve());
            // Assert.That(retrieve(), Is.EqualTo(1));
            _signals.Trigger(key, key);
            Thread.Sleep(2000);
            cached = DateTime.Now;
            //Assert.That(retrieve(), Is.EqualTo(1));
            Console.WriteLine(retrieve());
            _signals.Trigger(key, key);
        }
        [Test]
        public void TestClock()
        {
            _cacheManager = _container.Resolve<ICacheManager>(new TypedParameter(typeof(Type), GetType()));
            IRedisClock _clock= _container.Resolve<IRedisClock>();
            string key = "testItem2";
            Func<DateTime> retrieve = ()
                => _cacheManager.Get(key,
                        ctx =>
                        {
                            ctx.Monitor(_clock.When(key,TimeSpan.FromSeconds(1)));
                            return DateTime.Now;
                        });
            Console.WriteLine(retrieve());
            Thread.Sleep(5000);
            Console.WriteLine(retrieve());
        }
        [Test]
        public void TestGuid()
        {
            for (int i = 0; i < 100; i++)
            {
                //Console.WriteLine(GenerateComb());
                
               Console.WriteLine(CreateSequentialUID());

            }
        }
       

        public Guid GenerateComb()
        {

            byte[] guidArray = Guid.NewGuid().ToByteArray();
            DateTime baseDate = new DateTime(1900, 1, 1);

            DateTime now = DateTime.Now;
            // Get the days and milliseconds which will be used to build    

            //the byte string    

            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);

            TimeSpan msecs = now.TimeOfDay;
            // Convert to a byte array        

            // Note that SQL Server is accurate to 1/300th of a    

            // millisecond so we divide by 3.333333    

            byte[] daysArray = BitConverter.GetBytes(days.Days);

            byte[] msecsArray = BitConverter.GetBytes((long)

              (msecs.TotalMilliseconds / 3.333333));
            // Reverse the bytes to match SQL Servers ordering    

            Array.Reverse(daysArray);

            Array.Reverse(msecsArray);

            // Copy the bytes into the guid    

            Array.Copy(daysArray, daysArray.Length - 2, guidArray,

              guidArray.Length - 6, 2);

            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray,

              guidArray.Length - 4, 4);



            return new Guid(guidArray);

        }
        private static Guid CreateSequentialUID()

        {
            var address = NetworkInterface.GetAllNetworkInterfaces()[0].GetPhysicalAddress().GetAddressBytes(); // TODO: Interrogate, choose, and store the MAC address.

            var time = System.BitConverter.GetBytes(DateTime.Now.Ticks).Reverse().ToList();


            var byteList = new List<byte>(time);


            
            byteList.Add(0x0);

            byteList.Add(0x0);

            byteList.AddRange(address);

            Thread.Sleep((int)time[4] / 10);

            return new Guid(byteList.ToArray());


        }

    }

    public enum GuidVersion
	{ 
		TimeBased = 0x01, 
		Reserved = 0x02, 
 		NameBased = 0x03, 
 		Random = 0x04 
 	} 

 } 



    public class SequentialGuid
    {
        Guid _CurrentGuid;
        public Guid CurrentGuid
        {
            get
            {
                return _CurrentGuid;
            }
        }

        public SequentialGuid()
        {
            _CurrentGuid = Guid.NewGuid();
        }

        public SequentialGuid(Guid previousGuid)
        {
            _CurrentGuid = previousGuid;
        }

        public static SequentialGuid operator ++(SequentialGuid sequentialGuid)
        {
            byte[] bytes = sequentialGuid._CurrentGuid.ToByteArray();
            for (int mapIndex = 0; mapIndex < 16; mapIndex++)
            {
                int bytesIndex = SqlOrderMap[mapIndex];
                bytes[bytesIndex]++;
                if (bytes[bytesIndex] != 0)
                {
                    break; // No need to increment more significant bytes
                }
            }
            sequentialGuid._CurrentGuid = new Guid(bytes);
            return sequentialGuid;
        }

        private static int[] _SqlOrderMap = null;
        private static int[] SqlOrderMap
        {
            get
            {
                if (_SqlOrderMap == null)
                {
                    _SqlOrderMap = new int[16] {
                    3, 2, 1, 0, 5, 4, 7, 6, 9, 8, 15, 14, 13, 12, 11, 10
                };
                    // 3 - the least significant byte in Guid ByteArray [for SQL Server ORDER BY clause]
                    // 10 - the most significant byte in Guid ByteArray [for SQL Server ORDERY BY clause]
                }
                return _SqlOrderMap;
            }
        }
    }


