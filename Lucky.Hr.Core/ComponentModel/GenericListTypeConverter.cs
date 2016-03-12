// =================================================================== 
// 项目说明
//====================================================================
// 幸运草工作室 @ CopyRight 2014-2020
// 文件： GenericListTypeConverter.cs
// 项目名称： 
// 创建时间：2014/6/4
// 创建人：丁富升
// ===================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Hr.Core.ComponentModel
{
    /// <summary>
    /// 泛型集合类型转换
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericListTypeConverter<T> : TypeConverter
    {
        protected readonly TypeConverter TypeConverter;

        public GenericListTypeConverter()
        {
            TypeConverter = TypeDescriptor.GetConverter(typeof(T));
            if (TypeConverter == null)
                throw new InvalidOperationException("不能进行类型转换，不存在类型 " + typeof(T).FullName);
        }

        protected virtual string[] GetStringArray(string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                var result = input.Split(',');
                Array.ForEach(result, s => s.Trim());
                return result;
            }
            else
                return new string[0];
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType != typeof(string)) return base.CanConvertFrom(context, sourceType);
            var items = GetStringArray(sourceType.ToString());
            return items.Any();
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!(value is string)) return base.ConvertFrom(context, culture, value);
            var items = GetStringArray((string)value);
            var result = new List<T>();
            Array.ForEach(items, s =>
            {
                var item = TypeConverter.ConvertFromInvariantString(s);
                if (item != null)
                {
                    result.Add((T)item);
                }
            });

            return result;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string)) return base.ConvertTo(context, culture, value, destinationType);
            var result = string.Empty;
            if (((IList<T>)value) == null) return result;
            for (var i = 0; i < ((IList<T>)value).Count; i++)
            {
                var str1 = Convert.ToString(((IList<T>)value)[i], CultureInfo.InvariantCulture);
                result += str1;
                //最后一个项目之后不加逗号
                if (i != ((IList<T>)value).Count - 1)
                    result += ",";
            }
            return result;
        }
    }
}
