// =================================================================== 
// 项目说明
//====================================================================
// 幸运草工作室 @ CopyRight 2014-2020。
// 文件： RoleRespository.cs
// 项目名称： 
// 创建时间：2014/10/23
// 负责人：丁富升
// ===================================================================

using System.Collections.Generic;
using System.Linq;
using Lucky.Core.Data;
using Lucky.Entity;
using Lucky.IService;
using Lucky.ViewModels.Models.SiteManager;

namespace Lucky.Service
{
    public  class RoleService  :EntityRepository< Role>,IRoleService
    {
        private IHrDbContext _context;
      public RoleService(IHrDbContext context):base(context)
      {
          _context = context;
      }



      public IList<NavOperationViewModel> GetNavOperationViewModels(string roleid)
      {
          var list = (from nav in _context.Navs
              where nav.State == 1
              select new NavOperationViewModel
              {
                  NavId = nav.NavId,
                  NavName = nav.NavName,
                  ParentId = nav.ParentId,
                  Operations = (from oper in _context.Operations
                      join navoper in _context.NavOperations on oper.OperationId equals navoper.OperationId
                      where navoper.NavId == nav.NavId
                      select new NavOperationRoleViewModel
                      {
                          OperationId = oper.OperationId,
                          OperationName = oper.OperationName,
                          Checked = (from role in _context.RoleNavs select role).Any(role => role.NavId == nav.NavId && role.OperationId == oper.OperationId && role.RoleId == roleid)
                      })
              }).ToList();
          return list;
      }
    }
}