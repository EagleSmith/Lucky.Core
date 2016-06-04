using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucky.Core.Configuration;

namespace Lucky.Core.IO
{
    public class FileSystemSettings : ISettings
    {
        public string DirectoryName { get; set; }
    }
}
