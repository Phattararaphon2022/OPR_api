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

        public string comaddress_type { get; set; }

        public string comaddressth_no { get; set; }
        public string comaddressth_moo { get; set; }
        public string comaddressth_soi { get; set; }
        public string comaddressth_road { get; set; }
        public string comaddressth_tambon { get; set; }
        public string comaddressth_amphur { get; set; }
        public string provinceth_code { get; set; }

        public string comaddressen_no { get; set; }
        public string comaddressen_moo { get; set; }
        public string comaddressen_soi { get; set; }
        public string comaddressen_road { get; set; }
        public string comaddressen_tambon { get; set; }
        public string comaddressen_amphur { get; set; }
        public string comaddress_zipcode { get; set; }
        public string provinceen_code { get; set; }


        public string comaddress_tel { get; set; }
        public string comaddress_email { get; set; }
        public string comaddress_line { get; set; }
        public string comaddress_facebook { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    }
}
