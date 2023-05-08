using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.SYS.System
{
    public class cls_MTRounds
    {
        public cls_MTRounds() { }
        public int rounds_id { get; set; }
        public string rounds_code { get; set; }
        public string rounds_name_th { get; set; }
        public string rounds_name_en { get; set; }

        public string rounds_from { get; set; }
        public string rounds_to { get; set; }
        public string rounds_result { get; set; }
        public string rounds_group { get; set; }

        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }
    }
}
