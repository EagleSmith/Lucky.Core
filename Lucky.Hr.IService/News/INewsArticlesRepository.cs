// =================================================================== 
// 项目说明
//====================================================================
// 幸运草工作室 @ CopyRight 2014-2020。
// 文件： INewsArticlesRepository.cs
// 项目名称： 
// 创建时间：2015/3/3
// 负责人：丁富升
// ===================================================================
using System;
using System.Collections.Generic;
using System.Text;
using Lucky.Hr.Entity;
using Lucky.Hr.Core.Data;
using Lucky.Hr.ViewModels.Models.News;

namespace Lucky.Hr.IService
{
    public interface INewsArticlesRepository : IRepository<NewsArticle>
    {
        void DeleteMore(string ids);
        List<NewsArticlesViewModel> GetArticlesViewModels();
    }
}