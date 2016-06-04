using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Core.Infrastructure
{
    /// <summary>
    /// 启动任务接口
    /// </summary>
    public interface IStartupTask
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        void Execute();

        /// <summary>
        /// 执行顺序
        /// </summary>
        int Order { get; }
    }
}
