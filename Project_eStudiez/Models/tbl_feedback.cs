namespace Project_eStudiez.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_feedback
    {
        [Key]
        public int feedback_id { get; set; }

        [StringLength(150)]
        public string feedback_name { get; set; }

        [StringLength(150)]
        public string feedback_email { get; set; }

        [Column(TypeName = "text")]
        public string feedback_subject { get; set; }

        [Column(TypeName = "text")]
        public string feedback_message { get; set; }

        public DateTime? feedback_datetime { get; set; }

        [StringLength(50)]
        public string feedback_role { get; set; }
    }
}
