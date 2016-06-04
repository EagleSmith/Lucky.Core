using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ServiceStack;

namespace Lucky.Core.Test
{
    [TestFixture]
    public class TestProtoBuf
    {
        [Test]
        public void TestProtoBufSerialize()
        {
            int UPPER = 100000;
            TestNews news=new TestNews();
            news.ID = 1;
            news.Title = "你好世界！@？Hello World!";
            news.SortID = 1;
            news.IsValid = true;
            news.CreateDateTime=DateTime.Now;
            Console.WriteLine(news.Title);
            using (var file = System.IO.File.Create("News.bin"))
            {
                ProtoBuf.Serializer.Serialize(file, news);
            }
            Stopwatch stopwatch=new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < UPPER; i++)
            {
                var str = ProtoBufHelper.Serialize<TestNews>(news);
            }
            
            stopwatch.Stop();
            Console.WriteLine("ProtoBuf:"+stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();
            for (int i = 0; i < UPPER; i++)
            {
                var c = ServiceStack.ProtoBuf.ProtoBufExtensions.ToProtoBuf(news);
            }
            stopwatch.Stop();
            Console.WriteLine("Service:"+stopwatch.ElapsedMilliseconds);

            stopwatch.Restart();
            for (int i = 0; i < UPPER; i++)
            {
                Newtonsoft.Json.JsonConvert.SerializeObject(news);
            }
            stopwatch.Stop();
            Console.WriteLine("Json:"+stopwatch.ElapsedMilliseconds);
            string str1=Newtonsoft.Json.JsonConvert.SerializeObject(news);

            // TestNews n = ProtoBufHelper.DeSerialize<TestNews>(str);

            //   Console.WriteLine(n.Title+"   "+n.CreateDateTime);

            TestNews binPerson = null;
            using (var file = System.IO.File.OpenRead("News.bin"))
            {
                binPerson = ProtoBuf.Serializer.Deserialize<TestNews>(file);
            }
            Console.WriteLine(binPerson.Title+"   "+binPerson.CreateDateTime);

        }
    }
}
