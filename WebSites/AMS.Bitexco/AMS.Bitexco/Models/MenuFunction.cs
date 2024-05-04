namespace AMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MenuFunction")]
    public partial class MenuFunction
    {
        public MenuFunction()
        {
            ChildFunctions = new HashSet<MenuFunction>();
            Authorizes = new HashSet<Authorize>();
        }

        public int Id { get; set; }

        public string AppName { get; set; }

        public int? ParentId { get; set; }

        [Required]
        [StringLength(200)]
        public string MenuName { get; set; }

        [StringLength(2000)]
        public string MenuIcon { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [StringLength(2000)]
        public string URL { get; set; }

        public int Sort { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<MenuFunction> ChildFunctions { get; set; }

        public virtual MenuFunction ParentMenu { get; set; }

        public virtual ICollection<Authorize> Authorizes { get; set; }
    }

    public class TopMenu
    {
        public string GroupMenuName { get; set; }
        public string GroupMenuIcon { get; set; }
        public string MenuApp { get; set; }
        public string MenuName { get; set; }
        public string MenuIcon { get; set; }
        public string Url { get; set; }
    }
}
