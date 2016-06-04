// =================================================================== 
// 项目说明
//====================================================================
// 幸运草工作室 @ CopyRight 2014-2020。
// 文件： LinksRespository.cs
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

using Lucky.IService;

namespace Lucky.Service
{
    public  class LinksService  :EntityRepository<Link>,ILinksService
    {
      public LinksService(INewsContext context):base(context)
        {
            
        }
				
    }
}