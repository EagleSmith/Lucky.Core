using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Hr.Core.Utility.ProtoBuffer
{
    public interface IProtoBufferDeserializer
    {
        T FromFile<T>([NotNull] string filePath, bool gzipDecompress = false);

        Task<T> FromFileAsync<T>( [NotNull] string filePath, bool gzipDecompress = false);

        T FromByteArray<T>( [NotNull] byte[] value, bool gzipDecompress = false);
    }
}
