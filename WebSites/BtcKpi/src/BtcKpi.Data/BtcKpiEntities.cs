using BtcKpi.Data.Configuration;
using BtcKpi.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BtcKpi.Data.Repositories;

namespace BtcKpi.Data
{
    public class BtcKpiEntities : DbContext
    {
        public BtcKpiEntities() : base("BtcKpiEntities") { }

        public DbSet<Gadget> Gadgets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Test> Tests { get; set; }

        public DbSet<AdministratorShip> AdministratorShips { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleDepartment> RoleDepartments { get; set; }
        public DbSet<RolesFunction> RolesFunctions { get; set; }
        public DbSet<SysConfig> SysConfigs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UsersRole> UsersRoles { get; set; }
        public DbSet<CompleteWorkTitle> CompleteWorkTitles { get; set; }
        public DbSet<Ipf> Ipfs { get; set; }
        public DbSet<IpfSchedule> IpfSchedules { get; set; }
        public DbSet<PersonalPlan> PersonalPlans { get; set; }
        public DbSet<IpfDetail> IpfDetails { get; set; }
        public DbSet<IpfComment> IpfComments { get; set; }
        public DbSet<IpfCompetencyData> IpfCompetencyDatas { get; set; }
        public DbSet<zIpf> ZIpfs { get; set; }
        public DbSet<zIpfDetail> ZIpfDetails { get; set; }
        public DbSet<zPersonalPlan> ZPersonalPlans { get; set; }
        public DbSet<Upf> Upfs { get; set; }
        public DbSet<UpfSchedule> UpfSchedule { get; set; }
        public DbSet<UpfNameDetail> UpfNameDetails { get; set; }
        public DbSet<UpfJobDetail> UpfJobDetails { get; set; }
        public DbSet<UpfPersRewProposal> UpfPersRewProposals { get; set; }
        public DbSet<UpfRate> UpfRates { get; set; }
        public DbSet<UpfComment> UpfComments { get; set; }
        public DbSet<UpfCross> UpfCrosses { get; set; }
        public DbSet<UpfCrossDetail> UpfCrossDetails { get; set; }
        public DbSet<zUpfCross> ZUpfCrosses { get; set; }
        public DbSet<zUpfCrossDetail> ZUpfCrossDetails { get; set; }
        public DbSet<UpfHis> UpfHiss { get; set; }
        public DbSet<UpfNameDetailHis> UpfNameDetailHiss { get; set; }
        public DbSet<UpfJobDetailHis> UpfJobDetailHiss { get; set; }
        public DbSet<UpfPersRewProposalHis> UpfPersRewProposalHiss { get; set; }
        public DbSet<IpfReport> IpfReports { get; set; }
        public DbSet<UpfSummary> UpfSummaries { get; set; }
        public DbSet<UpfCrossSummary> UpfCrossSummaries { get; set; }
        public DbSet<PerformanceLSFB> PerformanceLsfbs { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<TypePerformance> TypePerformances { get; set; }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new GadgetConfiguration());
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new TestConfiguration());
            modelBuilder.Configurations.Add(new AdministratorShipConfiguration());
            modelBuilder.Configurations.Add(new CompanyConfiguration());
            modelBuilder.Configurations.Add(new DepartmentConfiguration());
            modelBuilder.Configurations.Add(new FunctionConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new RoleDepartmentConfiguration());
            modelBuilder.Configurations.Add(new RolesFunctionConfiguration());
            modelBuilder.Configurations.Add(new SysConfigConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UsersRoleConfiguration());
            modelBuilder.Configurations.Add(new CompleteWorkTitleConfiguration());
            modelBuilder.Configurations.Add(new IpfConfiguration());
            modelBuilder.Configurations.Add(new IpfScheduleConfiguration());
            modelBuilder.Configurations.Add(new PersonalPlanConfiguration());
            modelBuilder.Configurations.Add(new IpfDetailConfiguration());
            modelBuilder.Configurations.Add(new IpfCommentConfiguration());
            modelBuilder.Configurations.Add(new IpfCompetencyDataConfiguration());
            modelBuilder.Configurations.Add(new zIpfConfiguration());
            modelBuilder.Configurations.Add(new zIpfDetailConfiguration());
            modelBuilder.Configurations.Add(new zPersonalPlanConfiguration());
            modelBuilder.Configurations.Add(new UpfConfiguration());
            modelBuilder.Configurations.Add(new UpfScheduleConfiguration());
            modelBuilder.Configurations.Add(new UpfNameDetailConfiguration());
            modelBuilder.Configurations.Add(new UpfJobDetailConfiguration());
            modelBuilder.Configurations.Add(new UpfPersRewProposalConfiguration());
            modelBuilder.Configurations.Add(new UpfRateConfiguration());
            modelBuilder.Configurations.Add(new UpfCommentConfiguration());
            modelBuilder.Configurations.Add(new UpfCrossConfiguration());
            modelBuilder.Configurations.Add(new UpfCrossDetailConfiguration());
            modelBuilder.Configurations.Add(new zUpfCrossConfiguration());
            modelBuilder.Configurations.Add(new zUpfCrossDetailConfiguration());
            modelBuilder.Configurations.Add(new UpfHisConfiguration());
            modelBuilder.Configurations.Add(new UpfNameDetailHisConfiguration());
            modelBuilder.Configurations.Add(new UpfJobDetailHisConfiguration());
            modelBuilder.Configurations.Add(new UpfPersRewProposalHisConfiguration());
            modelBuilder.Configurations.Add(new UpfSummaryConfiguration());
            modelBuilder.Configurations.Add(new UpfCrossSummaryConfiguration());
            modelBuilder.Configurations.Add(new IpfReportConfiguration());
            modelBuilder.Configurations.Add(new PerformanceConfiguration());
            modelBuilder.Configurations.Add(new ProjectsConfiguration());
            modelBuilder.Configurations.Add(new TypePerformanceConfiguration());
       }
    }
}
