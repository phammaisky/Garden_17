namespace AMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    using AMS.Resource;

    [Table("GroupUser")]
    public partial class GroupUser
    {
        public GroupUser()
        {
            GroupUser_Authorize = new HashSet<GroupUser_Authorize>();
            UserInfoes = new HashSet<UserInfo>();
        }

        public int Id { get; set; }

        [Display(Name = "Tên nhóm người dùng")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Multi))]
        [StringLength(200, ErrorMessageResourceName = "StringLeng", ErrorMessageResourceType = typeof(Multi))]
        public string GroupName { get; set; }

        [Display(Name = "Mô tả")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Multi))]
        [StringLength(2000, ErrorMessageResourceName = "StringLeng", ErrorMessageResourceType = typeof(Multi))]
        public string Description { get; set; }

        public bool Active { get; set; }

        public string AppName { get; set; }

        public virtual ICollection<GroupUser_Authorize> GroupUser_Authorize { get; set; }

        public virtual ICollection<UserInfo> UserInfoes { get; set; }
    }
}
