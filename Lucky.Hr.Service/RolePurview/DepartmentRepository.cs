// =================================================================== 
// 项目说明
//====================================================================
// 幸运草工作室 @ CopyRight 2014-2020。
// 文件： DepartmentRespository.cs
// 项目名称： 
// 创建时间：2014/10/23
// 负责人：丁富升
// ===================================================================

using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Lucky.Hr.Core;
using Lucky.Hr.Core.Specification;
using Lucky.Hr.Core.Data;
using Lucky.Hr.Entity;
using Lucky.Hr.IService;
using Lucky.Hr.ViewModels.Models.SiteManager;

namespace Lucky.Hr.Service
{
    public  class DepartmentRepository  :EntityRepository< Department>,IDepartmentRepository
    {
        private IHrDbContext _db;
      public DepartmentRepository(IHrDbContext context):base(context)
      {
          _db = context;
      }


      public PagedList<DepartmentViewModel> GetList(int pageIndex = 1, string keyword = "")
      {
          var spec = SpecificationBuilder.Create<Department>();
          if (keyword != "")
              spec.Like(a => a.DepartmentName, keyword);
          var query = (from e in _db.Departments
                       where keyword == "" || e.DepartmentName.Contains(keyword)
                       select new
                       {
                           e.DepartmentId,
                           e.DistributorId,
                           e.ParentId,
                           e.DepartmentName,
                           e.Description,
                           e.State,
                           e.Sort,
                           ParentName = (from a in _db.Departments where a.DepartmentId == e.ParentId select a.DepartmentName).FirstOrDefault(),
                           DistributorName = e.Distributor.DistributionName
                       })
              .OrderByDescending(a => a.DepartmentId).ToPagedList(pageIndex, 20);
          var vm = query.Select(Mapper.DynamicMap<DepartmentViewModel>).ToPagedList(pageIndex, 20);

          return vm;
      }
    }
}