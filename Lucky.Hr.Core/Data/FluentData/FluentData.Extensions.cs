using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Data;
namespace Lucky.Hr.Core
{
    public partial interface  IDbContext : IDisposable
    {
        SqlBulkCopy CreateSqlCopy();
        IInsertBuilder<T> Insert<T>( T item);
        IDbContext CreateTransaction();
        IDbContext CreateConnection(DbConnection connection, IDbProvider dbProvider);
        IUpdateBuilder<T> Update<T>(T item);
    }
    public partial class DbContext : IDbContext
    {
        public SqlBulkCopy CreateSqlCopy()
        {
            IDbConnection connection = null;
            if (Data.UseTransaction)
            {
                if (Data.Connection == null)
                {
                    Data.Connection = Data.Provider.CreateConnection();
                    Data.Connection.ConnectionString=Data.ConnectionString;
                }
                connection = Data.Connection;
            }
            else
            {
                Data.Connection = Data.Provider.CreateConnection();
                Data.Connection.ConnectionString = Data.ConnectionString;
            }
            connection.Open();
            SqlBulkCopy sqlcopy = new SqlBulkCopy((SqlConnection)connection);
            return sqlcopy;
        }

        public IInsertBuilder<T> Insert<T>(T item)
        {
            return new InsertBuilder<T>(CreateCommand, typeof(T).Name, item);
        }
        public IUpdateBuilder<T> Update<T>(T item)
        {
            return new UpdateBuilder<T>(Data.Provider, CreateCommand, typeof(T).Name, item);
        }
        public IDbContext CreateTransaction()
        {
            if (Data.Connection.State != ConnectionState.Open)
                Data.Connection.Open();
            Data.Transaction = Data.Connection.BeginTransaction((System.Data.IsolationLevel)Data.IsolationLevel);
            return this;
        }
        public IDbContext CreateConnection(DbConnection connection, IDbProvider dbProvider)
        {
            Data.Connection = connection;
            Data.Provider = dbProvider;
            return this;
        }
    }
    public partial interface IDbCommand
    {
        List<TEntity> GetQuery<TEntity>();
        TEntity Single<TEntity>();
        DataTable GetTable();
        DataTable GetTablePaged(int PageSize, int PageIndex, out int Total);
        List<T> Paged<T>(int PageSize, int PageIndex, out int Total);
        List<dynamic> Paged(int PageSize, int PageIndex, out int Total);
	}
    internal partial class DbCommand
    {
        public DataTable GetTable()
        {
            DataTable items = null;
            Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
            {
                items = new DynamicQueryHandler().GetTable(Data);
            });
            return items;
        }
        public DataTable GetTablePaged(int PageSize, int PageIndex, out int Total)
        {
            DataTable items = null;
            int _total = 0;
            Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
            {
                items = new DynamicQueryHandler().GetTablePaged(Data, PageSize, PageIndex, out _total);
            });
            Total = _total;
            return items;
        }
       
        public List<dynamic> Paged(int PageSize, int PageIndex, out int Total)
        {
            List<dynamic> items = null;
            int _total = 0;
            Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
            {
                items = new DynamicQueryHandler().ExecutePagedList(Data, PageSize, PageIndex, out _total);
            });
            Total = _total;
            return items;
        }
        public List<T> Paged<T>(int PageSize, int PageIndex, out int Total)
        {
            List<T> items = null;
            int _total = 0;
            Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
            {
                items = new DynamicQueryHandler().PagedList<T>(Data, PageSize, PageIndex, out _total);
            });
            Total = _total;
            return items;
        }
        public TEntity Single<TEntity>()
        {
            var item = default(TEntity);
            Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
                                                            {
                                                                item = new DynamicQueryHandler().Single<TEntity>(Data);
                                                            });
            return item;
        }
        public List<TEntity> GetQuery<TEntity>()
        {
            var items = new List<TEntity>();
            Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
                                                            {
                                                                items = new DynamicQueryHandler().GetQuery<TEntity>(Data);
                                                            });
            return items;
        }
    }
    internal partial class DynamicQueryHandler
    {
        public DataTable GetTable(DbCommandData Data)
        {
            DataTable dt;
            dt = new DataTable();
            int colCount = Data.Reader.FieldCount;
            object[] vald = new object[colCount];
            for (int i = 0; i < colCount; i++)
            {
                dt.Columns.Add(new DataColumn(Data.Reader.GetName(i), Data.Reader.GetFieldType(i)));
            }
            while (Data.Reader.Read())
            {
                for (int i = 0; i < colCount; i++)
                    vald[i] = Data.Reader.GetValue(i);

                dt.Rows.Add(vald);
            }
            return dt;
        }
        public DataTable GetTablePaged(DbCommandData Data,int PageSize, int PageIndex, out int Total)
        {
            int iCount = 0;
            DataTable dt;
            dt = new DataTable();
            int colCount = Data.Reader.FieldCount;
            object[] vald = new object[colCount];
            for (int i = 0; i < colCount; i++)
            {
                dt.Columns.Add(new DataColumn(Data.Reader.GetName(i), Data.Reader.GetFieldType(i)));
            }
            while (Data.Reader.Read())
            {
                if (iCount >= PageSize * (PageIndex - 1) && iCount < PageSize * PageIndex)
                {
                    for (int i = 0; i < colCount; i++)
                        vald[i] = Data.Reader.GetValue(i);

                    dt.Rows.Add(vald);
                }
                iCount++; // 临时记录变量递增
            }
            Total = iCount;
            return dt;
        }
        public List<dynamic> ExecutePagedList(DbCommandData Data,int PageSize, int PageIndex, out int Total)
        {
            int iCount = 0; // 临时记录变量
            var items = new List<dynamic>();
            var autoMapper = new DynamicTypeAutoMapper(Data.Reader);
            while (Data.Reader.Read())
            {
                // 当前记录在当前页记录范围内
                if (iCount >= PageSize * (PageIndex - 1) && iCount < PageSize * PageIndex)
                {
                    var item = autoMapper.AutoMap();
                    items.Add(item);
                }
                iCount++; // 临时记录变量递增
            }
            Total = iCount;
            return items;
        }
        public List<T> PagedList<T>(DbCommandData Data, int PageSize, int PageIndex, out int Total)
        {
            int iCount = 0; // 临时记录变量
            var items = new List<T>();
            EntityBuilder<T> _eb = EntityBuilder<T>.CreateBuilder((Lucky.Hr.Core.DataReader)Data.Reader);
            while (Data.Reader.Read())
            {
                // 当前记录在当前页记录范围内
                if (iCount >= PageSize * (PageIndex - 1) && iCount < PageSize * PageIndex)
                {
                    items.Add(_eb.Build((Lucky.Hr.Core.DataReader)Data.Reader));
                }
                iCount++; // 临时记录变量递增
            }
            Total = iCount;
            return items;
        }

        public TEntity Single<TEntity>(DbCommandData Data)
        {
            var item = default(TEntity);
            EntityBuilder<TEntity> _eb = EntityBuilder<TEntity>.CreateBuilder((Lucky.Hr.Core.DataReader)Data.Reader);
            while (Data.Reader.Read())
            {
                item = _eb.Build((Lucky.Hr.Core.DataReader)Data.Reader);
            }
            return item;
        }
        public List<TEntity> GetQuery<TEntity>(DbCommandData Data)
        {
            var items = new List<TEntity>();
            EntityBuilder<TEntity> _eb = EntityBuilder<TEntity>.CreateBuilder((Lucky.Hr.Core.DataReader)Data.Reader);
            while (Data.Reader.Read())
            {
                items.Add(_eb.Build((Lucky.Hr.Core.DataReader)Data.Reader));
            }
            return items;
        }
    }
    public partial interface IBaseStoredProcedureBuilder
    {
        DataTable GetTable();
        DataTable GetTablePaged(int PageSize, int PageIndex, out int Total);
        List<dynamic> Paged(int PageSize, int PageIndex, out int Total);
    }
    internal abstract partial class BaseStoredProcedureBuilder
    {
        public DataTable GetTable()
        {
            return Data.Command.GetTable();
        }
        public DataTable GetTablePaged(int PageSize, int PageIndex, out int Total)
        {
            return Data.Command.GetTablePaged(PageSize, PageIndex, out Total);
        }
        public List<dynamic> Paged(int PageSize, int PageIndex, out int Total)
        {
            return Data.Command.Paged(PageSize, PageIndex, out Total);
        }
    }
}
