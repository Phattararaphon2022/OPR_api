using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTDep
    {

        public cls_MTDep() { }
        public int dep_id { get; set; }
        public string dep_code { get; set; }
        public string dep_name_th { get; set; }
        public string dep_name_en { get; set; }
        public string dep_parent { get; set; }
        public string dep_level { get; set; }
        public string parent_level { get; set; }
        public string company_code { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
    }
}
