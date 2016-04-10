// =================================================================== 
// 项目说明
//====================================================================
// 幸运草工作室 @ CopyRight 2014-2020。
// 文件： CategoryRespository.cs
// 项目名称： 
// 创建时间：2015/3/3
// 负责人：丁富升
// ===================================================================
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Lucky.Core;
using Lucky.Core.Data;
using Lucky.Entity;
using Lucky.Hr.IService;

namespace Lucky.Hr.Service
{
    public  class CategoryService  :EntityRepository<Category>,ICategoryService
    {
      public CategoryService(INewsContext context):base(context)
        {
            
        }
				
    }
}