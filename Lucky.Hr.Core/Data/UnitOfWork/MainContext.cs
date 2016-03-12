using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lucky.Hr.Core.Data.UnitOfWork
{
    public class MainContext : System.Data.Entity.DbContext, IMainContext
    {
        public MainContext(string connectionString)
            : base(connectionString)
        {
            _context = this;
            //this.Configuration.LazyLoadingEnabled = false;
            //Database.SetInitializer<MainContext>(new DropCreateDatabaseIfModelChanges<MainContext>());
        }
        #region IMainContext 成员

        private System.Data.Entity.DbContext _context;
        public System.Data.Entity.Core.Objects.ObjectContext ObjectContext
        {
            get { return ((IObjectContextAdapter)_context).ObjectContext; }
        }

        #endregion

        #region IUnitOfWork 成员
        public DbSet<TEntity> DbSet<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>();
        }
        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        public new void SaveChangesAsync()
        {
            base.SaveChangesAsync();
        }
        #endregion
    }

    
}
