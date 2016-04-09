using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Lucky.Core.Data.UnitOfWork;
using Lucky.Core.Utility.Extensions;
namespace Lucky.Core.Data
{
    public class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : class ,new()
    {
        private IMainContext _context;
        public EntityRepository(IMainContext context)
        {
            _context = context;
        }

        public DbSet<TEntity> Entities
        {
            get { return _context.DbSet<TEntity>(); }
        }
        public Database Database => (_context as DbContext)?.Database;

        #region IRepository<TEntity> 成员

        public IQueryable<TEntity> GetQuery(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, bool IsNoTracking = true)
        {
            if (IsNoTracking)
                return Entities.AsNoTracking().Where(predicate);
            return Entities.Where(predicate);
        }

        public IQueryable<TEntity> GetQueryOrderBy<PEntity>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, PEntity>> sortBy, bool IsNoTracking = true)
        {
            if (IsNoTracking)
                return Entities.AsNoTracking().Where(predicate).OrderByDescending(sortBy);
            return Entities.Where(predicate).OrderByDescending(sortBy);
        }
        public TEntity Single(object key)
        {
            long id;
            if (long.TryParse(key.ToString(), out id))
                return Entities.Find(id);
            else
                return Entities.Find(new Guid(key.ToString()));
        }

        public async Task<TEntity> SingleAsync(object key)
        {
            long id;
            if (long.TryParse(key.ToString(), out id))
                return await Entities.FindAsync(id);
            else
                return await Entities.FindAsync(new Guid(key.ToString()));
        }
        public TEntity Single(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, bool IsNoTracking = true)
        {
            return GetQuery(IsNoTracking).FirstOrDefault(predicate);
        }

