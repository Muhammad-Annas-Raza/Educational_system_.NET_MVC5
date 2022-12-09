using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_eStudiez.OtherMethods
{
    public class tbl_login
    {
        [Required(ErrorMessage = "Name is required")]
        public string user_name { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string user_password { get; set; }
    }
}