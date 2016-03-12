using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Hr.Core.Utility.ProtoBuffer
{
    public interface IProtoBufferSerializer
    {
        string SaveToFile([NotNull] object item, [NotNull] string filePath,FileMode fileMode = FileMode.Create, bool gzipCompress = false);

        Task<string> SaveToFileAsync( [NotNull] object item,[NotNull] string filePath, FileMode fileMode = FileMode.Create,bool gzipCompress = false);

        byte[] ToByteArray([NotNull] object item, bool gzipCompress = false);
    }
}
