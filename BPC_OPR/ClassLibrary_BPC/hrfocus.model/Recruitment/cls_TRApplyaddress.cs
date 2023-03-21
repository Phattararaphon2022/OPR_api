using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Recruitment
{
    public class cls_TRApplyaddress
    {
        public cls_TRApplyaddress() { }

        public string company_code { get; set; }
        public string applywork_code { get; set; }
        public int applyaddress_id { get; set; }
        public string applyaddress_type { get; set; }
        public string applyaddress_no { get; set; }
        public string applyaddress_moo { get; set; }
        public string applyaddress_soi { get; set; }
        public string applyaddress_road { get; set; }
        public string applyaddress_tambon { get; set; }
        public string applyaddress_amphur { get; set; }
        public string applyprovince_code { get; set; }
        public string applyaddress_zipcode { get; set; }
        public string applyaddress_tel { get; set; }
        public string applyaddress_email { get; set; }
        public string applyaddress_line { get; set; }
        public string applyaddress_facebook { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }
    }
}
