namespace AMS.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AMSEntities : DbContext
    {
        public AMSEntities()
            : base("name=AMSEntities")
        {
        }
        public virtual DbSet<Application> Application { get; set; }
        public virtual DbSet<Authorize> Authorizes { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<DeviceAndTool> DeviceAndTools { get; set; }
        public virtual DbSet<DeviceCategory> DeviceCategories { get; set; }
        public virtual DbSet<GenerateCode> GenerateCodes { get; set; }
        public virtual DbSet<GroupUser> GroupUsers { get; set; }
        public virtual DbSet<GroupUser_Authorize> GroupUser_Authorize { get; set; }
        public virtual DbSet<HistoryUse> HistoryUses { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<MenuFunction> MenuFunctions { get; set; }       
        public virtual DbSet<StatusCategory> StatusCategories { get; set; }
        public virtual DbSet<ToolCategory> ToolCategories { get; set; }
        public virtual DbSet<UserInfo> UserInfoes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Authorize>()
                .HasMany(e => e.GroupUser_Authorize)
                .WithRequired(e => e.Authorize)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
               .HasMany(e => e.CompanyChild)
               .WithOptional(e => e.CompanyParent)
               .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.HistoryUses)
                .WithOptional(e => e.Department)
                .HasForeignKey(e => e.DeptId);          

            modelBuilder.Entity<Department>()
                .HasMany(e => e.UserInfoes)
                .WithOptional(e => e.Department)
                .HasForeignKey(e => e.DeptId);

            modelBuilder.Entity<DeviceAndTool>()
                .Property(e => e.DeviceName)
                .IsFixedLength();

            modelBuilder.Entity<DeviceAndTool>()
                .HasMany(e => e.HistoryUses)
                .WithRequired(e => e.DeviceAndTool)
                .HasForeignKey(e => e.DeviceToolId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DeviceCategory>()
                .HasMany(e => e.DeviceAndTools)
                .WithOptional(e => e.DeviceCategory)
                .HasForeignKey(e => e.DeviceCatId);

            modelBuilder.Entity<GroupUser>()
                .HasMany(e => e.GroupUser_Authorize)
                .WithRequired(e => e.GroupUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GroupUser>()
                .HasMany(e => e.UserInfoes)
                .WithMany(e => e.GroupUsers)
                .Map(m => m.ToTable("GroupUser_User").MapLeftKey("GroupUserId").MapRightKey("UserId"));

            //modelBuilder.Entity<Location>()
            //    .HasMany(e => e.HistoryUses)
            //    .WithOptional(e => e.Location)
            //    .HasForeignKey(e => e.LocationId);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.LocationChild)
                .WithOptional(e => e.ParentLocation)
                .HasForeignKey(e => e.ParentId);


            modelBuilder.Entity<MenuFunction>()
                .HasMany(e => e.Authorizes)
                .WithRequired(e => e.MenuFunction)
                .HasForeignKey(e => e.FunctionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MenuFunction>()
                .HasMany(e => e.ChildFunctions)
                .WithOptional(e => e.ParentMenu)
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<UserInfo>()
                .HasMany(e => e.HistoryUses)
                .WithOptional(e => e.Staff)
                .HasForeignKey(e => e.HandedToStaffId);

            modelBuilder.Entity<StatusCategory>()
                .HasMany(e => e.HistoryUses)
                .WithRequired(e => e.StatusCategory)
                .HasForeignKey(e => e.StatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ToolCategory>()
                .HasMany(e => e.DeviceAndTools)
                .WithOptional(e => e.ToolCategory)
                .HasForeignKey(e => e.ToolCatId);

            modelBuilder.Entity<Company>()
              .HasMany(e => e.UserInfoes)
              .WithMany(e => e.Companies)
              .Map(m => m.ToTable("AMSPermitUserCompany").MapLeftKey("CompanyId").MapRightKey("UserId"));
        }

        public System.Data.Entity.DbSet<AMS.Controllers.UsersDomain> UsersDomains { get; set; }

        public System.Data.Entity.DbSet<AMS.Models.DeviceToolAndHistory> DeviceToolAndHistories { get; set; }

        public System.Data.Entity.DbSet<AMS.Models.Company> Companies { get; set; }
    }
}
