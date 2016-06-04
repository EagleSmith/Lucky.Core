// =================================================================== 
// 项目说明
//====================================================================
// 幸运草工作室 @ CopyRight 2014-2020。
// 文件： ManagerFileRespository.cs
// 项目名称： 
// 创建时间：2014/10/23
// 负责人：丁富升
// ===================================================================
using Lucky.Core.Data;
using Lucky.Entity;
using Lucky.Hr.IService;
namespace Lucky.Hr.Service
{
    public  class ManagerFileService  :EntityRepository< ManagerFile>,IManagerFileService
    {
      public ManagerFileService(IHrDbContext context):base(context)
        {
            
        }
				
    }
}