        public async Task<TEntity> SingleAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, bool IsNoTracking = true)
        {
            return await GetQuery(IsNoTracking).FirstOrDefaultAsync(predicate);
        }
        public TEntity Single(Specification.ISpecification<TEntity> specification, bool IsNoTracking = true)
        {
            return Single(specification.Predicate, IsNoTracking);
        }

        public IList<TEntity> GetList(bool IsNoTracking = true)
        {
            return GetQuery(IsNoTracking).ToList();
        }

        public IQueryable<TEntity> GetQuery(bool IsNoTracking = true)
        {
            return GetQuery(a => true, IsNoTracking);
        }
        public IQueryable<TEntity> GetQuery(Specification.ISpecification<TEntity> specification, bool IsNoTracking = true)
        {
            return GetQuery(specification.Predicate, IsNoTracking);
        }

        public IList<TEntity> Find(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, bool IsNoTracking = true)
        {
            return GetQuery(predicate, IsNoTracking).ToList();
        }

        public IList<TEntity> Find(Specification.ISpecification<TEntity> specification, bool IsNoTracking = true)
        {
            return GetQuery(specification, IsNoTracking).ToList();
        }

        public PagedList<TEntity> GetPaged<PEntity>(System.Linq.Expressions.Expression<Func<TEntity, PEntity>> sortBy, int pageIndex, int pageSize)
        {
            return GetQuery(true).OrderByDescending(sortBy).ToPagedList(pageIndex, pageSize);
        }

        public PagedList<TEntity> GetPaged<PEntity>(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, System.Linq.Expressions.Expression<Func<TEntity, PEntity>> sortBy, int pageIndex, int pageSize, bool desc = true)
        {
            if (desc)
                return GetQuery(predicate, true).OrderByDescending(sortBy).ToPagedList(pageIndex, pageSize);
            return GetQuery(predicate, true).OrderBy(sortBy).ToPagedList(pageIndex, pageSize);
        }

        public bool Exists(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return GetQuery(true).Any(predicate);
        }

        public int Count(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return GetQuery(true).Count(predicate);
        }

        public TEntity Add(TEntity entity, bool autoSaveChanges = true)
        {
            TEntity _entity=Entities.Add(entity);
            if(autoSaveChanges)
                _context.SaveChanges();
            return _entity;
        }

        public void AddRange(IEnumerable<TEntity> list, bool autoSaveChanges = true)
        {
            Entities.AddRange(list);
            if (autoSaveChanges)
            {
                _context.SaveChanges();
            }
        }

        public int Update(TEntity entity, bool autoSaveChanges = true)
        {
            Entities.Attach(entity);
            var dbContext = _context as System.Data.Entity.DbContext;
            if (dbContext != null) dbContext.Entry(entity).State = EntityState.Modified;
            if (autoSaveChanges)
            {
                _context.SaveChanges();
            }
            return 1;
        }

        public int Update(TEntity entity, IEnumerable<Expression<Func<TEntity, object>>> expressions, bool autoSaveChanges = true)
        {
            Entities.Attach(entity);
            var dbContext = _context as System.Data.Entity.DbContext;
            if (dbContext != null) dbContext.Entry(entity).SetModified(expressions);
            if (autoSaveChanges)
            {
                _context.SaveChanges();
            }
            
            return 1;
        }
        public void Delete(TEntity entity, bool autoSaveChanges = true)
        {
            if (entity != null)
            {
                Entities.Attach(entity);
                var dbContext = _context as System.Data.Entity.DbContext;
                if (dbContext != null) dbContext.Entry(entity).State = EntityState.Deleted;
                if (autoSaveChanges)
                {
                    _context.SaveChanges();
                }
            }
        }
        public void RemoveRange(IEnumerable<TEntity> list, bool autoSaveChanges = true)
        {
            Entities.RemoveRange(list);
            if (autoSaveChanges)
            {
                _context.SaveChanges();
            }
        }
        public void Delete(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, bool autoSaveChanges = true)
        {
            IList<TEntity> list = Find(predicate, false).ToList();
            RemoveRange(list, autoSaveChanges);
            
        }

        public void Delete(Specification.ISpecification<TEntity> specification, bool autoSaveChanges = true)
        {
            Delete(specification.Predicate,false);
            if (autoSaveChanges)
            {
                _context.SaveChanges();
            }
        }

        public void Delete(object key)
        {
            // IContext.Delete("").Where()
            var item = new TEntity();
            var itemType = item.GetType();
            var entityContainer = _context.ObjectContext.MetadataWorkspace.GetEntityContainer(_context.ObjectContext.DefaultContainerName, DataSpace.CSpace);
            var entitySetName = entityContainer.BaseEntitySets.First(b => b.ElementType.Name == itemType.Name).Name;
            var primaryKey = _context.ObjectContext.CreateEntityKey(entitySetName, item).EntityKeyValues[0];
            itemType.GetProperty(primaryKey.Key).SetValue(item, key, null);
            _context.ObjectContext.CreateObjectSet<TEntity>().Attach(item);
            _context.ObjectContext.ObjectStateManager.ChangeObjectState(item, System.Data.Entity.EntityState.Deleted);
        }

       

        #endregion

        

        #region 异步操作
        public async Task<TEntity> SingleAsync(Specification.ISpecification<TEntity> specification, bool IsNoTracking = true)
        {
            return await SingleAsync(specification.Predicate);
        }

        public async Task<IList<TEntity>> GetListAsync(bool IsTracking = false)
        {
            return await GetQuery(IsTracking).ToListAsync();
        }

        public async Task<IQueryable<TEntity>> GetQueryAsync(bool IsNoTracking = true)
        {
            return await GetQueryAsync(a => true, IsNoTracking);
        }

        public async Task<IQueryable<TEntity>> GetQueryAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, bool IsNoTracking = true)
        {
            if (IsNoTracking)
                return await Task.Run(()=>Entities.AsNoTracking().Where(predicate));
            return await Task.Run(()=>Entities.Where(predicate));
        }

        public async Task<IQueryable<TEntity>> GetQueryAsync(Specification.ISpecification<TEntity> specification, bool IsNoTracking = true)
        {
            return await GetQueryAsync(specification.Predicate);
        }

        public async Task<IList<TEntity>> FindAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, bool IsNoTracking = true)
        {
            return await GetQuery(predicate).ToListAsync();
        }

        public async Task<IList<TEntity>> FindAsync(Specification.ISpecification<TEntity> specification, bool IsNoTracking = true)
        {
            return await GetQuery(specification.Predicate).ToListAsync();
        }

        public async Task<PagedList<TEntity>> GetPagedAsync<PEntity>(System.Linq.Expressions.Expression<Func<TEntity, PEntity>> sortBy, int pageIndex, int pageSize)
        {
            return await Task.Run(()=>GetQuery(true).OrderByDescending(sortBy).ToPagedList(pageIndex, pageSize));
        }

        public async Task<PagedList<TEntity>> GetPagedAsync<PEntity>(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, System.Linq.Expressions.Expression<Func<TEntity, PEntity>> sortBy, int pageIndex, int pageSize, bool desc = true)
        {
            if (desc)
                return await Task.Run(()=>GetQuery(predicate, true).OrderByDescending(sortBy).ToPagedList(pageIndex, pageSize));
            return await Task.Run(()=>GetQuery(predicate, true).OrderBy(sortBy).ToPagedList(pageIndex, pageSize));
        }

        public async Task<bool> ExistsAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return await GetQuery(true).AnyAsync(predicate);
        }

        public async Task<int> CountAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return await GetQuery(true).CountAsync(predicate);
        }

        
        #endregion
    }
}
