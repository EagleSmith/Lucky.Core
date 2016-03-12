// =================================================================== 
// 项目说明
//====================================================================
// 幸运草工作室 @ CopyRight 2014-2020。
// 文件： NewsArticleTextRespository.cs
// 项目名称： 
// 创建时间：2015/3/3
// 负责人：丁富升
// ===================================================================
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Lucky.Hr.Core;

using Lucky.Hr.Core.Data;

using Lucky.Hr.Entity;

using Lucky.Hr.IService;

namespace Lucky.Hr.Service
{
    public  class NewsArticleTextRepository  :EntityRepository< NewsArticleText>,INewsArticleTextRepository
    {
      public NewsArticleTextRepository(INewsContext context):base(context)
        {
            
        }
				
    }
}