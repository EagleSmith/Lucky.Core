using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Hr.Core.IO
{
    public interface IStorageFile
    {
        string GetPath();
        string GetName();
        long GetSize();
        DateTime GetLastUpdated();
        string GetFileType();

        /// <summary>
        /// 创建用于从文件中读取流
        /// </summary>
        Stream OpenRead();

        /// <summary>
        /// 创建用于写入文件流
        /// </summary>
        Stream OpenWrite();
    }
}
