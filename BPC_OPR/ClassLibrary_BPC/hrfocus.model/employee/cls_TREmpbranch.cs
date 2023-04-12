using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TREmpbranch
    {
        public cls_TREmpbranch() { }
        public string company_code { get; set; }
        public string worker_code { get; set; }
        public string branch_code { get; set; }
        public DateTime empbranch_startdate { get; set; }
        public DateTime empbranch_enddate { get; set; }
        public string empbranch_note { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
    }
}
