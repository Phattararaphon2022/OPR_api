using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRDocatt
    {
        public cls_TRDocatt() { }
        public int reqdoc_id { get; set; }
        public int reqdoc_att_no { get; set; }
        public string reqdoc_att_file_name { get; set; }
        public string reqdoc_att_file_type{ get; set; }
        public string reqdoc_att_path { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
    }
}
