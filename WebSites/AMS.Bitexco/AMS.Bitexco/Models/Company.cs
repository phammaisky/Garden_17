namespace AMS.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Company")]
    public partial class Company
    {
        public Company()
        {
            CompanyChild = new HashSet<Company>();
            Departments = new HashSet<Department>();
            DeviceAndTools = new HashSet<DeviceAndTool>();
            UserInfoes = new HashSet<UserInfo>();
        }

        public int Id { get; set; }

        public int? ParentId { get; set; }

        [Required]
        [StringLength(200)]
        public string NameVn { get; set; }

        [Required]
        [StringLength(200)]
        public string NameEn { get; set; }

        [StringLength(2000)]
        public string Contents { get; set; }

        [StringLength(2000)]
        public string LogoLocation { get; set; }

        public int? Sort { get; set; }

        public virtual ICollection<Company> CompanyChild { get; set; }

        public virtual Company CompanyParent { get; set; }

        public virtual ICollection<Department> Departments { get; set; }

        public virtual ICollection<UserInfo> UserInfoes { get; set; }
        public virtual ICollection<DeviceAndTool> DeviceAndTools { get; set; }
    }
}
