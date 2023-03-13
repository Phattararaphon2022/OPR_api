using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTReason
    {
        public cls_MTReason() { }
        public string company_code { get; set; }
        public int reason_id { get; set; }
        public string reason_code { get; set; }
        public string reason_name_th { get; set; }
        public string reason_name_en { get; set; }
        public string reason_group { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }
    }
}
