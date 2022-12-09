namespace Project_eStudiez.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    public partial class tbl_user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_user()
        {
            tbl_mark = new HashSet<tbl_mark>();
        }

        [Key]
        public int user_id { get; set; }
        public string user_name { get; set; }
       

        public string user_address { get; set; }

        public string user_password { get; set; }

        public string user_image { get; set; }

        public string user_phone { get; set; }

        public int? user_age { get; set; }

        public int fk_role_id { get; set; }

        [StringLength(150)]
        [Index(IsUnique = true)]
        public string user_email { get; set; }

        public int user_approved { get; set; }

        public string user_std_enrollment_no { get; set; }

        public int? fk_standard_id { get; set; }

        public string user_verification_code { get; set; }

        [Required]
        [StringLength(100)]
        public string user_email_status { get; set; }
        [NotMapped]
        public HttpPostedFileBase Http_img { get; set; }

        [NotMapped]
        public string user_confirm_password { get; set; }
     

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_mark> tbl_mark { get; set; }

        public virtual tbl_role tbl_role { get; set; }

        public virtual tbl_standard tbl_standard { get; set; }
    }
}
