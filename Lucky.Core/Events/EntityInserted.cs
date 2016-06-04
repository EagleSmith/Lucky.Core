using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Core.Events
{
    public class EntityInserted<T> where T:IEntity
    {
        /// <summary>
        /// 用于消息通知添加数据操作
        /// </summary>
        /// <param name="entity"></param>
        public EntityInserted(T entity)
        {
            this.Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
