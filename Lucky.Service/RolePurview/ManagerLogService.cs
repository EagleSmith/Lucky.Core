﻿// =================================================================== 
// 项目说明
//====================================================================
// 幸运草工作室 @ CopyRight 2014-2020。
// 文件： ManagerLogRespository.cs
// 项目名称： 
// 创建时间：2014/10/23
// 负责人：丁富升
// ===================================================================
using Lucky.Core.Data;
using Lucky.Entity;
using Lucky.IService;
namespace Lucky.Service
{
    public  class ManagerLogService  :EntityRepository< ManagerLog>,IManagerLogService
    {
      public ManagerLogService(IHrDbContext context):base(context)
        {
            
        }
				
    }
}