using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRProjobworking
    {
        public cls_TRProjobworking() { }

        public int projobworking_id { get; set; }
        public string projobworking_emp { get; set; }
        public DateTime projobworking_workdate { get; set; }
        public DateTime projobworking_in { get; set; }
        public DateTime projobworking_out { get; set; }
        public string projobworking_status { get; set; }

        public string projob_code { get; set; }    
        public string project_code { get; set; }
                   
	    public string created_by { get;set; }
        public DateTime created_date { get; set; }
	    public string modified_by { get;set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }
    }
}
