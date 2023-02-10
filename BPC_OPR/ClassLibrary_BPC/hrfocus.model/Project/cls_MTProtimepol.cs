using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTProtimepol
    {
        public cls_MTProtimepol() { }
        public int protimepol_id { get; set; }
        public string protimepol_code { get; set; }
        public string protimepol_name_th { get; set; }
        public string protimepol_name_en { get; set; }

        public string protimepol_ot { get; set; }
        public string protimepol_allw { get; set; }
        public string protimepol_dg { get; set; }
        public string protimepol_lv { get; set; }
        public string protimepol_lt { get; set; }

        public string project_code { get; set; }
        
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }   
    }
}
