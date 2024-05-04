namespace AMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Authorize")]
    public partial class Authorize
    {
        public Authorize()
        {
            GroupUser_Authorize = new HashSet<GroupUser_Authorize>();
        }

        public int Id { get; set; }

        public int FunctionId { get; set; }

        [Required]
        [StringLength(200)]
        public string Operator { get; set; }

        [StringLength(2000)]
        public string UrlControlAction { get; set; }

        public bool HasExtend { get; set; }

        public virtual MenuFunction MenuFunction { get; set; }

        public virtual ICollection<GroupUser_Authorize> GroupUser_Authorize { get; set; }
    }
}
