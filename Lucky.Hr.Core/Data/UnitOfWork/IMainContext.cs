using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Hr.Core.Data.UnitOfWork
{
    public interface IMainContext : IUnitOfWork
    {
        /// <summary>
        /// Create a object set for a type TEntity
        /// </summary>
        /// <typeparam name="TEntity">Type of elements in object set</typeparam>
        /// <returns>Object set of type {TEntity}</returns>
        DbSet<TEntity> DbSet<TEntity>() where TEntity : class;

        ObjectContext ObjectContext { get; }

    }
}
