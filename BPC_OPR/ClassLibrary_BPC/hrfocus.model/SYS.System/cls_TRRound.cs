using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRRound
    {
        public cls_TRRound() { }
        public string company_code { get; set; }
        public string round_id { get; set; }
        public double round_from { get; set; }
        public double round_to { get; set; }
        public double round_result { get; set; }
    }
}
