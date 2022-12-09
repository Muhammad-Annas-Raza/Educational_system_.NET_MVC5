namespace Project_eStudiez.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_mark
    {
        [Key]
        public int mark_id { get; set; }

        public string mark_subject { get; set; }

        public int? mark_totalmark { get; set; }

        public int? mark_obtainedmark { get; set; }

        public int fk_user_id { get; set; }

        public DateTime? mark_date_time { get; set; }

        public string mark_std_standard { get; set; }

        public virtual tbl_user tbl_user { get; set; }
    }
}
