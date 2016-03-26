using System;
using System.Management;
using System.Text;
using Lucky.Hr.Core.Data.Dapper;
using Lucky.Hr.Core.Utility;
using NUnit.Framework;
namespace Lucky.Hr.Core.Test
{
    [TestFixture]
    public class TestCommon
    {
        [Test]
        public void TestRand()
        {
            var r = new Random();
            //for (int i = 0; i < 100; i++)
           // Console.WriteLine(r.Next(27));
           // long tick = DateTime.Now.Ticks;
           // Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
           // Stream Recy
            //var manager = new RecyclableMemoryStreamManager(); 
            //manager.g
            for (int i = 0; i < 100; i++)
            {
               // Console.WriteLine(ran.Next());
            }
            DateTime t = DateTime.Now;
            Console.WriteLine(t);
            t = t.AddMonths(-1);
            Console.WriteLine(t);

            DapperContext context=new DapperContext("TestDapper");
            context.Batch(s =>
            {
                string sql = @"select 1 as a1;select 2 as a2;select 3 as a3;select 4 as a4;
select 5 as a5;select 6 as a6;select 7 as a7;select 8 as a8;select 9 as a9;select 10 as a10;
";
                Dapper.SqlMapper.GridReader gr = s.QueryMultiple(sql);
                var a1 = gr.Read<b1>();
                var a2 = gr.Read<b2>();
                var a3 = gr.Read<b3>();
                var a4 = gr.Read<b4>();
                var a5 = gr.Read<b5>();
                var a6 = gr.Read<b6>();
                var a7 = gr.Read<b7>();
                var a8 = gr.Read<b8>();
                var a9 = gr.Read<b9>();
                var a10 = gr.Read<b10>();
               

            });
        }
        [Test]
        public void TestStringGetID()
        {
            string b = "008005006";
            string a = StringHelper.GetID(b, 6, 3, 3);
            Console.WriteLine(a);
            string str = "13222222222".Insert(3," ").Insert(8," ");
            bool s = true,y=false;
           
            Console.WriteLine(str);
            Console.Write(Environment.OSVersion);
        }
        public string GetSystemType()
        {
            string st = "";
            ManagementClass mc = new ManagementClass("Win32_OperatingSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * from Win32_OperatingSystem");
            
            foreach (ManagementObject mo in moc)
            {
                
                st = mo["SystemType"].ToString();
            }
            return st;
        }
        [Test]
        public void TestStringLength()
        {
            string str = "高姐看盘：沪铜08合约45930二次加空，仓位15%。预计筑顶后还会有一次加空操作。仓位控制好。";
            var b = Encoding.UTF8.GetBytes(str);
            Console.WriteLine(str.Length);
            Console.WriteLine(b.Length);
        }

    }

    public class b1
    {
        public string a1 { get; set; }
    }
    public class b2
    {
        public string a2 { get; set; }
    }
    public class b3
    {
        public string a3 { get; set; }
    }
    public class b4
    {
        public string a4 { get; set; }
    }
    public class b5
    {
        public string a5 { get; set; }
    }
    public class b6
    {
        public string a6 { get; set; }
    }
    public class b7
    {
        public string a7 { get; set; }
    }
    public class b8
    {
        public string a8 { get; set; }
    }
    public class b9
    {
        public string a9 { get; set; }
    }
    public class b10
    {
        public string a10 { get; set; }
    }
}
