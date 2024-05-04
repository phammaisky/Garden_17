namespace AMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GenerateCode")]
    public partial class GenerateCode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string tblName { get; set; }

        public int IncreNumber { get; set; }

        public int CodeNext { get; set; }

        [Required]
        [StringLength(5)]
        public string YearCode { get; set; }
    }
}
