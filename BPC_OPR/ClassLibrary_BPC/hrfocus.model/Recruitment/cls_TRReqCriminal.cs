using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRReqCriminal
    {
        public cls_TRReqCriminal() { }

        public string company_code { get; set; }
        public string applywork_code { get; set; }

        public int reqcriminal_id { get; set; }
        public string reqcriminal_location { get; set; }
        public DateTime reqcriminal_fromdate { get; set; }
        public DateTime reqcriminal_todate { get; set; }
        public double reqcriminal_count { get; set; }
        public string reqcriminal_result { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }


        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }
    }
}
