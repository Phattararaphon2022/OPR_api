using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRProjobcost
    {
        public cls_TRProjobcost() { }

        public int projobcost_id { get; set; }
        public string projobcost_code { get; set; }       
        public double projobcost_amount { get; set; }
       
        public string version { get; set; }
        public string projobcost_status { get; set; }

        public bool projobcost_auto { get; set; }

        public string projob_code { get; set; }    
        public string project_code { get; set; }
                   
	    public string created_by { get;set; }
        public DateTime created_date { get; set; }
	    public string modified_by { get;set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

        //-- Show
        public string procost_type { get; set; }       
    }
}
