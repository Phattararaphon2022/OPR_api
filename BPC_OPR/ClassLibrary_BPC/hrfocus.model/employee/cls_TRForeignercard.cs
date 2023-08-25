using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRForeignercard
    {
        public cls_TRForeignercard() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }

        public int foreignercard_id { get; set; }
        public string foreignercard_code { get; set; }

        public string foreignercard_type { get; set; }

        public DateTime foreignercard_issue { get; set; }
        public DateTime foreignercard_expire { get; set; }


        public string created_by { get; set; }
        public DateTime created_date { get; set; }


        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }
    }
}
