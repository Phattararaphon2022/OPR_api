using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.SYS.System
{
    public class cls_MTRound
    {
        public cls_MTRound() { }

        public int round_id { get; set; }
        public string round_code { get; set; }
        public string round_name_th { get; set; }
        public string round_name_en { get; set; }

        public string round_group { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }


        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    }
}
