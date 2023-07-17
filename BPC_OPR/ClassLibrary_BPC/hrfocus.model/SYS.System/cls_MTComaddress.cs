using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
     {
    public class cls_MTComaddress
    {
        public cls_MTComaddress() { }

        public string company_code { get; set; }
        public string combranch_code { get; set; }

        public string comaddres_type { get; set; }

        public string comaddres_noth { get; set; }
        public string comaddres_mooth { get; set; }
        public string comaddres_soith { get; set; }
        public string comaddres_roadth { get; set; }
        public string comaddres_tambonth { get; set; }
        public string comaddres_amphurth { get; set; }

        public string comaddres_noen { get; set; }
        public string comaddres_mooen { get; set; }
        public string comaddres_soien { get; set; }
        public string comaddres_roaden { get; set; }
        public string comaddres_tambonen { get; set; }
        public string comaddres_amphuren { get; set; }
        public string comaddres_zipcode { get; set; }

        public string province_code { get; set; }


        public string comaddres_tel { get; set; }
        public string comaddres_email { get; set; }
        public string comaddres_line { get; set; }
        public string comaddres_facebook { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    }
}
