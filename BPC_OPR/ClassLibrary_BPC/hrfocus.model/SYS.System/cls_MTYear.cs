using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTYear
    {
        public cls_MTYear() { }
        public string company_code { get; set; }
        public string year_id { get; set; }
        public string year_code { get; set; }
        public string year_name_th { get; set; }
        public string year_name_en { get; set; }
        public DateTime year_fromdate { get; set; }
        public DateTime year_todate { get; set; }
        public string year_group { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }
    }
}
