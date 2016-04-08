using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Lucky.Hr.Core.Test
{
    public class XUnitTest
    {
        private readonly ITestOutputHelper _output;

        public XUnitTest(ITestOutputHelper output)
        {
            _output = output;
        }
        [Fact]
        public void Test()
        {
            Assert.Equal(4, 4);
        }
        [Fact]
        public void TestAdd()
        {
            Assert.Equal<int>(5, 2 + 3);
            _output.WriteLine("Hello");
            
        }
    }
}
