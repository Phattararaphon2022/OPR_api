using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRAddress
    {
        public cls_TRAddress(){ }

        public string company_code { get; set; }
        public string worker_code { get; set; }
        public int address_id { get; set; }
        public string address_type { get; set; }
        public string address_no { get; set; }
        public string address_moo { get; set; }
        public string address_soi { get; set; }
        public string address_road { get; set; }
        public string address_tambon { get; set; }
        public string address_amphur { get; set; }
        public string province_code { get; set; }
        public string address_zipcode { get; set; }
        public string address_tel { get; set; }
        public string address_email { get; set; }
        public string address_line { get; set; }
        public string address_facebook { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }
    }
}
