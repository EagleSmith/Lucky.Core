using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IO;
using ProtoBuf;

namespace Lucky.Core.Utility.ProtoBuffer
{
    public class ProtoBufferDeserializer:IProtoBufferDeserializer
    {
        /// <summary>
        ///     Deserializes from file
        /// </summary>
        /// <typeparam name="T">Type to deserialize into</typeparam>
        /// <param name="filePath">Filepath for deserialization</param>
        /// <param name="gzipDecompress">Use gzip decompression, if your data is serialized with gzip</param>
        /// <returns>File deserialized into type</returns>
        public T FromFile<T>(
                             [NotNull] string filePath,
                             bool gzipDecompress = false)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));

            var readAllBytes = File.ReadAllBytes(filePath);

            return FromByteArray<T>(readAllBytes, gzipDecompress);
        }

        /// <summary>
        ///     Deserializes from file
        /// </summary>
        /// <typeparam name="T">Type to deserialize into</typeparam>
        /// <param name="filePath">Filepath for deserialization</param>
        /// <param name="gzipDecompress">Use gzip decompression, if your data is serialized with gzip</param>
        /// <returns>File deserialized into type</returns>
        public async Task<T> FromFileAsync<T>(
                                              [NotNull] string filePath,
                                              bool gzipDecompress = false)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));


            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous))
            {
                byte[] buff = new byte[fs.Length];
                await fs.ReadAsync(buff, 0, (int)fs.Length);
                fs.Position = 0;

                if (gzipDecompress)
                {
                    using (var gzip = new GZipStream(fs, CompressionMode.Decompress, true))
                    {
                        return Serializer.Deserialize<T>(gzip);
                    }
                }

                return Serializer.Deserialize<T>(fs);
            }
        }

        /// <summary>
        ///     Deserializes from byte array
        /// </summary>
        /// <typeparam name="T">Type to deserialize into</typeparam>
        /// <param name="value">Byte-array to be deserialized</param>
        /// <param name="gzipDecompress">Use gzip decompression, if your data is serialized with gzip</param>
        /// <returns>Byte-array deserialized into type</returns>
        public T FromByteArray<T>(
                                  [NotNull] byte[] value,
                                  bool gzipDecompress = false)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            var manager = new RecyclableMemoryStreamManager();
            
            using (var ms =new RecyclableMemoryStream(manager,"mytag") )//new MemoryStream(value)
            {
                ms.Write(value,0,value.Length);
                ms.Position = 0;
                if (gzipDecompress)
                {
                    using (var gzip = new GZipStream(ms, CompressionMode.Decompress, true))
                    {
                        return Serializer.Deserialize<T>(gzip);
                    }
                }

                return Serializer.Deserialize<T>(ms);
            }
        }
    }
}
