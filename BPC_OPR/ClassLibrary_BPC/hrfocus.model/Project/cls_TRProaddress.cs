using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRProaddress
    {
        public cls_TRProaddress() { }

        public int proaddress_id { get; set; }
                      
        public string proaddress_type { get; set; }
        public string proaddress_no { get; set; }
        public string proaddress_moo { get; set; }
        public string proaddress_soi { get; set; }
        public string proaddress_road { get; set; }
        public string proaddress_tambon { get; set; }
        public string proaddress_amphur { get; set; }
        public string proaddress_zipcode { get; set; }
        public string proaddress_tel { get; set; }
        public string proaddress_email { get; set; }
        public string proaddress_line { get; set; }
        public string proaddress_facebook { get; set; }
        public string province_code { get; set; }

        public string project_code { get; set; }  
                
	    public string created_by { get;set; }
        public DateTime created_date { get; set; }
	    public string modified_by { get;set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }
    }
}
