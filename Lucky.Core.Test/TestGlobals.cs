using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucky.Core.Utility;
using Xunit;
using Xunit.Abstractions;

namespace Lucky.Hr.Core.Test
{
    public class TestGlobals
    {
        private ITestOutputHelper _output;
        public TestGlobals(ITestOutputHelper testOutputHelper)
        {
            _output = testOutputHelper;
        }
        [Fact]
        public void TestGetFileType()
        {
            string strPath = "d:\\logo.jpg";
            _output.WriteLine(Globals.GetFileType(strPath).ToString());
        }
    }
}
