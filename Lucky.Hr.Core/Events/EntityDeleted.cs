using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Core.Events
{
    /// <summary>
    /// 用于消息通知删除操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityDeleted<T>where T:IEntity
    {
        public EntityDeleted(T entity)
        {
            this.Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
