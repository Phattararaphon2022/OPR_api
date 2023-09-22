using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTForetype
    {
        public cls_MTForetype() { }
        public int foretype_id { get; set; }
        public string foretype_code { get; set; }
        public string foretype_name_th { get; set; }
        public string foretype_name_en { get; set; }
        public string company_code { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }
    }
}
