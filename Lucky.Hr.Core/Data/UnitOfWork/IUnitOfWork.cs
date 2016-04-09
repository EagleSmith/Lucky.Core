using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Core.Data.UnitOfWork
{
    /// <summary>
    /// 数据工作单元接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}
