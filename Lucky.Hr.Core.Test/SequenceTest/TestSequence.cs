using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Lucky.Core.Cache;
using Lucky.Core.Infrastructure;
using Lucky.Core.Logging;
using Lucky.Core.Utility.Sequence;
using SharpCode.Test;
using Xunit;
using Xunit.Abstractions;


namespace Lucky.Core.Test.SequenceTest
{
    /// <summary>
    /// 自动生成序列测试
    /// </summary>
   
    public class TestSequence
    {
        public IContainer _container;
        private ICacheManager _cacheManager;
        private int flag = 0;
        private readonly ITestOutputHelper _output;

        public TestSequence(ITestOutputHelper output)
        {
            _output = output;
            EngineContext.Initialize(false);
            var builder = new ContainerBuilder();
            builder.RegisterType<Id64Generator>().As<Id64Generator>().SingleInstance();
            builder.RegisterType<IdGuidGenerator>().As<IdGuidGenerator>().SingleInstance();
            builder.RegisterType<HrLogger>().As<ILogger>().SingleInstance();
            _container = builder.Build();
        }
        const string formatter = "{0,22}{1,30}";
        /// <summary>
        /// 64位长整型生成
        /// </summary>
        [Fact]
        public void Long64BitGeneratorTest()
        {
            var id64Generator = new Id64Generator();
            
            
            for (int i=0;i<20;i++)
            {
                _output.WriteLine(Long64BitGenerator().ToString());
                Thread.Sleep(1);
            }
        }
        [Fact]
        public void Long64BitGeneratorTimeTest()
        {
            var id64Generator = new Id64Generator();
            CodeTimer.Time("生成随机", 10, () =>
            {
                id64Generator.Take(1000000).ToArray();
            });
            
        }
        public long Long64BitGenerator()
        {
            var id64Generator = new Id64Generator();// _container.Resolve<Id64Generator>();
            return id64Generator.GenerateId();
        }
        [Fact]
        public void GuidGeneratorTest()
        {
            
            _output.WriteLine(" == Guid ids ==");
            for (int i = 0; i < 20; i++)
            {
                _output.WriteLine(GuidGenerator().ToString());
                Thread.Sleep(1);
            }
        }
        [Fact]
        public void SequenceQueueTest()
        {
            ILogger logger= _container.Resolve<ILogger>();
            for (int i = 0; i < 2000; i++)
            {
                logger.Information(SequenceQueue.NewIdLong().ToString());
                //Console.WriteLine(SequenceQueue.GetLong());
            }
        }
        public Guid GuidGenerator()
        {
            var idGuidGenerator = _container.Resolve<IdGuidGenerator>();
            return idGuidGenerator.GenerateId();
        }
        [Fact]
        public void LongToStringIdGeneratorTest()
        {
            _output.WriteLine(" == String ids with prefix 'o' ==");

            var idGenerator = new IdStringGeneratorWrapper(new Id64Generator(), "Pro_");

            foreach (var id in idGenerator.Take(5).ToArray())
            {
                _output.WriteLine(id);
            }
        }
        [Fact]
        public void MacAddressGenerator()
        {
            var mac = NetworkInterface.GetAllNetworkInterfaces()
                .Where(i => i.OperationalStatus == OperationalStatus.Up)
                .First().GetPhysicalAddress().GetAddressBytes();

            if (BitConverter.IsLittleEndian)
                Array.Reverse(mac);

            var generator = new IdGuidGenerator(mac);

            _output.WriteLine("");
            _output.WriteLine(" == Guid ids with MAC Address identifier ({0}) ==",
                BitConverter.ToString(mac));

            foreach (var id in generator.Take(5).ToArray())
            {
                _output.WriteLine(id.ToString());
            }

            _output.WriteLine("");
            _output.WriteLine(" == Guid ids with MAC Address identifier:[{0}] and epoch:[{1}] ==",
                BitConverter.ToString(mac), new DateTime(2012, 10, 1));

            generator = new IdGuidGenerator(mac, new DateTime(2012, 10, 1));
           
            foreach (var id in generator.Take(5).ToArray())
            {
                _output.WriteLine(id.ToString());
            }
        }
        [Fact]
        public void LongToUpperHexStringIdGenerator()
        {
            _output.WriteLine(" == String ids (upper hex conversion) with prefix 'upper' ==");

            var idGenerator =
                new IdStringGeneratorWrapper(
                    new Id64Generator(), IdStringGeneratorWrapper.UpperHex, "upper");
            foreach (var id in idGenerator.Take(5).ToArray())
            {
                _output.WriteLine(id);
            }
        }
        [Fact]
        public void LongToLowerHexStringIdGenerator()
        {
            _output.WriteLine(" == String ids (lower hex conversion) with prefix 'low' ==");

            var idGenerator =
                new IdStringGeneratorWrapper(
                    new Id64Generator(), IdStringGeneratorWrapper.LowerHex, "low");

            foreach (var id in idGenerator.Take(5).ToArray())
            {
                _output.WriteLine(id);
            }
            int a = 0,b=0;
            for (int i = 0; i < 20; i++)
            {
                _output.WriteLine(GetLongLowerString(a,b));
                a++;
                b++;
            }
        }
        [Fact]
        public void LongToBase32StringIdGenerator()
        {
            _output.WriteLine(" == String ids (base 32 conversion) ==");

            var idGenerator =
                new IdStringGeneratorWrapper(
                    new Id64Generator(), IdStringGeneratorWrapper.Base32);

            foreach (var id in idGenerator.Take(50).ToArray())
            {
                _output.WriteLine(id);
            }
        }
        [Fact]
        public void TestNewGuid()
        {
            for (int i = 0; i < 2000; i++)
            {
                _output.WriteLine(SequenceQueue.NewIdGuid().ToString());
            }
            
        }
        private string GetLongLowerString(int a,int b)
        {
            var idGenerator =
                new IdStringGeneratorWrapper(
                    new Id64Generator(generatorId:a,sequence:b), IdStringGeneratorWrapper.LowerHex, "low");
            return idGenerator.GenerateId();
        }
        public  void GetBytesUInt64(ulong argument)
        {
            byte[] byteArray = BitConverter.GetBytes(argument);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(byteArray);

            _output.WriteLine(formatter, argument,
                BitConverter.ToString(byteArray));
        }
    }
}
