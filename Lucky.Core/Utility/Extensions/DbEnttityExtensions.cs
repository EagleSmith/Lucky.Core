using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Core.Utility.Extensions
{
    public static class DbEnttityExtensions
    {
        public static void SetModified<TEntity>(
        this DbEntityEntry<TEntity> entry,
        IEnumerable<Expression<Func<TEntity, object>>> expressions) where TEntity : class, new ()
        {
            foreach (var expression in expressions)
                entry.Property(expression).IsModified = true;
        }
        public static void SetIgnore<TEntity>(
       this DbEntityEntry<TEntity> entry,
       IEnumerable<Expression<Func<TEntity, object>>> expressions) where TEntity : class, new()
        {
            foreach (var expression in expressions)
                entry.Property(expression).IsModified = false;
        }
        
    }
}
