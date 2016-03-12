using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;


namespace Lucky.Hr.Core.Data.Dapper
{
    public interface IDbConnectionFactory
    {
        IDbConnection Create();
        IDbConnection CreateAndOpen();
    }

    /// <summary>
    /// Default implementation of ConnectionFactory
    /// used by DbContext
    /// </summary>
    public class DbConnectionFactory : IDbConnectionFactory
    {
        readonly DbProviderFactory _factory;
        readonly string _conectionString;

        /// <summary>
        /// connectionStringName: 
        ///		connection string name defined under connections section of the config file.
        /// The connection element must have propper DbProviderFactory defined
        /// </summary>
        public DbConnectionFactory(string connectionName)
            : this(ConfigurationManager.ConnectionStrings.GetByName(connectionName))
        { }

        protected DbConnectionFactory(ConnectionStringSettings connectionSetting)
        {
            _factory = connectionSetting.CreatDbProviderFactory();
            _conectionString = connectionSetting.ConnectionString;
        }

        public virtual IDbConnection Create()
        {
            return _factory.CreateConnection(_conectionString);
        }

        public virtual IDbConnection CreateAndOpen()
        {
            var con = Create();
            con.Open();
            return con;
        }
    }
    static class DbConnectionFactoryHelpers
    {
        /// <summary>
        /// Creates connection
        /// </summary>
        internal static IDbConnection CreateConnection(this DbProviderFactory sender, string connectionString)
        {
            var connection = sender.CreateConnection();
            connection.ConnectionString = connectionString;
            return connection;
        }

        /// <summary>
        /// Creates and opens connection
        /// </summary>
        internal static IDbConnection CreateOpenedConnection(this DbProviderFactory sender, string connectionString)
        {
            var connection = sender.CreateConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Get connection by name form Connection Configuration Collection
        /// </summary>
        public static ConnectionStringSettings GetByName(this ConnectionStringSettingsCollection sender, string connectionStringName)
        {
            var connectionSetting = sender[connectionStringName];
            if (connectionSetting == null)
            { throw new InvalidOperationException(string.Format("Can't find a connection string with the name '{0}'", connectionStringName)); }
            return connectionSetting;
        }

        /// <summary>
        /// Creates an instance of DbProviderFactory defined in config file in connection setting
        /// </summary>
        public static DbProviderFactory CreatDbProviderFactory(this ConnectionStringSettings sender)
        {
            try
            {
                return DbProviderFactories.GetFactory(sender.ProviderName);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(sender.ProviderName, ex);
            }
        }
    }
    public interface IDbCommand
    {
        int Execute(string sql, object param = null, CommandType? commandType = null, int? commandTimeout = 0);

        IEnumerable<T> Query<T>(string sql, object param = null, CommandType? commandType = null, int? commandTimeout = 0);

        IEnumerable<dynamic> Query(string sql, object param = null, CommandType? commandType = null, int? commandTimeout = 0);

        IEnumerable<dynamic> Query(System.Type type, string sql, object param = null, System.Data.CommandType? commandType = null, int? commandTimeout = 0);
    }
    public partial interface ISqlAdapter
    {
        int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, String tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert);
    }
    /// <summary>
    /// Default behavior exposed by DbContext helps with injection
    /// </summary>
    public interface IDbContext : IDbCommand
    {
        void Batch(Action<ISession> action);
        TResult Batch<TResult>(Func<ISession, TResult> func);
    }

    /// <summary>
    /// Interface to help with transaction managment
    /// </summary>
    public interface ISession : IDbCommand
    {
        void BeginTransaction();
        void BeginTransaction(System.Data.IsolationLevel il);
        void CommitTransaction();
        void RollbackTransaction();

        IDbConnection Connection { get; }

        int Execute(CommandDefinition definition);

