namespace AMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Application")]
    public partial class Application
    {
        public Application()
        {            
        }
        [Key] 
        public string AppName { get; set; }
        public string FullNameVn { get; set; }
        public string FullNameEn { get; set; }
        public string SeverName { get; set; }
        public string LinkApp { get; set; }
    }
}