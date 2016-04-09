using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Lucky.Core.Data.UnitOfWork;
using Lucky.Hr.Entity;
using Lucky.Hr.Entity.Mapping;
using Lucky.Hr.IService;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Lucky.Hr.Service
{
    public class MainContextIdentity : IdentityDbContext, IMainContext
    {
        public MainContextIdentity(string connectionString)
            : base(connectionString)
        {
            _context = this;
        }
        private System.Data.Entity.DbContext _context;
        public System.Data.Entity.Core.Objects.ObjectContext ObjectContext
        {
            get { return ((IObjectContextAdapter)_context).ObjectContext; }
        }
        public DbSet<TEntity> DbSet<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>();
        }
        public new void SaveChanges()
        {
            base.SaveChanges();
        }
    }
    public class MainContext : DbContext, IMainContext
    {
        public MainContext(string connectionString)
            : base(connectionString)
        {
            _context = this;
        }
        private System.Data.Entity.DbContext _context;
        public System.Data.Entity.Core.Objects.ObjectContext ObjectContext
        {
            get { return ((IObjectContextAdapter)_context).ObjectContext; }
        }
        public DbSet<TEntity> DbSet<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>();
        }
        public new void SaveChanges()
        {
            base.SaveChanges();
        }
    }
    public class HrDbContext : MainContextIdentity, IHrDbContext
    {
        public static HrDbContext Create()
        {
            return new HrDbContext();
        }
        static HrDbContext()
        {
           // Database.SetInitializer<HrDbContext>(null);
            
        }
        public HrDbContext():base("Name=LuckyHrContext")
        {
           // Configuration.ValidateOnSaveEnabled = false;  
        }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Cert> Certs { get; set; }
        public DbSet<CertCategory> CertCategories { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentRole> DepartmentRoles { get; set; }
        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<DistributorConfig> DistributorConfigs { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<JobCategory> JobCategories { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<LanguageCategory> LanguageCategories { get; set; }
       // public DbSet<Manager> Managers { get; set; }
        public DbSet<ManagerFile> ManagerFiles { get; set; }
        public DbSet<ManagerLog> ManagerLogs { get; set; }
        public DbSet<ManagerRecord> ManagerRecords { get; set; }
       // public DbSet<ManagerRole> ManagerRoles { get; set; }
        public DbSet<Nav> Navs { get; set; }
        public DbSet<NavOperation> NavOperations { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsType> NewsTypes { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Other> Other { get; set; }
        public DbSet<Personal> Personals { get; set; }
        public DbSet<Practice> Practices { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<ResumeArea> ResumeAreas { get; set; }
        public DbSet<ResumeIndustry> ResumeIndustries { get; set; }
        public DbSet<ResumeJobCategory> ResumeJobCategories { get; set; }
      //  public DbSet<Role> Roles { get; set; }
        public DbSet<RoleNav> RoleNavs { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<SkillCategory> SkillCategories { get; set; }
        public DbSet<Work> Works { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AchievementMap());
            modelBuilder.Configurations.Add(new AreaMap());
            modelBuilder.Configurations.Add(new CertMap());
            modelBuilder.Configurations.Add(new CertCategoryMap());
            modelBuilder.Configurations.Add(new DepartmentMap());
            modelBuilder.Configurations.Add(new DepartmentRoleMap());
            modelBuilder.Configurations.Add(new DistributorMap());
            modelBuilder.Configurations.Add(new DistributorConfigMap());
            modelBuilder.Configurations.Add(new EducationMap());
            modelBuilder.Configurations.Add(new IndustryMap());
            modelBuilder.Configurations.Add(new JobCategoryMap());
            modelBuilder.Configurations.Add(new LanguageMap());
            modelBuilder.Configurations.Add(new LanguageCategoryMap());
            modelBuilder.Configurations.Add(new ManagerMap());
            modelBuilder.Configurations.Add(new ManagerFileMap());
            modelBuilder.Configurations.Add(new ManagerLogMap());
            modelBuilder.Configurations.Add(new ManagerRecordMap());
            modelBuilder.Configurations.Add(new NavMap());
            modelBuilder.Configurations.Add(new NavOperationMap());
            modelBuilder.Configurations.Add(new NewsMap());
            modelBuilder.Configurations.Add(new NewsTypeMap());
            modelBuilder.Configurations.Add(new OperationMap());
            modelBuilder.Configurations.Add(new OtherMap());
            modelBuilder.Configurations.Add(new PersonalMap());
            modelBuilder.Configurations.Add(new PracticeMap());
            modelBuilder.Configurations.Add(new ProjectMap());
            modelBuilder.Configurations.Add(new ResumeMap());
            modelBuilder.Configurations.Add(new ResumeAreaMap());
            modelBuilder.Configurations.Add(new ResumeIndustryMap());
            modelBuilder.Configurations.Add(new ResumeJobCategoryMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new RoleNavMap());
            modelBuilder.Configurations.Add(new SkillMap());
            modelBuilder.Configurations.Add(new SkillCategoryMap());
            modelBuilder.Configurations.Add(new WorkMap());
            base.OnModelCreating(modelBuilder);
        }
    }
    public class ApplicationUserManager : UserManager<Manager>
    {
        public ApplicationUserManager(IUserStore<Manager> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<Manager>(context.Get<HrDbContext>()));
            // 配置用户名的验证逻辑
            manager.UserValidator = new UserValidator<Manager>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // 配置密码的验证逻辑
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            // 注册双重身份验证提供程序。此应用程序使用手机和电子邮件作为接收用于验证用户的代码的一个步骤
            // 你可以编写自己的提供程序并在此处插入。
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<Manager>
            {
                MessageFormat = "Your security code is: {0}"
            });
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<Manager>
            {
                Subject = "安全代码",
                BodyFormat = "Your security code is: {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<Manager>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
    public class ApplicationRoleManager : RoleManager<Role>
    {
        public ApplicationRoleManager(IRoleStore<Role, string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<Role>(context.Get<HrDbContext>()));
        }
    }
    public class ApplicationSignInManager : SignInManager<Manager, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager) { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(Manager user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // 在此处插入电子邮件服务可发送电子邮件。
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // 在此处插入短信服务可发送短信。
            return Task.FromResult(0);
        }
    }
}
