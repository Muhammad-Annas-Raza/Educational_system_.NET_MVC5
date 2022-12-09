namespace Project_eStudiez.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_extraclass
    {
        [Key]
        public int extraclass_id { get; set; }

        [Column(TypeName = "text")]
        public string extraclass_subject { get; set; }

        [StringLength(150)]
        public string extraclass_dateTime { get; set; }

        public DateTime? extraclass_datetime_created_at { get; set; }

        [StringLength(255)]
        public string extraclass_class { get; set; }
    }
}
