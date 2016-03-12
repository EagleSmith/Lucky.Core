using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Hr.Core.Events.EventsBus
{
    /// <summary>
    /// 事件监听者接口
    /// </summary>
    public interface IConsumer<T>
    {
        /// <summary>
        /// 处理事件
        /// </summary>
        void HandleEvent(T eventMessage);
    }
}
