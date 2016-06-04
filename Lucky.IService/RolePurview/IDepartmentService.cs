// =================================================================== 
// 项目说明
//====================================================================
// 幸运草工作室 @ CopyRight 2014-2020。
// 文件： IDepartmentService.cs
// 项目名称： 
// 创建时间：2014/10/23
// 负责人：丁富升
// ===================================================================

using Lucky.Core;
using Lucky.Core.Data;
using Lucky.Entity;
using Lucky.ViewModels.Models.SiteManager;

namespace Lucky.IService
{
    public interface IDepartmentService : IRepository< Department>
    {
        PagedList<DepartmentViewModel> GetList(int pageIndex = 1, string keyword = "");
    }
}