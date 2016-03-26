using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;

namespace Lucky.Hr.Core
{
    public class EntityBuilder<TEntity>
    {
        private static readonly MethodInfo GetValueMethod =
        typeof(IDataRecord).GetMethod("get_Item", new[] { typeof(int) });
        private static readonly MethodInfo IsDbNullMethod =
            typeof(IDataRecord).GetMethod("IsDBNull", new[] { typeof(int) });
        private delegate TEntity Load(IDataRecord dataRecord);

        private Load _handler;
        private EntityBuilder() { }
        public TEntity Build(IDataRecord dataRecord)
        {
            return _handler(dataRecord);
        }

        public TEntity Build(TEntity entity, IDataRecord dataRecord)
        {
            return _handler(dataRecord);
        }
        public static EntityBuilder<TEntity> CreateBuilder(IDataRecord dataRecord)
        {
            var dynamicBuilder = new EntityBuilder<TEntity>();
            var method = new DynamicMethod("DynamicCreateEntity", typeof(TEntity),
                    new[] { typeof(IDataRecord) }, typeof(TEntity), true);
            ILGenerator generator = method.GetILGenerator();
            LocalBuilder result = generator.DeclareLocal(typeof(TEntity));
            generator.Emit(OpCodes.Newobj, typeof(TEntity).GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);
            for (int i = 0; i < dataRecord.FieldCount; i++)
            {
                PropertyInfo propertyInfo = typeof(TEntity).GetProperty(dataRecord.GetName(i));
                Label endIfLabel = generator.DefineLabel();
                if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                {
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, IsDbNullMethod);
                    generator.Emit(OpCodes.Brtrue, endIfLabel);
                    generator.Emit(OpCodes.Ldloc, result);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, GetValueMethod);
                    generator.Emit(OpCodes.Unbox_Any, dataRecord.GetFieldType(i));
                    generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
                    generator.MarkLabel(endIfLabel);
                }
            }
            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);
            dynamicBuilder._handler = (Load)method.CreateDelegate(typeof(Load));
            return dynamicBuilder;
        }
    }

    public static class TableConvert
    {
        public static List<T> GetTableToList<T>(this DataTable table)
        {
            List<T> list=new List<T>();
            IDataReader dr = table.CreateDataReader();
            EntityBuilder<T> eb=EntityBuilder<T>.CreateBuilder(dr);
            while (dr.Read())
                list.Add(eb.Build(dr));
            return list;
        }
        public static List<T> GetDataReaderList<T>(this IDataReader dr)
        {
            List<T> list = new List<T>();
            EntityBuilder<T> eb = EntityBuilder<T>.CreateBuilder(dr);
            while (dr.Read())
                list.Add(eb.Build(dr));
            return list;
        }
    }
    
}