        IEnumerable<T> Query<T>(CommandDefinition definition);
        int? Insert<T>(dynamic data);
        int Update<T>(dynamic data);
        bool Delete<T>(object id);
        IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = null, int? commandTimeout = 0);
        IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = null, int? commandTimeout = 0);
        IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = null, int? commandTimeout = 0);
        IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = null, int? commandTimeout = 0);

        SqlMapper.GridReader QueryMultiple(CommandDefinition command);
        SqlMapper.GridReader QueryMultiple(string sql, dynamic param = null, System.Data.CommandType? commandType = null, int? commandTimeout = 0);
    }

    /// <summary>
    /// Light weight DbContext implementation based on dapper
    /// Use it to create your own DbContext
    /// It will help manage connection life time and transactions
    /// </summary>
    public abstract class DapperContext : IDbContext
    {
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> KeyProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> TypeProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();

        public DapperContext(string connectionName)
            : this(new DbConnectionFactory(connectionName))
        { }

        public DapperContext(IDbConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        public virtual IDbConnectionFactory ConnectionFactory
        {
            get;
            private set;
        }

        /// <summary>
        /// Enables execution of multiple statements and helps with
        /// transaction management
        /// </summary>
        public virtual void Batch(Action<ISession> action)
        {
            using (var con = ConnectionFactory.CreateAndOpen())
            {
                try
                {
                    action(new Session(con));
                }
                finally
                {
                    con.Close();
                }
            }
        }

        /// <summary>
        /// Enables execution of multiple statements and helps with
        /// transaction management
        /// </summary>
        public virtual TResult Batch<TResult>(Func<ISession, TResult> func)
        {
            using (var con = ConnectionFactory.CreateAndOpen())
            {
                try
                {
                    return func(new Session(con));
                }
                finally
                {
                    con.Close();
                }
            }
        }

        class Session : ISession
        {
            readonly IDbConnection _connection;
            IDbTransaction _transaction;

            public Session(IDbConnection connection)
            {
                _connection = connection;
                _transaction = null;
            }

            public void BeginTransaction()
            {
                if (_transaction == null)
                { _transaction = _connection.BeginTransaction(); }
            }

            public void BeginTransaction(System.Data.IsolationLevel il)
            {
                if (_transaction == null)
                { _transaction = _connection.BeginTransaction(il); }
            }

            public void CommitTransaction()
            {
                if (_transaction != null)
                {
                    _transaction.Commit();
                }
                _transaction = null;
            }

            public void RollbackTransaction()
            {
                if (_transaction != null)
                {
                    _transaction.Rollback();
                }
                _transaction = null;
            }

            public IDbConnection Connection { get { return _connection; } }

            public int Execute(CommandDefinition command)
            {
                return _connection.Execute(command);
            }
            /// <summary>
            /// Insert a row into the db
            /// </summary>
            /// <param name="data">Either DynamicParameters or an anonymous type or concrete type</param>
            /// <returns></returns>
            public virtual int? Insert<T>(dynamic data)
            {
                var o = (object)data;
                List<string> paramNames = GetParamNames(o);
                paramNames.Remove("Id");
                string TableName = GetTableName(typeof(T));

                string cols = string.Join(",", paramNames);
                string cols_params = string.Join(",", paramNames.Select(p => "@" + p));
                var sql = "set nocount on insert " + TableName + " (" + cols + ") values (" + cols_params + ") select cast(scope_identity() as int)";

                return _connection.Query<int?>(sql, o).Single();
            }

            /// <summary>
            /// Update a record in the DB
            /// </summary>
            /// <param name="id"></param>
            /// <param name="data"></param>
            /// <returns></returns>
            public int Update<T>(dynamic data)
            {
                List<string> paramNames = GetParamNames((object)data);
                var keys = KeyPropertiesCache(typeof(T));

                if (keys.Count < 1)
                    throw new Exception("必须要有主键");
                string sql = "";
                for (var i = 0; i < keys.Count(); i++)
                {
                    var property = keys.ElementAt(i);
                    sql = sql + String.Format("{0} = @{1}", property.Name, property.Name);
                    if (i < keys.Count() - 1)
                        sql = sql + (" and ");
                }
                string TableName = GetTableName(typeof(T));
                var builder = new StringBuilder();
                builder.Append("update ").Append(TableName).Append(" set ");
                builder.AppendLine(string.Join(",", paramNames.Where(a => a != keys[0].Name).Select(p => p + "= @" + p)));
                builder.Append("where " + sql);

                DynamicParameters parameters = new DynamicParameters(data);
                return _connection.Execute(builder.ToString(), parameters);
            }
            /// <summary>
            /// Delete a record for the DB
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public bool Delete<T>(object id)
            {
                var keys = KeyPropertiesCache(typeof(T));

                if (keys.Count < 1)
                    throw new Exception("必须要有主键");
                string TableName = GetTableName(typeof(T));
                return _connection.Execute("delete from " + TableName + " where " + keys[0].Name + " = @id", new { id }) > 0;
            }
            public IEnumerable<T> Query<T>(CommandDefinition command)
            {
                return _connection.Query<T>(command);
            }

            public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = null, int? commandTimeout = 0)
            {
                return _connection.Query(sql, map, param, _transaction, buffered, splitOn, commandTimeout, commandType);
            }

            public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = null, int? commandTimeout = 0)
            {
                return _connection.Query(sql, map, param, _transaction, buffered, splitOn, commandTimeout, commandType);
            }

            public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = null, int? commandTimeout = 0)
            {
                return _connection.Query(sql, map, param, _transaction, buffered, splitOn, commandTimeout, commandType);
            }

            public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, bool buffered = true, string splitOn = "Id", CommandType? commandType = null, int? commandTimeout = 0)
            {
                return _connection.Query(sql, map, param, _transaction, buffered, splitOn, commandTimeout, commandType);
            }

            public int Execute(string sql, object param = null, CommandType? commandType = null, int? commandTimeout = 0)
            {
                return _connection.Execute(sql, param, _transaction, commandTimeout, commandType);
            }

            public IEnumerable<T> Query<T>(string sql, object param = null, CommandType? commandType = null, int? commandTimeout = 0)
            {
                if (typeof(T) == typeof(IDictionary<string, object>))
                {
                    return _connection.Query(sql, param, _transaction, true, commandTimeout, commandType).OfType<T>();
                }
                return _connection.Query<T>(sql, param, _transaction, true, commandTimeout, commandType);
            }

            public IEnumerable<dynamic> Query(string sql, object param = null, CommandType? commandType = null, int? commandTimeout = 0)
            {
                return _connection.Query(sql, param, null, true, commandTimeout, commandType);
            }

            public IEnumerable<dynamic> Query(Type type, string sql, object param = null, CommandType? commandType = null, int? commandTimeout = 0)
            {
                return _connection.Query(type, sql, param, _transaction, true, commandTimeout, commandType);
            }

            public SqlMapper.GridReader QueryMultiple(CommandDefinition command)
            {
                return _connection.QueryMultiple(command);
            }

            public SqlMapper.GridReader QueryMultiple(string sql, dynamic param = null, System.Data.CommandType? commandType = null, int? commandTimeout = 0)
            {
                return _connection.QueryMultiple(new CommandDefinition(sql, param, _transaction, commandTimeout, commandType));
            }

        }

        public int Execute(string sql, object param = null, CommandType? commandType = null, int? commandTimeout = 0)
        {
            return Batch(s => s.Execute(sql, param, commandType, commandTimeout));
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, CommandType? commandType = null, int? commandTimeout = 0)
        {
            return Batch(s => s.Query<T>(sql, param, commandType, commandTimeout));
        }

        public IEnumerable<dynamic> Query(string sql, object param = null, CommandType? commandType = null, int? commandTimeout = 0)
        {
            return Batch(s => s.Query(sql, param, commandType, commandTimeout));
        }

        public IEnumerable<object> Query(Type type, string sql, object param = null, CommandType? commandType = null, int? commandTimeout = 0)
        {
            return Batch(s => s.Query(type, sql, param, commandType, commandTimeout));
        }
        static ConcurrentDictionary<Type, List<string>> paramNameCache = new ConcurrentDictionary<Type, List<string>>();

        internal static List<string> GetParamNames(object o)
        {
            if (o is DynamicParameters)
            {
                return (o as DynamicParameters).ParameterNames.ToList();
            }

            List<string> paramNames;
            if (!paramNameCache.TryGetValue(o.GetType(), out paramNames))
            {
                paramNames = new List<string>();
                foreach (var prop in o.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public))
                {
                    var attribs = prop.GetCustomAttributes(typeof(IgnorePropertyAttribute), true);
                    var attr = attribs.FirstOrDefault() as IgnorePropertyAttribute;
                    if (attr == null || (attr != null && !attr.Value))
                    {
                        paramNames.Add(prop.Name);
                    }
                }
                paramNameCache[o.GetType()] = paramNames;
            }
            return paramNames;
        }
        internal static string GetTableName(Type type)
        {
            string name;

            {
                name = type.Name + "s";
                if (type.IsInterface && name.StartsWith("I"))
                    name = name.Substring(1);

                //NOTE: This as dynamic trick should be able to handle both our own Table-attribute as well as the one in EntityFramework 
                var tableattr = type.GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name == "TableAttribute") as
                    dynamic;
                if (tableattr != null)
                    name = tableattr.Name;

            }
            return name;
        }
        internal static List<PropertyInfo> KeyPropertiesCache(Type type)
        {

            IEnumerable<PropertyInfo> pi;
            if (KeyProperties.TryGetValue(type.TypeHandle, out pi))
            {
                return pi.ToList();
            }

            var allProperties = TypePropertiesCache(type);
            var keyProperties = allProperties.Where(p => p.GetCustomAttributes(true).Any(a => a is KeyAttribute)).ToList();

            if (keyProperties.Count == 0)
            {
                var idProp = allProperties.FirstOrDefault(p => p.Name.ToLower() == "id");
                if (idProp != null)
                {
                    keyProperties.Add(idProp);
                }
            }

            KeyProperties[type.TypeHandle] = keyProperties;
            return keyProperties;
        }

        internal static List<PropertyInfo> TypePropertiesCache(Type type)
        {
            IEnumerable<PropertyInfo> pis;
            if (TypeProperties.TryGetValue(type.TypeHandle, out pis))
            {
                return pis.ToList();
            }

            var properties = type.GetProperties().ToArray();
            TypeProperties[type.TypeHandle] = properties;
            return properties.ToList();
        }
    }

}
