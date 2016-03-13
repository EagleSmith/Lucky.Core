using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Lucky.Hr.Caching;
using Lucky.Hr.Core.Caching.Memcached;
using Lucky.Hr.Core.Logging;
using Lucky.Hr.Core.Services;
using Lucky.Hr.Core.Utility.ProtoBuffer;
using Newtonsoft.Json;
using NUnit.Framework;
using ProtoBuf;
using SharpCode.Test;
using StackExchange.Redis.Extensions.Core;

namespace Lucky.Hr.Core.Test.Cache
{
    public class TestCache
    {
        public IContainer _container;
        [SetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DefaultCacheHolder>().As<ICacheHolder>().SingleInstance();
            builder.RegisterType<MemCacheHolder>().As<IMemCacheHolder>().SingleInstance();
            builder.RegisterType<DefaultCacheContextAccessor>().As<ICacheContextAccessor>().SingleInstance();
            builder.RegisterType<DefaultParallelCacheContext>().As<IParallelCacheContext>().SingleInstance();
            builder.RegisterType<DefaultAsyncTokenProvider>().As<IAsyncTokenProvider>().SingleInstance();
            builder.RegisterType<MemCacheManager>().As<IMemCacheManager>().SingleInstance();

            builder.RegisterType<StackExchange.Redis.Extensions.Newtonsoft.NewtonsoftSerializer>().As<ISerializer>().SingleInstance();
            // builder.RegisterType<StackExchange.Redis.Extensions.Protobuf.ProtobufSerializer>().As<ISerializer>().SingleInstance();
            builder.RegisterType<StackExchangeRedisCacheClient>().As<ICacheClient>().SingleInstance();

            builder.RegisterType<Signals>().As<ISignals>().SingleInstance();
            builder.RegisterType<MemSignals>().As<IMemSignals>().SingleInstance();
            builder.RegisterType<HrLogger>().As<ILogger>().InstancePerLifetimeScope();
            builder.RegisterType<Clock>().As<IClock>().SingleInstance();
            builder.RegisterType<MemClock>().As<IMemClock>().SingleInstance();

            builder.RegisterType<ProtoBufferSerializer>().As<IProtoBufferSerializer>().SingleInstance();
            builder.RegisterType<ProtoBufferDeserializer>().As<IProtoBufferDeserializer>().SingleInstance();

            builder.RegisterModule(new CacheModule());
            _container = builder.Build();
        }
        [Test]
        public void TestMemcached()
        {
            var _cacheManager = _container.Resolve<IMemCacheManager>(new TypedParameter(typeof(Type), GetType()));
            SSOToken token = new SSOToken();
            token.token = "123456789";
            token.custcode = "C001";
            token.timeout = DateTime.Now;
            token.userid = 1;
            //for (int i = 0; i < 10000; i++)
            {
                string str = "str" + 1;
                _cacheManager.Get(str, ac => token);
            }
            string _str = JsonConvert.SerializeObject(token);
            _cacheManager.Get("TestCache1", ac => token);
            CodeTimer.Time("读取缓存", 10000, () =>
            {
               // for (int i = 0; i < 10000*100; i++)
                {
                    string str = "str" + 9999;
                    _cacheManager.Get(str, ac => _str);
                }
            });
           // string _str = JsonConvert.SerializeObject(token);
            ILogger logger = _container.Resolve<ILogger>();
            //通过formatter对象以二进制格式将obj对象序列化后到文件MyFile.bin中
           
            StreamWriter strw;
            string filepa = "Token.txt";
            string mypath = "d:\\"+filepa;
            strw = File.AppendText(mypath);
            for (int i = 0; i < 10000 * 100; i++)
            {
                //strw.WriteLine(i.ToString() + _str);
            }
            strw.Close();

        }
        [Test]
        public void TestMemcachedTime()
        {
            var _cacheManager = _container.Resolve<ICacheManager>(new TypedParameter(typeof(Type), GetType()));
            var _clock = _container.Resolve<IMemClock>();
            var inOneSecond = _clock.UtcNow.AddSeconds(3);
            var cached = 0;

            // each call after the specified datetime will be reevaluated
            Func<string> retrieve = ()
                => _cacheManager.Get("testItem1",
                        ctx =>
                        {
                            ctx.Monitor(_clock.When("testItem1",TimeSpan.FromSeconds(2)));
                            return DateTime.Now.ToString();
                        });

            Console.WriteLine(retrieve());
            Thread.Sleep(3000);
            Console.WriteLine(retrieve());
            Console.WriteLine(retrieve());
            Console.WriteLine(retrieve());
            Thread.Sleep(3000);
            Console.WriteLine(retrieve());
        }
        [Test]
        public void DeserializerProtoBuf()
        {
            
            var s=_container.Resolve<IProtoBufferSerializer>();
            CodeTimer.Time("Protobuf", 10000, () =>
            {
                SSOToken token = new SSOToken();
                token.token = Guid.NewGuid().ToString();
                token.custcode = "C001";
                token.timeout = DateTime.Now;
                token.userid = 1;
                var arry = s.ToByteArray(token);
            });
            CodeTimer.Time("Json.net", 10000, () =>
            {
                SSOToken token = new SSOToken();
                token.token = Guid.NewGuid().ToString();
                token.custcode = "C001";
                token.timeout = DateTime.Now;
                token.userid = 1;
                JsonConvert.SerializeObject(token);
            });
            var d=_container.Resolve<IProtoBufferDeserializer>();
            //var _token = d.FromByteArray<SSOToken>(arry);
            var str = "";

        }
    }
    [Serializable]
    [ProtoBuf.ProtoContract]
    public class SSOToken
    {
        [ProtoMember(1)]
        public string _id { get; set; }
        [ProtoMember(2)]
        public int userid { get; set; }
        [ProtoMember(3)]
        public string custcode { get; set; }
        [ProtoMember(4)]
        public string token { get; set; }
        [ProtoMember(5)]
        public DateTime timeout { get; set; }
    }
}
