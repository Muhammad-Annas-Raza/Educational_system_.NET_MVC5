namespace Project_eStudiez.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    public partial class tbl_pdf
    {
        [Key]
        public int pdf_id { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string pdf_material { get; set; }

        public DateTime? pdf_datetime { get; set; }

        [StringLength(255)]
        public string pdf_class { get; set; }

        [StringLength(255)]
        public string pdf_title { get; set; }
        [NotMapped]
        public HttpPostedFileBase Http_img { get; set; }
    }
}
