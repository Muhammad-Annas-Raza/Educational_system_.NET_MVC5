using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project_eStudiez.Models;


namespace Project_eStudiez.OtherMethods
{
    public class Class3in1
    {
        public List<tbl_link> lst_tbl_link { get; set; }
        public List<tbl_pdf> lst_tbl_pdf { get; set; }
        public List<tbl_extraclass> lst_tbl_extraclass { get; set; }
        public tbl_extraclass row_extraclass { get; set; }
    }
}