// =================================================================== 
// 项目说明
//====================================================================
// 幸运草工作室 @ CopyRight 2014-2020
// 文件： IPagedList.cs
// 项目名称： 
// 创建时间：2014/6/4
// 创建人：丁富升
// ===================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Hr.Core
{
    /// <summary>
    /// 数据分页接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPagedList<out T>:IEnumerable<T>,IPagedList
    {
        
    }

    public interface IPagedList:IEnumerable
    {
        /// <summary>
        /// 分页索引
        /// </summary>
        int PageIndex { get; }
        /// <summary>
        /// 每页数据量
        /// </summary>
        int PageSize { get; }
        /// <summary>
        /// 数据总量
        /// </summary>
        int TotalCount { get; }
        /// <summary>
        /// 分页总数
        /// </summary>
        int TotalPages { get; }
        /// <summary>
        /// 上一页
        /// </summary>
        bool HasPreviousPage { get; }
        /// <summary>
        /// 下一页
        /// </summary>
        bool HasNextPage { get; }
    }
}
