using System;
using System.Management;
using System.Text;
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
}
