using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRProjobcontract
    {
        public cls_TRProjobcontract() { }

        public int projobcontract_id { get; set; }
        public string projobcontract_ref { get; set; }

        public int projobcontract_working { get; set; }
        public double projobcontract_hrsperday { get; set; }
        public double projobcontract_hrsot { get; set; }

        public string projob_code { get; set; }
        public string project_code { get; set; }
        public string version { get; set; }
                       
	    public string created_by { get;set; }
        public DateTime created_date { get; set; }
	    public string modified_by { get;set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }
    }
}
