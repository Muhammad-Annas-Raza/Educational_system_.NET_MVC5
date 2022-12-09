namespace Project_eStudiez.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_link
    {
        [Key]
        public int link_id { get; set; }

        [Column(TypeName = "text")]
        public string link_material { get; set; }

        public DateTime? link_datetime { get; set; }

        [StringLength(255)]
        public string link_title { get; set; }

        [StringLength(255)]
        public string link_class { get; set; }
    }
}
