using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Hr.Core.Data.FluentData
{
    public static class DataReaderExtensions
    {
        #region "Public Methods"

    /// <summary>
    /// ExtensionMethod that creates a List(of Target) from the IDataReader
    /// </summary>
    /// <typeparam name="Target"></typeparam>
    /// <param name="Reader">IDataReader</param>
    /// <returns>Generic List</returns>
    public static List<Target> ToList<Target>(this IDataReader Reader) where Target : class, new()
    {
        CultureInfo Culture = CultureInfo.CurrentCulture;
        return ToList<Target>(Reader, Culture);
    }

    /// <summary>
    /// ExtensionMethod that creates a List(of Target) from the IDataReader
    /// </summary>
    /// <typeparam name="Target"></typeparam>
    /// <param name="Reader">IDataReader</param>
    /// <param name="Provider">IFormatProvider</param>
    /// <returns>Generic List</returns>
    public static List<Target> ToList<Target>(this IDataReader Reader, CultureInfo Culture) where Target : class, new()
    {
        List<Target> List = new List<Target>();
        if (!Reader.IsClosed)
        {
            Reader.Read();
            Func<IDataRecord, Target> Creator = Mapper<Target>.GetCreator(Reader, Culture);
            do
            {
                List.Add(Creator(Reader));
            } while (Reader.Read());
        }
        return List;
    }

    /// <summary>
    /// ExtensionMethod that creates an IEnumerable from the IDatareader
    /// </summary>
    /// <param name="Reader">IDataReader</param>
    /// <returns>IEnumerable</returns>
    public static IEnumerable<Target> AsEnumerable<Target>(this IDataReader Reader) where Target : class, new()
    {
        CultureInfo Culture = CultureInfo.CurrentCulture;
        if (!Reader.IsClosed)
        {
            Reader.Read();
            Func<IDataRecord, Target> Creator = Mapper<Target>.GetCreator(Reader, Culture);
            do
            {
                yield return (Creator(Reader));
            } while (Reader.Read());
        }
    }

    /// <summary>
    /// ExtensionMethod that creates an IEnumerable from the IDatareader
    /// </summary>
    /// <param name="Reader">IDataReader</param>
    /// <param name="Provider">IFormatProvider</param>
    /// <returns>IEnumerable</returns>
    public static IEnumerable<Target> AsEnumerable<Target>(this IDataReader Reader, CultureInfo Culture) where Target : class, new()
    {
        if (!Reader.IsClosed)
        {
            Reader.Read();
            Func<IDataRecord, Target> Creator = Mapper<Target>.GetCreator(Reader, Culture);
            do
            {
                yield return (Creator(Reader));
            } while (Reader.Read());
        }
    }

    #endregion

    #region "Private Methods"
    #region"Converter"

    /// <summary>
    /// Gets an expression representing the recordField converted to the TargetPropertyType
    /// </summary>
    /// <param name="RecordFieldType">The Type of the RecordField</param>
    /// <param name="RecordFieldExpression">An Expression representing the RecordField value</param>
    /// <param name="TargetMemberType">The Type of the TargetMember</param>
    /// <returns>Expression</returns>
    private static Expression GetConvertedRecordFieldExpression(Type RecordFieldType, Expression RecordFieldExpression, Type TargetMemberType, CultureInfo Culture)
    {
        Expression ConvertedRecordFieldExpression;
        if (object.ReferenceEquals(TargetMemberType, RecordFieldType))
        {
            //Just assign the RecordField
            ConvertedRecordFieldExpression = RecordFieldExpression;
        }
        else if (object.ReferenceEquals(RecordFieldType, typeof(string)))
        {
            ConvertedRecordFieldExpression = GetParseExpression(RecordFieldExpression, TargetMemberType, Culture);
        }
        else if (object.ReferenceEquals(TargetMemberType, typeof(string)))
        {
            //There are no casts from primitive types to String.
            //And Expression.Convert Method (Expression, Type, MethodInfo) only works with static methods.
            ConvertedRecordFieldExpression = Expression.Call(RecordFieldExpression, RecordFieldType.GetMethod("ToString", Type.EmptyTypes));
        }
        else if (object.ReferenceEquals(TargetMemberType, typeof(bool)))
        {
            MethodInfo ToBooleanMethod = typeof(Convert).GetMethod("ToBoolean", new[] { RecordFieldType });
            ConvertedRecordFieldExpression = Expression.Call(ToBooleanMethod, RecordFieldExpression);
        }
        else
        {
            //Using Expression.Convert works wherever you can make an explicit or implicit cast.
            //But it casts OR unboxes an object, therefore the double cast. First unbox to the SourceType and then cast to the TargetType
            //It also doesn't convert a numerical type to a String or date, this will throw an exception.
            ConvertedRecordFieldExpression = Expression.Convert(RecordFieldExpression, TargetMemberType);
        }
        return ConvertedRecordFieldExpression;
    }

    /// <summary>
    /// Creates an expression that parses a string to an enum
    /// </summary>
    /// <param name="RecordFieldExpression">The RecordField to parse</param>
    /// <param name="TargetMemberType">The Type of enum</param>
    /// <returns>MethodCallExpression</returns>
    private static MethodCallExpression GetEnumParseExpression(Expression RecordFieldExpression, Type TargetMemberType)
    {
        //Get the MethodInfo for parsing an Enum
        MethodInfo EnumParseMethod = typeof(Enum).GetMethod("Parse", new[] { typeof(Type), typeof(string), typeof(bool) });
        ConstantExpression TargetMemberTypeExpression = Expression.Constant(TargetMemberType);
        ConstantExpression IgnoreCase = Expression.Constant(true, typeof(bool));
        //Create an expression the calls the Parse method
        MethodCallExpression CallExpression = Expression.Call(EnumParseMethod, new[] { TargetMemberTypeExpression, RecordFieldExpression, IgnoreCase });
        return CallExpression;
    }

    /// <summary>
    /// Creates an Expression that parses a string to Char or Boolean
    /// </summary>
    /// <param name="RecordFieldExpression"></param>
    /// <param name="TargetMemberType"></param>
    /// <returns></returns>
    private static MethodCallExpression GetGenericParseExpression(Expression RecordFieldExpression, Type TargetMemberType)
    {
        MethodInfo ParseMetod = TargetMemberType.GetMethod("Parse", new[] { typeof(string) });
        MethodCallExpression CallExpression = Expression.Call(ParseMetod, new[] { RecordFieldExpression });
        return CallExpression;
    }

    /// <summary>
    /// Creates an Expression that parses a string to a number
    /// </summary>
    /// <param name="RecordFieldExpression"></param>
    /// <param name="TargetMemberType"></param>
    /// <param name="Provider"></param>
    /// <returns></returns>
    private static MethodCallExpression GetNumberParseExpression(Expression RecordFieldExpression, Type TargetMemberType,  CultureInfo Culture)
    {
        MethodInfo ParseMetod = TargetMemberType.GetMethod("Parse", new[] { typeof(string), typeof(NumberFormatInfo) });
        ConstantExpression ProviderExpression = Expression.Constant(Culture.NumberFormat, typeof(NumberFormatInfo));
        MethodCallExpression CallExpression = Expression.Call(ParseMetod, new[] { RecordFieldExpression, ProviderExpression });
        return CallExpression;
    }

    /// <summary>
    /// Creates an Expression that parses a string to a DateTime
    /// </summary>
    /// <param name="RecordFieldExpression"></param>
    /// <param name="TargetMemberType"></param>
    /// <param name="Culture"></param>
    /// <returns></returns>
    private static MethodCallExpression GetDateTimeParseExpression(Expression RecordFieldExpression, Type TargetMemberType, CultureInfo Culture)
    {
        MethodInfo ParseMetod = TargetMemberType.GetMethod("Parse", new[] { typeof(string), typeof(DateTimeFormatInfo) });
        ConstantExpression ProviderExpression = Expression.Constant(Culture.DateTimeFormat, typeof(DateTimeFormatInfo));
        MethodCallExpression CallExpression = Expression.Call(ParseMetod, new[] { RecordFieldExpression, ProviderExpression });
        return CallExpression;
    }

    /// <summary>
    /// Creates an Expression that parses a string
    /// </summary>
    /// <param name="RecordFieldExpression"></param>
    /// <param name="TargetMemberType"></param>
    /// <param name="Provider"></param>
    /// <returns></returns>
    private static Expression GetParseExpression(Expression RecordFieldExpression, Type TargetMemberType, CultureInfo Culture)
    {
        Type UnderlyingType = GetUnderlyingType(TargetMemberType);
        if (UnderlyingType.IsEnum)
        {
            MethodCallExpression ParsedEnumExpression = GetEnumParseExpression(RecordFieldExpression, UnderlyingType);
            //Enum.Parse returns an object that needs to be unboxed
            return Expression.Unbox(ParsedEnumExpression, TargetMemberType);
        }
        else
        {
            Expression ParseExpression = default(Expression);
            switch (UnderlyingType.FullName)
            {
                case "System.Byte":
                case "System.UInt16":
                case "System.UInt32":
                case "System.UInt64":
                case "System.SByte":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.Double":
                case "System.Decimal":
                    ParseExpression = GetNumberParseExpression(RecordFieldExpression, UnderlyingType, Culture);
                    break;
                case "System.DateTime":
                    ParseExpression = GetDateTimeParseExpression(RecordFieldExpression, UnderlyingType, Culture);
                    break;
                case "System.Boolean":
                case "System.Char":
                    ParseExpression = GetGenericParseExpression(RecordFieldExpression, UnderlyingType);
                    break;
                default:
                    throw new ArgumentException(string.Format("Conversion from {0} to {1} is not supported", "String", TargetMemberType));
            }
            if (Nullable.GetUnderlyingType(TargetMemberType) == null)
            {
                return ParseExpression;
            }
            else
            {
                //Convert to nullable if necessary
                return Expression.Convert(ParseExpression, TargetMemberType);
            }
        }
    }

    /// <summary>
    /// Returns Underlying Type if it's a nullable, otherwise the type itself
    /// </summary>
    /// <param name="Type"></param>
    /// <returns></returns>
    private static Type GetUnderlyingType(Type Type)
    {
        if (Nullable.GetUnderlyingType(Type) == null)
        {
            return Type;
        }
        else
        {
            return Nullable.GetUnderlyingType(Type);
        }
    }
    #endregion

    #region"Mapper"

    /// <summary>
    /// Get's default value of T
    /// </summary>
    /// <param name="T">The Type you want the default value for</param>
    /// <returns>Default value</returns>
    private static object GetDefaultValue(Type T)
    {
        if (T.IsValueType && Nullable.GetUnderlyingType(T) == null)
        {
            return Activator.CreateInstance(T);
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Gets an Expression that checks if the current RecordField is null
    /// </summary>
    /// <param name="SourceType">The Type of the Record</param>
    /// <param name="SourceInstance">The Record instance</param>
    /// <param name="i">The index of the parameter</param>
    /// <returns>MethodCallExpression</returns>
    private static MethodCallExpression GetNullCheckExpression(Type SourceType, ParameterExpression SourceInstance, int i)
    {
        MethodInfo GetNullValueMethod = SourceType.GetMethod("IsDBNull", new Type[] { typeof(int) });
        MethodCallExpression NullCheckExpression = Expression.Call(SourceInstance, GetNullValueMethod, Expression.Constant(i, typeof(int)));
        return NullCheckExpression;
    }

    /// <summary>
    /// Gets an Expression that represents the getter method for the RecordField
    /// </summary>
    /// <param name="SourceType">The Type of the Record</param>
    /// <param name="SourceInstance">The Record instance</param>
    /// <param name="i">The index of the parameter</param>
    /// <param name="RecordFieldType">The Type of the RecordField</param>
    /// <returns></returns>
    private static Expression GetRecordFieldExpression(Type SourceType, ParameterExpression SourceInstance, int i, Type RecordFieldType)
    {
        string MethodName = "Get" + RecordFieldType.Name;
        MethodInfo GetValueMethod = null;

        if (RecordFieldType.Equals(typeof(object)))
        {
            GetValueMethod = SourceType.GetMethod("GetValue", new Type[] { typeof(int) });
        }
        else
        {
            GetValueMethod = SourceType.GetMethod(MethodName, new Type[] { typeof(int) });
        }
        Expression RecordFieldExpression = Expression.Call(SourceInstance, GetValueMethod, Expression.Constant(i, typeof(int)));
        return RecordFieldExpression;
    }

    /// <summary>
    /// Returns The Type of the TargetMember
    /// </summary>
    /// <param name="TargetMember"></param>
    /// <returns></returns>
    private static Type GetTargetMemberType(MemberInfo TargetMember)
    {
        switch (TargetMember.MemberType)
        {
            case MemberTypes.Field:
                return ((FieldInfo)TargetMember).FieldType;
            case MemberTypes.Property:
                return ((PropertyInfo)TargetMember).PropertyType;
            default:
                return null;
        }
    }

    /// <summary>
    /// Returns an Expression representing the value to set the TargetProperty to
    /// </summary>
    /// <param name="SourceInstance">The Record instance</param>
    /// <param name="SourceType">The Type of the Record</param>
    /// <param name="TargetMemberType">The Type of the TargetMember</param>
    /// <param name="RecordFieldType">The Type of the RecordField</param>
    /// <param name="i">The index of the parameter</param>
    /// <param name="AllowDBNull">Whether the sourcefield in the database Allows null or not</param>
    private static Expression GetTargetValueExpression(ParameterExpression SourceInstance, 
        Type SourceType, 
        Type TargetMemberType, 
        Type RecordFieldType, 
        int i, 
        bool AllowDBNull, 
        CultureInfo Culture)
    {
        //Create an expression that assigns the converted value to the target
        Expression TargetValueExpression = default(Expression);
        Expression RecordFieldExpression = GetRecordFieldExpression(SourceType, SourceInstance, i, RecordFieldType);
        Expression ConvertedRecordFieldExpression = GetConvertedRecordFieldExpression(RecordFieldType, RecordFieldExpression, TargetMemberType, Culture);

        if (AllowDBNull)
        {
            ParameterExpression Value = Expression.Variable(TargetMemberType, "Value");
            TargetValueExpression = Expression.Block(
                new ParameterExpression[] { Value },
                Expression.IfThenElse(
                    GetNullCheckExpression(SourceType, SourceInstance, i),
                    Expression.Assign(Value, Expression.Constant(GetDefaultValue(TargetMemberType), TargetMemberType)),
                    Expression.Assign(Value, ConvertedRecordFieldExpression)
                    ),
                Expression.Convert(Value, TargetMemberType)
            );
        }
        else
        {
            TargetValueExpression = ConvertedRecordFieldExpression;
        }
        return TargetValueExpression;
    }

    /// <summary>
    /// Checks if the RecordField matches the TargetMember
    /// </summary>
    /// <param name="RecordFieldName">The Name of the RecordField</param>
    /// <param name="TargetMember">The Member of the TargetInstance to check</param>
    /// <returns>True if Fields match</returns>
    /// <remarks>FieldNameAttribute takes precedence over TargetMembers name.</remarks>
    private static bool FieldsMatch(string RecordFieldName, MemberInfo TargetMember)
    {
        string Fieldname = string.Empty;
        switch (TargetMember.MemberType)
        {
            case MemberTypes.Field:
                FieldInfo FieldInfo = (FieldInfo)TargetMember;
                if (FieldInfo.GetCustomAttributes(typeof(FieldNameAttribute), true).Count() > 0)
                {
                    Fieldname = ((FieldNameAttribute)FieldInfo.GetCustomAttributes(typeof(FieldNameAttribute), true)[0]).FieldName;
                }
                break;
            case MemberTypes.Property:
                if (((PropertyInfo)TargetMember).CanWrite)
                {
                    PropertyInfo PropertyInfo = (PropertyInfo)TargetMember;
                    if (PropertyInfo.GetCustomAttributes(typeof(FieldNameAttribute), true).Count() > 0)
                    {
                        Fieldname = ((FieldNameAttribute)PropertyInfo.GetCustomAttributes(typeof(FieldNameAttribute), true)[0]).FieldName;
                    }
                }
                else
                {
                    return false;
                }
                break;
        }
        return Fieldname.ToLower() == RecordFieldName.ToLower() || TargetMember.Name.ToLower() == RecordFieldName.ToLower();
    }

    /// <summary>
    /// Creates a delegate that creates an instance of Target from the supplied DataRecord
    /// </summary>
    /// <param name="RecordInstance">An instance of a DataRecord</param>
    /// <returns>A Delegate that creates a new instance of Target with the values set from the supplied DataRecord</returns>
    private static Func<IDataRecord, Target> GetInstanceCreator<Target>(IDataRecord RecordInstance, CultureInfo Culture) where Target : class, new()
    {
        List<MemberBinding> Bindings = new List<MemberBinding>();
        Type SourceType = typeof(IDataRecord);
        ParameterExpression SourceInstance = Expression.Parameter(SourceType, "SourceInstance");
        Type TargetType = typeof(Target);
        DataTable SchemaTable = ((IDataReader)RecordInstance).GetSchemaTable();

        //Loop through the Properties in the Target and the Fields in the Record to check which ones are matching
        for (int i = 0; i < RecordInstance.FieldCount; i++)
        {
            foreach (MemberInfo TargetMember in TargetType.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.ExactBinding))
            {
                //Check if the RecordFieldName matches the TargetMember
                if (FieldsMatch(RecordInstance.GetName(i), TargetMember))
                {
                    Type RecordFieldType = RecordInstance.GetFieldType(i);
                    bool AllowDBNull = Convert.ToBoolean(SchemaTable.Rows[i]["AllowDBNull"]);
                    Type TargetMemberType = GetTargetMemberType(TargetMember);
                    Expression TargetValueExpression = GetTargetValueExpression(SourceInstance, SourceType, TargetMemberType, RecordFieldType, i, AllowDBNull, Culture);

                    //Create a binding to the target member
                    MemberAssignment BindExpression = Expression.Bind(TargetMember, TargetValueExpression);
                    Bindings.Add(BindExpression);
                    break;
                }
            }
        }
        //Create a memberInitExpression that Creates a new instance of Target using bindings to the DataRecord
        MemberInitExpression Body = Expression.MemberInit(Expression.New(TargetType), Bindings);
        //Compile the Expression to a Delegate
        return Expression.Lambda<Func<IDataRecord, Target>>(Body, SourceInstance).Compile();
    }

    #endregion
    #endregion

    #region "Nested Classes"

    private class ReaderField
    {
        private readonly Type _fieldType;

        private readonly string _fieldName;
        public ReaderField(string FieldName, Type FieldType)
        {
            _fieldName = FieldName;
            _fieldType = FieldType;
        }
    }

    private class TypeList : List<ReaderField>
    {

        public TypeList(CultureInfo Culture)
        {
            this.Culture = Culture;
        }

        public CultureInfo Culture { get; set; }

        public override bool Equals(object obj)
        {
            TypeList list = obj as TypeList;
            if (list == null)
            {
                return false;
            }
            else
            {
                return this.Culture.Equals(list.Culture) && Enumerable.SequenceEqual<ReaderField>(this, list);
            }
        }

        public override int GetHashCode()
        {
            Int64 hash = Culture.GetHashCode();
            int i = 0;
            foreach (ReaderField Item in this)
            {
                hash = hash * 31 + Item.GetHashCode();
                i += 1;
                if (i < 7)
                {
                    hash = unchecked((Int32)(hash ^ hash >> 32));
                    i = 0;
                }
            }
            return unchecked((Int32)(hash ^ hash >> 32));
        }
    }

    private sealed class Mapper<Target> : Dictionary<Int32, Func<IDataRecord, Target>> where Target : class, new()
    {
        private Mapper()
        {
        }
        private static readonly object SyncRoot = new object();

        private static readonly Dictionary<TypeList, Func<IDataRecord, Target>> _Creators = new Dictionary<TypeList, Func<IDataRecord, Target>>();
        private static TypeList GetTypeList(IDataRecord RecordInstance, CultureInfo Culture)
        {
            TypeList List = new TypeList(Culture);
            ReaderField Field = null;
            for (int i = 0; i < RecordInstance.FieldCount; i++)
            {
                Field = new ReaderField(RecordInstance.GetName(i), RecordInstance.GetFieldType(i));
                List.Add(Field);
            }
            return List;
        }

        static internal Func<IDataRecord, Target> GetCreator(IDataRecord RecordInstance, CultureInfo Culture)
        {
            TypeList List = GetTypeList(RecordInstance, Culture);
            if (!_Creators.ContainsKey(List))
            {
                lock (SyncRoot)
                {
                    if (!_Creators.ContainsKey(List))
                    {
                        _Creators.Add(List, GetInstanceCreator<Target>(RecordInstance, Culture));
                    }
                }
            }
            return _Creators[List];
        }
    }
    #endregion
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
class FieldNameAttribute : Attribute
{


    private readonly string _FieldName;
    public string FieldName
    {
        get { return _FieldName; }
    }

    public FieldNameAttribute(string FieldName)
    {
        _FieldName = FieldName;
    }
}
}
