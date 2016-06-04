using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucky.Core.Data.UnitOfWork;
using Lucky.Entity;

namespace Lucky.IService
{
    public interface IHrDbContext : IMainContext
    {
        DbSet<Achievement> Achievements { get; set; }
        DbSet<Area> Areas { get; set; }
        DbSet<Cert> Certs { get; set; }
        DbSet<CertCategory> CertCategories { get; set; }
        DbSet<Department> Departments { get; set; }
        DbSet<DepartmentRole> DepartmentRoles { get; set; }
        DbSet<Distributor> Distributors { get; set; }
        DbSet<DistributorConfig> DistributorConfigs { get; set; }
        DbSet<Education> Educations { get; set; }
        DbSet<Industry> Industries { get; set; }
        DbSet<JobCategory> JobCategories { get; set; }
        DbSet<Language> Languages { get; set; }
        DbSet<LanguageCategory> LanguageCategories { get; set; }
       // DbSet<Manager> Managers { get; set; }
        DbSet<ManagerFile> ManagerFiles { get; set; }
        DbSet<ManagerLog> ManagerLogs { get; set; }
        DbSet<ManagerRecord> ManagerRecords { get; set; }
      //  DbSet<ManagerRole> ManagerRoles { get; set; }
        DbSet<Nav> Navs { get; set; }
        DbSet<NavOperation> NavOperations { get; set; }
        DbSet<News> News { get; set; }
        DbSet<NewsType> NewsTypes { get; set; }
        DbSet<Operation> Operations { get; set; }
        DbSet<Other> Other { get; set; }
        DbSet<Personal> Personals { get; set; }
        DbSet<Practice> Practices { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<Resume> Resumes { get; set; }
        DbSet<ResumeArea> ResumeAreas { get; set; }
        DbSet<ResumeIndustry> ResumeIndustries { get; set; }
        DbSet<ResumeJobCategory> ResumeJobCategories { get; set; }
       // DbSet<Role> Roles { get; set; }
        DbSet<RoleNav> RoleNavs { get; set; }
        DbSet<Skill> Skills { get; set; }
        DbSet<SkillCategory> SkillCategories { get; set; }
        DbSet<Work> Works { get; set; }

    }
}
