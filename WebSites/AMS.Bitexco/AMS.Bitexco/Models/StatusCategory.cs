namespace AMS.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("StatusCategory")]
    public partial class StatusCategory
    {
        public StatusCategory()
        {
            HistoryUses = new HashSet<HistoryUse>();            
        }

        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public virtual ICollection<HistoryUse> HistoryUses { get; set; }

    }
}
