using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lucky.Core.Utility.Extensions
{
    public static class BinarySerializerExtensions
    {
        /// <summary>
        /// 序列化为二进制流字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToBinary<T>(this T value) where T : new()
        {
            using (MemoryStream streamMemory = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                // 2. Serialize the dataset object using the binary formatter
                formatter.Serialize(streamMemory, value);
                // 3. Encrypt the binary data
                string binaryData = Convert.ToBase64String(streamMemory.GetBuffer());
                // 4. Write the data to a file
                return binaryData;
            }
        }

        /// <summary>
        /// 反序列化二进制流为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sString"></param>
        /// <returns></returns>
        public static T FromBinary<T>(this string stream) where T : new()
        {
            object data = new object();
            try
            {
                // 2. Read the binary data, and convert it to a string
                string cipherData = stream;
                // 3. Decrypt the binary data
                byte[] binaryData = Convert.FromBase64String(cipherData);
                // 4. Rehydrate the dataset
                using (MemoryStream streamMemory = new MemoryStream(binaryData))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    data = formatter.Deserialize(streamMemory);
                }
            }
            catch
            {
                // data could not be deserialized
                data = null;
            }
            return (T)data;
        }
    }
}
