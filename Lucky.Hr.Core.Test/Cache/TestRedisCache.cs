using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
