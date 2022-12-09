namespace Project_eStudiez.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_resources
    {
        [Key]
        public int resource_id { get; set; }

        [Column(TypeName = "text")]
        public string resource_material { get; set; }

        [Column(TypeName = "text")]
        public string resource_datetime { get; set; }
    }
}
