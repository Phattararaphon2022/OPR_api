using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TREmppolatt
    {
        public cls_TREmppolatt() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }
        public string emppolatt_policy_code { get; set; }
        public string emppolatt_policy_type { get; set; }
        public string emppolatt_policy_note { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public bool flag { get; set; }

    }
}
