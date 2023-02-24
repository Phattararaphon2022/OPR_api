using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRCriminal
    {
        public cls_TRCriminal() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }

        public int empcriminal_id { get; set;}
        public string empcriminal_location { get; set; }
        public DateTime empcriminal_fromdate { get; set; }
        public DateTime empcriminal_todate { get; set; }
        public double empcriminal_count { get; set; }
        public string empcriminal_result { get;set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }


        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }
    }
}
