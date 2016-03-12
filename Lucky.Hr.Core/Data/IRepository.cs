using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Lucky.Hr.Core.Data.UnitOfWork;
using Lucky.Hr.Core.Specification;

namespace Lucky.Hr.Core.Data
{
    public interface IRepository<TEntity>
    {
        /// <summary>
        /// 根据主键取得单一实体
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        TEntity Single(object key);
        /// <summary>
        /// 异步获取单一实体，根据主键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<TEntity> SingleAsync(object key);

        /// <summary>
        /// 根据表达式查询单一实体
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="IsNoTracking"></param>
        /// <returns></returns>
        TEntity Single(Expression<Func<TEntity, bool>> predicate, bool IsNoTracking = true);
        /// <summary>
        /// 根据表达式查询单一实体异步实现
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="IsNoTracking"></param>
        /// <returns></returns>
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate, bool IsNoTracking = true);
        /// <summary>
        /// Gets single entity using specification
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        TEntity Single(ISpecification<TEntity> specification, bool IsNoTracking = true);

        Task<TEntity> SingleAsync(ISpecification<TEntity> specification, bool IsNoTracking = true);

        /// <summary>
        /// 取得所有实体IList 对象
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        IList<TEntity> GetList(bool IsTracking = false);
        Task<IList<TEntity>> GetListAsync(bool IsTracking = false);


        /// <summary>
        /// 取得所有实体IQueryable 对象
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        IQueryable<TEntity> GetQuery(bool IsNoTracking = true);

        Task<IQueryable<TEntity>> GetQueryAsync(bool IsNoTracking = true);
        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> predicate, bool IsNoTracking = true);
        IQueryable<TEntity> GetQueryOrderBy<PEntity>(Expression<Func<TEntity, bool>> predicate,Expression<Func<TEntity, PEntity>> sortBy, bool IsNoTracking = true);
        Task<IQueryable<TEntity>> GetQueryAsync(Expression<Func<TEntity, bool>> predicate, bool IsNoTracking = true);
        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        IQueryable<TEntity> GetQuery(ISpecification<TEntity> specification, bool IsNoTracking = true);

        Task<IQueryable<TEntity>> GetQueryAsync(ISpecification<TEntity> specification, bool IsNoTracking = true);
        /// <summary>
        /// Finds the specified expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        IList<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool IsNoTracking = true);

        Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, bool IsNoTracking = true);
        /// <summary>
        /// Finds the specified specification.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="specification">The specification.</param>
        /// <returns></returns>
        IList<TEntity> Find(ISpecification<TEntity> specification, bool IsNoTracking = true);

        Task<IList<TEntity>> FindAsync(ISpecification<TEntity> specification, bool IsNoTracking = true);
        /// <summary>
        /// Gets the paged.
        /// </summary>
        /// <typeparam name="PEntity">The type of the entity.</typeparam>
        /// <param name="sortBy">The sort by.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        PagedList<TEntity> GetPaged<PEntity>(Expression<Func<TEntity, PEntity>> sortBy, int pageIndex, int pageSize);

        Task<PagedList<TEntity>> GetPagedAsync<PEntity>(Expression<Func<TEntity, PEntity>> sortBy, int pageIndex, int pageSize);
        /// <summary>
        /// Gets the paged.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="sortBy">The sort by.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        PagedList<TEntity> GetPaged<PEntity>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, PEntity>> sortBy, int pageIndex, int pageSize, bool desc = true);
        Task<PagedList<TEntity>> GetPagedAsync<PEntity>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, PEntity>> sortBy, int pageIndex, int pageSize, bool desc = true);
        /// <summary>
        /// Existses the specified expression.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> predicate);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// Counts entities with the specified criteria.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        int Count(Expression<Func<TEntity, bool>> predicate);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        TEntity Add(TEntity entity, bool autoSaveChanges=true);


        void AddRange(IEnumerable<TEntity> list, bool autoSaveChanges = true);

        /// <summary>
        /// Attaches the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        int Update(TEntity entity, bool autoSaveChanges = true);
        /// <summary>
        /// 部分字段更新
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="expressions"></param>
        /// <param name="autoSaveChanges"></param>
        /// <returns></returns>
        int Update(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> expressions,
            bool autoSaveChanges = true);
        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        void Delete(TEntity entity, bool autoSaveChanges = true);



        void RemoveRange(IEnumerable<TEntity> list, bool autoSaveChanges = true);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        void Delete(Expression<Func<TEntity, bool>> predicate, bool autoSaveChanges = true);

        

        /// <summary>
        /// Deletes entities which satify specificatiion
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria.</param>
        void Delete(ISpecification<TEntity> specification, bool autoSaveChanges = true);

        

        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        void Delete(object key);


        Lucky.Hr.Core.IDbContext IContext { get; }
    }
}