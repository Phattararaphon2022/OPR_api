using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Attendance
{
    public class cls_TREmpattitem
    {
        public cls_TREmpattitem() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }

        public string empattitem_sa { get; set; }
        public string empattitem_ot { get; set; }
        public string empattitem_aw { get; set; }
        public string empattitem_dg { get; set; }
        public string empattitem_lv { get; set; }
        public string empattitem_ab { get; set; }
        public string empattitem_lt { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }

        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    }
}
