using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Core.Utility
{
    public static class StringHelper
    {
        /// <summary>
        /// 无限分级主键字符串生成
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <param name="index">取值位轩</param>
        /// <param name="length">取值长度</param>
        /// <param name="num">需要返回的字符数</param>
        /// <returns></returns>
        public static string GetID(string str, int index, int length,int num)
        {
            string tem = str.Substring(index, length);
            int b = 0;
            int.TryParse(tem, out b);
            b = b + 1;
            if (b.ToString().Length < num)
            {
                tem = b.ToString();
                while (num-tem.Length>0)
                {
                    tem = "0" + tem;
                }
            }
            
            return str.Substring(0,index)+tem;
        }
        public static object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
        /// <summary>
        /// 数据类型转换，当要转换的是 string 类型时需要加上最大长度
        /// </summary>
        /// <param name="type"></param>
        /// <param name="val"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static object ConverterStringData(Type type, string val, int maxLength = int.MaxValue)
        {
            object defaultval = null;
            try
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(type);
                defaultval = typeConverter.ConvertFromString(val);
                if (type == typeof(string))
                    return val.Substring(0, maxLength);

            }
            catch (Exception ex)
            {
                defaultval = GetDefaultValue(type);
            }
            return defaultval;
        }
    }
}
